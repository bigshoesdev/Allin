const Sequelize = require('sequelize');

module.exports = (sequelize) => {

    var LoginHistory = {
        async recordLoginHistory(id, ipaddress) {
            var starttime = (new Date()).toLocaleString();

            var strSQL = "INSERT INTO tbl_loginhist(userid,ipaddr,clientid,starttime) VALUES(?,?,?,?)";
            var result = await sequelize.query(strSQL, { replacements: [id, ipaddress, "", starttime] });
        },
        async recordLogoutHistory(userid) {
            var endtime = (new Date()).toLocaleString();
            var strSQL = "  UPDATE tbl_loginhist SET endtime=? WHERE id IN  ( SELECT id FROM tbl_loginhist WHERE userid=? and endtime is null)";
            var result = await sequelize.query(strSQL, { replacements: [endtime, userid] });
        },
    }

    return LoginHistory;
};