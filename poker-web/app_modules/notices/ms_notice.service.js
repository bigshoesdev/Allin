const config = require('config.json');
const jwt = require('jsonwebtoken');
const bcrypt = require('bcryptjs');
const gatewayDB = require('_helpers/gateway_db');
const Notice = gatewayDB.Notice;

module.exports = {
    getAll,
    getById,
};

async function getAll() {
    return await Notice.findAll({
        order: [['regdate', 'DESC']]
    }).map(model => model.toJSON());
}

async function getById(id) {
    let notice = await Notice.findByPk(id);

    return notice.toJSON();
}