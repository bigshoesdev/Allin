const Sequelize = require('sequelize');

module.exports = (sequelize) => {
    const model = sequelize.define('gifthistory', {
        id: { type: Sequelize.INTEGER, primaryKey: true, autoIncrement: true },
        sentUID: { type: Sequelize.INTEGER, allowNull: false, get() { return parseInt(this.getDataValue('sentChips')) } },
        sentUserID: { type: Sequelize.STRING, allowNull: false },
        sentUserName: { type: Sequelize.STRING, allowNull: false },
        receiveUID: { type: Sequelize.INTEGER, allowNull: false, get() { return parseInt(this.getDataValue('sentChips')) } },
        receiveUserID: { type: Sequelize.STRING, allowNull: false },
        receiveUserName: { type: Sequelize.STRING, allowNull: false },
        sentChips: { type: Sequelize.INTEGER, allowNull: false, get() { return parseInt(this.getDataValue('sentChips')) } },
        createdDate: { type: Sequelize.DATE, defaultValue: Sequelize.literal('CURRENT_TIMESTAMP') }
    }, { timestamps: false, tableName: 'TBL_GIFTHISTORY' })

    return model;
};