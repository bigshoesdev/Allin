const mongoose = require('mongoose');
const Schema = mongoose.Schema;

const schema = new Schema({
    uid: { type: String, required: true },
    title: { type: String, required: true },
    ncontent: { type: String, required: true },
    answer: { type: String, default: "" },
    replied: { type: Boolean, default: false },
    createdDate: { type: Sequelize.DATE, defaultValue: Sequelize.literal('CURRENT_TIMESTAMP') }
});

schema.set('toJSON', { virtuals: true });

module.exports = mongoose.model('Question', schema);
