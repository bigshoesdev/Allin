require('rootpath')();
const express = require('express');
const app = express();
const server = require('http').createServer(app);
const io = require('socket.io').listen(server, { 'pingInterval': 1000, 'pingTimeout': 10000 });
const cors = require('cors');
const bodyParser = require('body-parser');
const jwt = require('_helpers/jwt');
const errorHandler = require('_helpers/error-handler');
const lessMiddleware = require('less-middleware');
const path = require('path');
const msUserService = require('app_modules/users/ms_user.service');
const msDB = require('_helpers/ms_db');
const Setting = msDB.Setting;
const logger = require('logger').createLogger('development.log');

require('./socket/ms_socket')(io);

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());
app.use(cors());

// use JWT auth to secure the api
app.use(jwt());


// api routes
app.use('/users', require('./app_modules/users/users.controller'));

// global error handler
app.use(errorHandler);

// start server
const port = process.env.NODE_ENV === 'production' ? 10606 : 10606;

server.listen(port, async function () {
	logger.format = function (level, date, message) {
		return date.toDateString() + ": " + message;
	};

	global.log = function(message) {
		logger.info(message);
		console.log(message);
	}

	msUserService.resetLoginFlag();

	let managerFee = await Setting.getManageFee();
	global.managerFee = managerFee;
});


