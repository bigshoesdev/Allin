const Sequelize = require('sequelize');

module.exports = (sequelize) => {
    const model = sequelize.define('blockword', {
        id: { type: Sequelize.INTEGER, primaryKey: true, autoIncrement: true },
        word: { type: Sequelize.STRING, allowNull: false },
        regdate: { type: Sequelize.DATE },
    }, { timestamps: false, tableName: 'TBL_BLOCKWORD' })

    return model;
};