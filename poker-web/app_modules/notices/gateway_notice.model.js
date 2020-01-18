const Sequelize = require('sequelize');

module.exports = (sequelize) => {
    const model = sequelize.define('notice', {
        id: { type: Sequelize.INTEGER, primaryKey: true, autoIncrement: true },
        title: { type: Sequelize.STRING, allowNull: false },
        textcontent: { type: Sequelize.STRING, allowNull: false },
        regdate: { type: Sequelize.DATE },
    }, {
            getterMethods: {
                ncontent() {
                    return this.textcontent;
                },
                createdDate() {
                    return this.regdate;
                }
            },
            timestamps: false,
            tableName: 'TBL_BOARD'
        })

    return model;
};