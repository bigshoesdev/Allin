const Sequelize = require('sequelize');

module.exports = (sequelize) => {
    let model = sequelize.define('setting', {
        id: { type: Sequelize.INTEGER, primaryKey: true, autoIncrement: true },
        mngsubtract: { type: Sequelize.FLOAT, defaultValue: 0, get() { return parseFloat(this.getDataValue('mngsubtract')) } },
        changedate: { type: Sequelize.DATE, defaultValue: Sequelize.literal('CURRENT_TIMESTAMP') }
    }, { timestamps: false, tableName: 'TBL_SETTINGINFO' })

    model.getManageFee = async function () {
        try {
            let setting = await this.findOne();

            if (setting) {
                return setting.mngsubtract;
            } else {
                return 10;
            }
        } catch (e) {
            console.log(e);
        }
    }

    return model;
};