const Sequelize = require('sequelize');

module.exports = (sequelize) => {
    const model = sequelize.define('award', {
        id: { type: Sequelize.INTEGER, primaryKey: true, autoIncrement: true },
        startRank: { type: Sequelize.INTEGER, allowNull: false, get() { return parseInt(this.getDataValue('startRank')) } },
        endRank: { type: Sequelize.INTEGER, allowNull: false, get() { return parseInt(this.getDataValue('endRank')) } },
        award: { type: Sequelize.INTEGER, allowNull: false, get() { return parseInt(this.getDataValue('award')) } },
        createdDate: { type: Sequelize.DATE, defaultValue: Sequelize.literal('CURRENT_TIMESTAMP') }
    }, { timestamps: false, tableName: 'AWARD' })

    return model;
};