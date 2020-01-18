const msDB = require('_helpers/ms_db');
const Alarm = msDB.Alarm;

module.exports = {
    register
};

async function register(alarmData) {
    if (alarmData.kind =="") {
        return {success:false, res:'Alarm data incorrect'};
    }

    try {
        var alarm = Alarm.build(alarmData);
        await alarm.save();
        return {success:true, res:'Rec alarm success'};
    } catch (err) {
        console.log(err);
        return {success:false, res:'Database save error'};
    }
}
