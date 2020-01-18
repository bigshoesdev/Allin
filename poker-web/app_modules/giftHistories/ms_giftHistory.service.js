const config = require('config.json');
const msDB = require('_helpers/ms_db');
const gatewayDB = require('_helpers/gateway_db');
const GiftHistory = msDB.GiftHistory;
const User = gatewayDB.User;

module.exports = {
    register,
    getAll,
    getByUserId,
    delete: _delete
};

async function getAll() {
    return await GiftHistory.findAll({
        order: [['createdDate', 'DESC']]
    }).map(model => model.toJSON());
}

async function getByUserId(uid) {
    return await GiftHistory.findAll({
        where: { sentUserID: uid },
        order: [['createdDate', 'DESC']]
    }).map(model => model.toJSON());
}

async function register(giftData) {
    var userId = giftData.userId,
        chipsTogive = parseInt(giftData.sentChips),
        nickNameToGift = giftData.nickNameToGift;
    currencyExPassword = giftData.currencyExPassword;
    console.log('%%%%%% giftData in giftService');

    if (userId == "" || chipsTogive <= 0 || nickNameToGift == "") {
        return { success: false, res: 'giftHistory Data incorrect' };
    }

    try {
        var sentUser = await User.findByPk(userId);

        if (sentUser.currencyExPassword !== currencyExPassword) {
            return { success: false, res: 'Invalid currencyExPassword' };
        }

        var userToGift = await User.findByUserID(nickNameToGift);

        if (!userToGift) {
            return { success: false, res: 'Invalid user to give gift' };
        }


        sentUser.chips -= chipsTogive;
        userToGift.chips += chipsTogive;

        await User.updateChips(sentUser.id, sentUser.chips);
        await User.updateChips(userToGift.id, userToGift.chips);

        var data = {};

        data.sentUID = sentUser.id;
        data.sentUserID = sentUser.userID;
        data.sentUserName = sentUser.nickName;
        data.receiveUID = userToGift.id;
        data.receiveUserID = userToGift.userID;
        data.receiveUserName = userToGift.nickName;
        data.sentChips = giftData.sentChips;

        var history = GiftHistory.build(data);

        await history.save();

        return { success: true, res: 'Giving gift success' };

    } catch (err) {
        console.log(err);
        return { success: false, res: 'Database save error' };
    }
}

async function _delete(id) {
    await GiftHistory.destroy({
        where: {
            id: id
        }
    });
}