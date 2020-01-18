const config = require('config.json');
const gatewayDB = require('_helpers/gateway_db');
const msDB = require('_helpers/ms_db');
const ConvertHistory = msDB.ConvertHistory;
const User = gatewayDB.User;

module.exports = {
    register,
    getByUserId
};

async function getByUserId(uid) {
    return await ConvertHistory.findAll({
        where: { userId: uid }
    }).map(model => model.toJSON());
}

async function register(convertData) {

    var userId = convertData.uid,
        convertPoints = parseInt(convertData.convertPoints);

    try {
        var user = await User.findByPk(userId);

        if (!user) throw 'User not found';
        if (user.point < convertPoints) {
            return { success: false, res: 'Points Lack' };
        }

        user.chips += convertPoints;
        user.point -= convertPoints;

        await User.updatePointMoney(user.id, user.point);
        await User.updateChips(user.id, user.chips);

        var history = ConvertHistory.build({ userId: userId, convertPoints: convertPoints, remaindPoints: user.point, userName: user.nickName });
        
        await history.save();

        return { success: true, res: 'Convert success' };

    } catch (err) {
        console.log(err);
        return { success: false, res: 'Database save error' };
    }

}
