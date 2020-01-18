const express = require('express');
const router = express.Router();
const msNoticeService = require('./ms_notice.service');

// routes
router.get('/', getAll);
router.get('/:id', getById);

module.exports = router;
function getById(req, res, next) {
    msNoticeService.getById(req.params.id)
        .then(notice => notice ? res.json(notice) : res.sendStatus(404))
        .catch(err => next(err));
}

function getAll(req, res, next) {
    msNoticeService.getAll()
        .then(notice => res.json(notice))
        .catch(err => next(err));
}
