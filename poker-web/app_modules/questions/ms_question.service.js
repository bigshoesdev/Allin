const config = require('config.json');
const jwt = require('jsonwebtoken');
const bcrypt = require('bcryptjs');
const msDB = require('_helpers/ms_db');
const Question = msDB.Question;

module.exports = {
    getAll,
    getById,
    create,
    update,
    register,
    delete: _delete
};

async function getAll() {
    return await Question.findAll({
        order: [['createdDate', 'DESC']]
    }).map(model => model.toJSON());
}

async function getById(id) {
    let question = await Question.findByPk(id);

    return question.toJSON();
}

async function create(questionParam) {
    // validate
    if (questionParam.uid == "" || questionParam.title == "" || questionParam.ncontent == "") {
        throw 'Qeustion incorrect';
    }

    const question = Question.build(questionParam);

    // save user
    await question.save();
}


async function register(questionData) {
    // validate
    if (questionData.uid == "" || questionData.title == "" || questionData.ncontent == "") {
        return { success: false, res: 'questionData incorrect' };
    }

    try {
        var question = Question.build(questionData);

        await question.save();

        return { success: true, res: 'Rec question success' };
    } catch (err) {
        console.log(err);
        return { success: false, res: 'Database save error' };
    }
}

async function update(id, questionBody) {
    const question = await Question.findByPk(id);

    // validate
    if (!question) throw 'Question not found';

    Object.assign(question, questionBody);

    await question.save();
}

async function _delete(id) {
    await Question.destroy({
        where: {
            id: id
        }
    });
}