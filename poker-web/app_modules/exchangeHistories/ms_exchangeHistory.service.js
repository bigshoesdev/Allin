const config = require('config.json');
const msDB = require('_helpers/ms_db');
const gatewayDB = require('_helpers/gateway_db');
const User = gatewayDB.User;
const Charge = gatewayDB.Charge;
const Coupon = gatewayDB.Coupon;
const Withdraw = gatewayDB.Withdraw;

const cryptoRandomString = require('crypto-random-string');

module.exports = {
    register,
};


async function register(exchangeData) {
    // validate
    if (exchangeData.userId == "" || exchangeData.exchangeChips <= 0 || exchangeData.exchangeMethod == "" || exchangeData.depositHolder == "") {
        console.log('exchangeData incorrect')
        return { success: false, res: 'exchangeData incorrect' };
    }

    const user = await User.findByPk(exchangeData.userId);

    if (!user) throw 'Can not find user';

    if (user.depositHolder !== exchangeData.depositHolder) {
        return { success: false, res: 'depositHoler incorrect' };
    }


    if (exchangeData.exchangeMethod == "환전") {
        if (exchangeData.bankName !== user.bankName) {
            return { success: false, res: 'Bank name incorrect' };
        }

        if (exchangeData.bankAccountNumber !== user.bankAccountNumber) {
            return { success: false, res: 'Bank account number incorrect' };
        }

        if (exchangeData.connectPlace == "") {
            return { success: false, res: 'connectplace incorrect' };
        }

        if (exchangeData.currencyExPassword !== user.currencyExPassword) {
            return { success: false, res: 'currencyExPassword incorrect' };
        }

        if (exchangeData.exchangeChips > user.chips) {
            return { success: false, res: 'Chips lack' };
        }
    }

    try {
        if (exchangeData.exchangeMethod == "환전") {
            user.chips -= exchangeData.exchangeChips;
            await User.updateChips(user.id, user.chips);

            await Withdraw.recordWithdraw(user.id, user.bankName, exchangeData.depositHolder, user.nickName, exchangeData.exchangeChips);
        } else {
            var couponname = "사용자충전 " + cryptoRandomString(10);
            await Coupon.recordCoupon(couponname, exchangeData.exchangeChips, user.parentid);
            await Charge.recordCharge(user.id, couponname, exchangeData.exchangeChips, user.nickName);
        }
        return { success: true, res: 'Rec exchange history success' };
    } catch (err) {
        console.log(err);
        return { success: false, res: 'Database save error' };
    }
}