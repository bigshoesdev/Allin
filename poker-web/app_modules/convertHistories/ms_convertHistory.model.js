const Sequelize = require('sequelize');

module.exports = (sequelize) => {
    const model = sequelize.define('converthistory', {
        id: { type: Sequelize.INTEGER, primaryKey: true, autoIncrement: true },
        userId: { type: Sequelize.STRING, allowNull: false },
        userName: { type: Sequelize.STRING, allowNull: false },
        convertPoints: { type: Sequelize.INTEGER, allowNull: false, get() { return parseInt(this.getDataValue('convertPoints')) } },
        remaindPoints: { type: Sequelize.INTEGER, allowNull: false, get() { return parseInt(this.getDataValue('remaindPoints')) } },
        createdDate: { type: Sequelize.DATE, defaultValue: Sequelize.literal('CURRENT_TIMESTAMP') }
    }, { timestamps: false, tableName: 'TBL_CONVERTHISTORY' })

    return model;
};