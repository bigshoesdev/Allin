const express = require('express');
const router = express.Router();
const msConvertHistoryService = require('./ms_convertHistory.service');

// routes
router.post('/register', register);
router.delete('/:id', _delete);

module.exports = router;

function register(req, res, next) {
    msConvertHistoryService.register(req.body)
        .then(() => res.json({}))
        .catch(err => next(err));
}

function getByUserId(req, res, next) {
    msConvertHistoryService.getByUserId(req.params.id)
        .then(convertHistory => convertHistory ? res.json(convertHistory) : res.sendStatus(404))
        .catch(err => next(err));
}