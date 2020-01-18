const Sequelize = require('sequelize');

module.exports = (sequelize) => {
    let model = sequelize.define('setting', {
        id: { type: Sequelize.INTEGER, primaryKey: true, autoIncrement: true },
        notice_scroll: { type: Sequelize.STRING },
        notice_content: { type: Sequelize.STRING },
        notice_title: { type: Sequelize.STRING },
        login_url: { type: Sequelize.STRING },
        login_show: { type: Sequelize.INTEGER },
        changedate: { type: Sequelize.DATE, defaultValue: Sequelize.literal('CURRENT_TIMESTAMP') }
    }, { timestamps: false, tableName: 'TBL_ADMININFO' })

    model.getNoiceMessage = async function () {
        try {
            let setting = await this.findOne();

            if (setting) {
                return setting.notice_content;
            } else {
                return "";
            }
        } catch (e) {
            console.log(e);
        }
    }

    model.getNoticeTitle = async function () {
        try {
            let setting = await this.findOne();

            if (setting) {
                return setting.notice_title;
            } else {
                return "";
            }
        } catch (e) {
            console.log(e);
        }
    }

    model.getNoticeScroll = async function () {
        try {
            let setting = await this.findOne();

            if (setting) {
                return setting.notice_scroll;
            } else {
                return "";
            }
        } catch (e) {
            console.log(e);
        }
    }

    model.getLoginShow = async function () {
        try {
            let setting = await this.findOne();

            if (setting) {
                return setting.login_show > 0 ? true : false;
            } else {
                return false;
            }
        } catch (e) {
            console.log(e);
        }
    }
    
    model.getLoginImageUrl = async function () {
        try {
            let setting = await this.findOne();

            if (setting) {
                return setting.login_url;
            } else {
                return "";
            }
        } catch (e) {
            console.log(e);
        }
    }


    return model;
};