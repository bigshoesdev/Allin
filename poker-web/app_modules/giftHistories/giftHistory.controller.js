const express = require('express');
const router = express.Router();
const msGiftHistoryService = require('./ms_giftHistory.service');

router.post('/register', register);
router.get('/', getAll);
router.get('/:id', getByUserId);
router.delete('/:id', _delete);

module.exports = router;

function register(req, res, next) {
    msGiftHistoryService.register(req.body)
        .then((result) => res.json({'success': result}))
        .catch(err => next(err));
}

function getByUserId(req, res, next) {
    msGiftHistoryService.getByUserId(req.params.id)
        .then(giftHistory => giftHistory ? res.json(giftHistory) : res.sendStatus(404))
        .catch(err => next(err));
}

function getAll(req, res, next) {
    msGiftHistoryService.getAll()
        .then(giftHistory => res.json(giftHistory))
        .catch(err => next(err));
}

function _delete(req, res, next) {
    msGiftHistoryService.delete(req.params.id)
        .then(() => res.json({}))
        .catch(err => next(err));
}
