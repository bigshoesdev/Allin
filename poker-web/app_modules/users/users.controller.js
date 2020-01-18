const express = require('express');
const router = express.Router();
const msUserService = require('./ms_user.service');
// routes
router.post('/register', register);
router.get('/:id', getById);
router.get('/current', getCurrent);
router.post('/check', checkUserID);

module.exports = router;

function register(req, res, next) {
    var ip = req.headers['x-forwarded-for'] || req.connection.remoteAddress;
    ip = ip.substr(ip.search(/\d/));
    msUserService.create(ip, req.body)
        .then(() => {
            console.log('bbbbbb');
            res.json({})
        })
        .catch(err => {
            console.log(err);
            console.log('ccccccc'); next(err);
        });
}

function getById(req, res, next) {
    msUserService.getById(req.params.id)
        .then(user => user ? res.json(user) : res.sendStatus(404))
        .catch(err => next(err));
}

function getCurrent(req, res, next) {
    msUserService.getById(req.params.sub)
        .then(user => user ? res.json(user) : res.sendStatus(404))
        .catch(err => next(err));
}

function checkUserID(req, res, next) {
    if (req.body.type == "userID") {
        msUserService.checkUserID(req.body)
            .then(() => res.json({ message: 'success' }))
            .catch(err => { next(err) });
    } else {
        msUserService.checkUserNickName(req.body)
            .then(() => res.json({ message: 'success' }))
            .catch(err => { next(err) });
    }
}
