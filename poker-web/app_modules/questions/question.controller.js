const express = require('express');
const router = express.Router();
const msQuestionService = require('./ms_question.service');

router.post('/register', register);
router.get('/', getAll);
router.get('/:id', getById);
router.put('/:id', update);
router.delete('/:id', _delete);

module.exports = router;

function register(req, res, next) {
    msQuestionService.create(req.body)
        .then(() => res.json({}))
        .catch(err => next(err));
}

function getById(req, res, next) {
    msQuestionService.getById(req.params.id)
        .then(question => question ? res.json(question) : res.sendStatus(404))
        .catch(err => next(err));
}

function getAll(req, res, next) {
    msQuestionService.getAll()
        .then(question => res.json(question))
        .catch(err => next(err));
}

function update(req, res, next) {
    msQuestionService.update(req.params.id, req.body)
        .then(() => res.json({}))
        .catch(err => next(err));
}

function _delete(req, res, next) {
    msQuestionService.delete(req.params.id)
        .then(() => res.json({}))
        .catch(err => next(err));
}
