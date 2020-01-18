const Sequelize = require('sequelize');

module.exports = (sequelize) => {

    var Charge = {
        async recordCharge(userid, couponname, money, username) {
            var strSQL = "INSERT INTO tbl_charge(userid,couponname,money, name) VALUES(?,?,?,?)";
            var result = await sequelize.query(strSQL, { replacements: [userid, couponname, money, username] });
        },
    }

    return Charge;
};