const Sequelize = require('sequelize');

module.exports = (sequelize) => {

    var User = {
        async findAll() {
            var result = await sequelize.query('SELECT * FROM tbl_userlist', { type: Sequelize.QueryTypes.SELECT });
            return result.map(this.getUserInfo);
        },
        async getUserByLoginIDAndPass(loginid, loginpwd) {
            var result = await sequelize.query('SELECT * FROM tbl_userlist WHERE loginid=? AND loginpwd=?', { replacements: [loginid, loginpwd], type: Sequelize.QueryTypes.SELECT });
            return result.map(this.getUserInfo);
        },

        async checkIPBlocked(ipaddress) {
            var result = await sequelize.query('SELECT COUNT(*) as count FROM tbl_blockip WHERE ipaddr=?', { replacements: [ipaddress], type: Sequelize.QueryTypes.SELECT });

            if (parseInt(result[0]['count']) > 0)
                return true;
            else
                return false;
        },
        async setUserManagerID(id, partner) {
            var result = await sequelize.query('SELECT * FROM tbl_enterprise WHERE partner=?', { replacements: [partner], type: Sequelize.QueryTypes.SELECT });

            if (result.length == 0) {
                return 0;
            } else {
                var nBonsaID = 0;
                var nBubonsaID = 0;
                var nChongpanID = 0;
                var nMaejangID = 0;
                var nParentID = 0;
                var nClass = 0;                         // 부류번호(1-매장,2-총판,3-부본사,4-본사,5-슈퍼)
                var strBonsaName = "";
                var strBubonsaName = "";
                var strChongpanName = "";
                var strMaejangName = "";

                strBonsaName = result[0]["bonsa"];
                strBubonsaName = result[0]["bubonsa"];
                strChongpanName = result[0]["chongpan"];

                nClass = parseInt(result[0]["class"]);
                nBonsaID = result[0]["bonsaid"];
                nBubonsaID = result[0]["bubonsaid"];
                nChongpanID = result[0]["chongpanid"];

                if (nClass == 1) {
                    // 매장소속인 경우
                    nMaejangID = parseInt(result[0]["id"]);
                    nParentID = nMaejangID;
                    strMaejangName = result[0]["name"];
                }
                else if (nClass == 2) {
                    // 총판 소속인 경우
                    nChongpanID = parseInt(result[0]["id"]);
                    nParentID = nChongpanID;
                    strChongpanName = result[0]["name"];
                }
                else if (nClass == 3) {
                    // 부본사직속인 경우
                    nBubonsaID = parseInt(result[0]["id"]);
                    nParentID = nBubonsaID;
                    strBubonsaName = result[0]["name"];
                }
                else if (nClass == 4) {
                    // 본사직속인 경우
                    nBonsaID = parseInt(result[0]["id"]);
                    nParentID = nBonsaID;
                    strBonsaName = result[0]["name"];
                }

                var strQuery = "UPDATE tbl_userlist " +
                    "SET bonsa=?,bubonsa=?,chongpan=?,maejang=?,parentid=?,bonsaid=?,bubonsaid=?,chongpanid=?,maejangid=?,partner=? " +
                    "WHERE id=?";

                var result = await sequelize.query(strQuery, { replacements: [strBonsaName, strBubonsaName, strChongpanName, strMaejangName, nParentID, nBonsaID, nBubonsaID, nChongpanID, nMaejangID, partner, id] });

                return nBonsaID;
            }
        },
        async recordLoginHistory(id, ipaddress) {
            var starttime = (new Date()).toLocaleString();

            var strSQL = "INSERT INTO tbl_loginhist(userid,ipaddr,clientid,starttime) VALUES(?,?,?,?)";
            var result = await sequelize.query(strSQL, { replacements: [id, ipaddress, "", starttime] });
        },
        async recordLogoutHistory(userid) {
            var endtime = (new Date()).toLocaleString();
            var strSQL = "UPDATE tbl_loginhist SET endtime=? WHERE id IN ( SELECT id FROM tbl_loginhist WHERE userid=? and endtime is null)";
            var result = await sequelize.query(strSQL, { replacements: [endtime, userid] });
        },
        async updateRecommendFee(id) {
            try {
                var strSQL = "DECLARE @return_value int; EXEC @return_value = P_UPDATE_ONE_RECOMMEND_FEE @recommender_id=?;SELECT 'return' = @return_value";
                var result = await sequelize.query(strSQL, { replacements: [id], type: Sequelize.QueryTypes.SELECT });

                return result[0]['return'];
            } catch (e) {
                console.log(e);
            }
        },
        async checkPartnerExist(partner) {
            strSQL = "SELECT COUNT(*) as count FROM tbl_enterprise WHERE partner=?";

            var result = await sequelize.query(strSQL, { replacements: [partner], type: Sequelize.QueryTypes.SELECT });

            if (parseInt(result[0]['count']) > 0)
                return true;
            else
                return false;
        },
        async checkUserIDExist(loginid) {
            strSQL = "SELECT COUNT(*) as count FROM tbl_userlist WHERE loginid=?";

            var result = await sequelize.query(strSQL, { replacements: [loginid], type: Sequelize.QueryTypes.SELECT });
            if (parseInt(result[0]['count']) > 0)
                return true;
            else
                return false;
        },
        async checkUserNickNameExist(nickname) {
            strSQL = "SELECT COUNT(*) as count FROM tbl_userlist WHERE nickname=?";

            var result = await sequelize.query(strSQL, { replacements: [nickname], type: Sequelize.QueryTypes.SELECT });
            if (parseInt(result[0]['count']) > 0)
                return true;
            else
                return false;
        },
        async registerNewUser(userParam, ipaddress) {
            try {
                var strSQL = "INSERT INTO tbl_userlist(loginid,loginpwd,name,telnum,nickname,gamemoney,partner,recommender_dbid,recommender_loginid,loginip,bankName,bankAccountNumber,depositHolder,currencyExPassword) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                await sequelize.query(strSQL, { replacements: [userParam.userID, userParam.password, userParam.depositHolder, userParam.phoneNumber, userParam.nickName, 0, userParam.referID, null, null, ipaddress, userParam.bankName, userParam.bankAccountNumber, userParam.depositHolder, userParam.currencyExPassword] });
                return true;
            } catch (e) {
                return false;
            }
        },
        async findByPk(uid) {
            var result = await sequelize.query('SELECT  TOP 1 * FROM tbl_userlist WHERE id=?', { replacements: [uid], type: Sequelize.QueryTypes.SELECT });

            if (result.length > 0) {
                var user = this.getUserInfo(result[0]);
                return user;
            } else
                return null;
        },
        async findByUserID(uid) {
            var result = await sequelize.query('SELECT  TOP 1 * FROM tbl_userlist WHERE loginid=?', { replacements: [uid], type: Sequelize.QueryTypes.SELECT });

            if (result.length > 0) {
                var user = this.getUserInfo(result[0]);
                return user;
            } else
                return null;
        },
        async findByNickName(nickname) {
            var result = await sequelize.query('SELECT  TOP 1 * FROM tbl_userlist WHERE nickname=?', { replacements: [nickname], type: Sequelize.QueryTypes.SELECT });

            if (result.length > 0) {
                var user = this.getUserInfo(result[0]);
                return user;
            } else
                return null;
        },
        async findAllSpecials() {
            var result = await sequelize.query('SELECT * FROM tbl_userlist where isviewer=1', { type: Sequelize.QueryTypes.SELECT });
            return result.map(this.getUserInfo);
        },
        async findAllViews() {
            var result = await sequelize.query('SELECT * FROM tbl_userlist where isviewer=2', { type: Sequelize.QueryTypes.SELECT });
            return result.map(this.getUserInfo);
        },
        async updateGameType(id, gametype) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET gametype=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [gametype, id] });
        },
        async updateAvatarNo(id, avatarNo) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET avatarNo=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [avatarNo, id] });
        },
        async updateFrontDesk(id, frontDesk) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET front_desk=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [frontDesk, id] });
        },
        async updateBackDesk(id, backDesk) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET back_desk=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [backDesk, id] });
        },
        async updateFelt(id, felt) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET felt=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [felt, id] });
        },
        async updateBackground(id, background) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET background=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [background, id] });
        },
        async updateNoLogin(id, nologin) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET nologin=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [nologin, id] });
        },
        async updateLoginFlag(id, loginFlag) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET loginFlag=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [loginFlag, id] });
        },
        async updateStatus(id, status) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET status=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [status, id] });
        },
        async updateIsNew(id, is_new) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET is_new=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [is_new, id] });
        },
        async updateChips(id, chips) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET gamemoney=? " +
                "WHERE id=?";
            await sequelize.query(strQuery, { replacements: [chips, id] });
        },
        async updateChipsByAdd(id, chips) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET gamemoney+=? " +
                "WHERE id=?";
            await sequelize.query(strQuery, { replacements: [chips, id] });
        },
        async updatePointMoney(id, money) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET dealmoney=? " +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [money, id] });
        },
        async updateWinLostNumber(id, winNumber, lostNumber) {
            var strQuery = "UPDATE tbl_userlist " +
                "SET allinwincount=? , allinlosecount=?" +
                "WHERE id=?";

            await sequelize.query(strQuery, { replacements: [winNumber, lostNumber, id] });
        },
        async addPartnerMoney(userid, money) {
            try {
                var strSQL = "EXEC P_US_ADD_PARTNER_MONEY @user_id=?, @bet_money=?";
                var result = await sequelize.query(strSQL, { replacements: [userid, money] });
            } catch (e) {
                console.log(e);
            }
        },
        getUserInfo(u) {
            return {
                id: u.id,
                nologin: u.nologin,
                bonsaid: u.bonsaid,
                partner: u.partner,
                winNumber: parseInt(u.allinwincount ? u.allinwincount : 0),
                lostNumber: parseInt(u.allinlosecount ? u.allinlosecount : 0),
                depositHolder: u.depositHolder ? u.depositHolder : "",
                currencyExPassword: u.currencyExPassword ? u.currencyExPassword : "",
                userID: u.loginid ? u.loginid : "",
                userPwd: u.loginpwd ? u.loginpwd : "",
                nickName: u.nickname ? u.nickname : "",
                phoneNumber: u.telnum ? u.telnum : "",
                bankName: u.bankName ? u.bankName : "",
                bankAccountNumber: u.bankAccountNumber ? u.bankAccountNumber : "",
                point: parseInt(u.dealmoney ? u.dealmoney : 0),
                chips: parseInt(u.gamemoney ? u.gamemoney : 0),
                avatarNo: parseInt(u.avatarNo ? u.avatarNo : 0),
                loginFlag: u.loginFlag ? u.loginFlag : false,
                parentid: parseInt(u.parentid ? u.parentid : 0),
                stopchat: u.stopchat > 0 ? true : false,
                backDesk: parseInt(u.back_desk ? u.back_desk : 0),
                frontDesk: parseInt(u.front_desk ? u.front_desk : 0),
                background: parseInt(u.background ? u.background : 0),
                felt: parseInt(u.felt ? u.felt : 0),
                status: u.status ? u.status : false,
            }
        },
    }

    return User;
};