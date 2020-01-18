const config = require('config.json');
const Sequelize = require('sequelize');

const sequelize = new Sequelize(config.gatewaydbName, config.gatewaydbUser, config.gatewaydbPass, {
    dialect: 'mssql',
    operatorsAliases: false,
    logging: false
});


module.exports = {
    User: require('../app_modules/users/gateway_user.model')(sequelize),
    Coupon: require('../app_modules/exchangeHistories/gateway_coupon.model')(sequelize),
    Withdraw: require('../app_modules/exchangeHistories/gateway_withdraw.model')(sequelize),
    Charge: require('../app_modules/exchangeHistories/gateway_charge.model')(sequelize),
    Notice: require('../app_modules/notices/gateway_notice.model')(sequelize),
    BlockWord: require('../app_modules/blockwords/gateway_blockword.model')(sequelize),
    Setting:require('../app_modules/users/gateway_setting.model')(sequelize),
};