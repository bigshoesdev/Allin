const config = require('config.json');
const Sequelize = require('sequelize');

const sequelize = new Sequelize(config.dbName, config.dbUser, config.dbPass,{
    dialect: 'mssql',
    operatorsAliases: false,
    logging: false
});


module.exports = {
    LoginHistory: require('../app_modules/users/ms_loginhistory.model')(sequelize),
    GameHistory:require('../app_modules/users/ms_gamehistory.model')(sequelize),
    Setting: require('../app_modules/users/ms_setting.model')(sequelize),
    Award: require('../app_modules/awards/ms_award.model')(sequelize),
    Alarm:  require('../app_modules/alarms/ms_alarm.model')(sequelize),
    ConvertHistory: require('../app_modules/convertHistories/ms_convertHistory.model')(sequelize),
    GiftHistory: require('../app_modules/giftHistories/ms_giftHistory.model')(sequelize),
    Question: require('../app_modules/questions/ms_question.model')(sequelize),
};