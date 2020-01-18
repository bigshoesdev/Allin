const Sequelize = require('sequelize');

module.exports = (sequelize) => {

    var Withdraw = {
        async recordWithdraw(userid, bankinfo, name, nickname, money) {
            var strSQL = "INSERT INTO tbl_withdraw(userid,bankinfo,name,nickname,money,realmoney) VALUES(?,?,?,?,?,?)";
            var result = await sequelize.query(strSQL, { replacements: [userid, bankinfo, name, nickname, money, money] });
        },
    }

    return Withdraw;
};