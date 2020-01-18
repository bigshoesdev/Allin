const Sequelize = require('sequelize');

module.exports = (sequelize) => {

    var Coupon = {
        async recordCoupon(name, money,enterpriseid) {
            var strSQL = "INSERT INTO tbl_coupon(name,money,enterpriseid) VALUES(?,?,?)";
            var result = await sequelize.query(strSQL, { replacements: [name, money,enterpriseid] });
        },
    }

    return Coupon;
};