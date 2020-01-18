const config = require('config.json');
const jwt = require('jsonwebtoken');
const bcrypt = require('bcryptjs');
const msDB = require('_helpers/ms_db');
const gatewayDB = require('_helpers/gateway_db');

const User = msDB.User;
const LoginHistory = msDB.LoginHistory;
const Setting = msDB.Setting;
const gatewayUser = gatewayDB.User;

module.exports = {
    authenticate,
    checkUserID,
    checkUserNickName,
    create,
    disconnect,
    updateChips,
    updateChipsByAdd,
    updateWinLostNumber,
    resetLoginFlag
};

async function authenticate(ip, { userID, password, auto }) {
    const users = await gatewayUser.getUserByLoginIDAndPass(userID, password);
    const checkIPAddress = await gatewayUser.checkIPBlocked(ip);

    if (checkIPAddress) {
        return { success: false, message: 'IP block' };
    }
    if (users.length > 0) {
        let user = users[0];

        if (user.loginFlag && auto != "auto") {
            return { success: false, message: 'duplicate' };
        } else {
            if (user.status) {
                return { success: false, message: 'not allow' };
            }

            var bonsaid = user.bonsaid ? user.bonsaid : 0;
            var partner = user.partner;

            if (partner.length > 0 && bonsaid == 0) {
                bonsaid = await gatewayUser.setUserManagerID(user.id, partner);
            }

            if (!bonsaid) {
                return { success: false, message: 'NO partner code' };
            }

            user.nologin = 0;

            await gatewayUser.updateNoLogin(user.id, user.nologin);

            return {
                success: true,
                user: user,
            };
        }

    } else {
        return { success: false, message: 'incorrect' };
    }
}

async function create(ip, userParam) {
    if (userParam.userID.length < 6)
        throw 'id length low';

    if (userParam.nickName.length < 3)
        throw 'nickname length low';

    if (await gatewayUser.checkUserIDExist(userParam.userID)) {
        throw 'userID duplicate';
    }

    if (await gatewayUser.checkUserNickNameExist(userParam.nickName)) {
        throw 'Nickname duplicate';
    }

    if (await gatewayUser.checkPartnerExist(userParam.referID) == 0) {
        throw 'Invalid partnerID';
    }

    let result =  await gatewayUser.registerNewUser(userParam, ip);

    if (!result)
        throw "Fail register";
 
    const users = await gatewayUser.getUserByLoginIDAndPass(userParam.userID, userParam.password);

    let user = users[0];

    await gatewayUser.updateStatus(user.id, 1);
    await gatewayUser.updateIsNew(user.id, 1);

    var bonsaid = user.bonsaid ? user.bonsaid : 0;
    var partner = user.partner;

    if (partner.length > 0 && bonsaid == 0) {
        bonsaid = await gatewayUser.setUserManagerID(user.id, partner);
    }
}

async function checkUserID(userParam) {
    if (userParam.userID.length < 6)
        throw 'length low';

    if (await gatewayUser.checkUserIDExist(userParam.userID)) {
        throw 'check userID duplicate';
    }
}

async function checkUserNickName(userParam) {
    if (userParam.nickname.length < 3)
        throw 'length low';

    if (await gatewayUser.checkUserNickNameExist(userParam.nickname)) {
        throw 'check userNickName duplicate';
    }
}

async function disconnect(id, autoLogin) {
    let user = await gatewayUser.findByPk(id);

    if (!user) throw 'User not found';

    user.loginFlag = false;

    if (!autoLogin) {
        await gatewayUser.updateLoginFlag(user.id, user.loginFlag);
        await gatewayUser.recordLogoutHistory(user.id);
        await LoginHistory.recordLogoutHistory(user.id);
    }

    await gatewayUser.updateGameType(user.id, "");
}

async function updateChips(id, chips) {
    await gatewayUser.updateChips(id, chips);
}

async function updateChipsByAdd(id, chips) {
    await gatewayUser.updateChipsByAdd(id, chips);
}

async function resetLoginFlag() {
    var allUsers = await gatewayUser.findAll();

    allUsers.forEach(user => {
        user.loginFlag = false;
        gatewayUser.updateLoginFlag(user.id, user.loginFlag);

        gatewayUser.updateLoginFlag(user.id, user.loginFlag);
        gatewayUser.recordLogoutHistory(user.id);
        LoginHistory.recordLogoutHistory(user.id);
    });
}

async function updateWinLostNumber(nickName, winNumber, lostNumber) {
    var user = await gatewayUser.findByNickName(nickName);

    if (!user) throw 'User not found';

    user.winNumber += winNumber;
    user.lostNumber += lostNumber;

    await gatewayUser.updateWinLostNumber(user.id, user.winNumber, user.lostNumber);
}
