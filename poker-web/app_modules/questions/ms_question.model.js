const Sequelize = require('sequelize');

module.exports = (sequelize) => {
    const model = sequelize.define('question', {
        id: { type: Sequelize.INTEGER, primaryKey: true, autoIncrement: true },
        uid: { type: String, required: true },
        title: { type: Sequelize.STRING, allowNull: false },
        ncontent: { type: Sequelize.STRING, allowNull: false },
        answer: { type: Sequelize.STRING, defaultValue: "" },
        replied: { type: Sequelize.BOOLEAN, defaultValue: false },
        createdDate: { type: Sequelize.DATE, defaultValue: Sequelize.literal('CURRENT_TIMESTAMP') }
    }, { timestamps: false, tableName: 'QUESTION' })

    return model;
};