const expressJwt = require('express-jwt');
const config = require('config.json');
const msUserService = require('../app_modules/users/ms_user.service');

module.exports = jwt;

function jwt() {
    const secret = config.secret;
    return expressJwt({ secret, isRevoked }).unless({
        path: [
            // public routes that don't require authentication
            '/users/authenticate',
            '/users/register',
            '/users/check',
            '/',
            /\/assets*/,
            /\/js*/,
            /\/audio*/,
            /\/avatar*/,
            /\/images*/,
            /\/styles*/,
            /\/partials*/,
            /\/admin*/,
        ]
    });
}

async function isRevoked(req, payload, done) {
    const user = await msUserService.getById(payload.sub);
    
    // revoke token if user no longer exists
    if (!user) {
        return done(null, true);
    }

    done();
};