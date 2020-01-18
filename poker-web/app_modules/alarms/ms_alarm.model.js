const Sequelize = require('sequelize');

module.exports = (sequelize) => {
    const model = sequelize.define('alarm', {
        id: { type: Sequelize.INTEGER, primaryKey: true, autoIncrement: true },
        kind: { type: Sequelize.STRING, allowNull: false },
        createdDate: { type: Sequelize.DATE, defaultValue: Sequelize.literal('CURRENT_TIMESTAMP') }
    }, { timestamps: false, tableName: 'ALARM' })

    return model;
};