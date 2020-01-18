const config = require('config.json');
const gatewayDB = require('_helpers/gateway_db');
const BlockWord = gatewayDB.BlockWord;

const Sequelize = require('sequelize');
const Op = Sequelize.Op;

module.exports = {
    getCount,
    getAll
};

async function getCount(word) {
    return BlockWord.count({ where: {'word': {[Op.like]: '%' + word + '%'}} });
}

async function getAll() {
    return await BlockWord.findAll().map(model => model.toJSON());
}