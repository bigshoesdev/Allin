const Sequelize = require('sequelize');

module.exports = (sequelize) => {

    var GameHistory = {
        async recordGameHistory(entrycount, room, panmoney, entrynicknames, entrystartmonies, entrychangemonies, entrycards, entryids, entrytypes, betmonies) {
            var regdate = (new Date()).toLocaleString();
            var queryStr = "entrycount,room,panmoney,regdate,";
            var valueStr = "?,?,?,?,";
            var replacements = [entrycount, room, panmoney, regdate];
            for (var i = 0; i < 9; i++) {
                if (entrynicknames[i]) {
                    queryStr += "entrynickname" + (i + 1) + ",";
                    valueStr += "?,";
                    replacements.push(entrynicknames[i]);
                    queryStr += "entrystartmoney" + (i + 1) + ",";
                    valueStr += "?,";
                    replacements.push(entrystartmonies[i]);
                    queryStr += "entrychangemoney" + (i + 1) + ",";
                    valueStr += "?,";
                    replacements.push(entrychangemonies[i]);
                    queryStr += "entrycard" + (i + 1) + ",";
                    valueStr += "?,";
                    replacements.push(entrycards[i]);
                    queryStr += "entryid" + (i + 1) + ",";
                    valueStr += "?,";
                    var entryid = entryids[i];
                    if (isNaN(entryid))
                        entryid = 0;
                    replacements.push(entryid);
                    queryStr += "entrytype" + (i + 1) + ",";
                    valueStr += "?,";
                    replacements.push(entrytypes[i]);
                    queryStr += "bet_money" + (i + 1) + ",";
                    valueStr += "?,";
                    replacements.push(betmonies[i]);
                }
            }

            queryStr += "service_fee";
            valueStr += "?";
            replacements.push(global.managerFee);

            var strSQL = "INSERT INTO tbl_gamehist(" + queryStr + ") VALUES(" + valueStr + ");";
            await sequelize.query(strSQL, { replacements: replacements });
        },
    }

    return GameHistory;
};