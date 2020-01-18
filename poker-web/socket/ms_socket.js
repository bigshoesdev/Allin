const path = require('path'),
    config = require('config.json'),
    socketioJwt = require('socketio-jwt'),
    userService = require('../app_modules/users/ms_user.service'),
    noticeService = require('../app_modules/notices/ms_notice.service'),
    convertHistoryService = require('../app_modules/convertHistories/ms_convertHistory.service'),
    questionService = require('../app_modules/questions/ms_question.service'),
    alarmService = require('../app_modules/alarms/ms_alarm.service'),
    giftHistoryService = require('../app_modules/giftHistories/ms_giftHistory.service'),
    exchangeHistoryService = require('../app_modules/exchangeHistories/ms_exchangeHistory.service'),
    blockWordService = require('../app_modules/blockwords/ms_blockword.service'),
    { User, Notice, BlockWord, Setting } = require('../_helpers/gateway_db'),
    { ConvertHistory, Question, GiftHistory, Award, LoginHistory } = require('../_helpers/ms_db'),

    Table = require('poker_modules/table'),
    Player = require('poker_modules/player');

const Sequelize = require('sequelize');
const Op = Sequelize.Op;

var ipCheck = false;

module.exports = function (io) {

    var lobbyTables = [];
    var players = [];
    var tables = {};

    /**
     * Event emitter function that will be sent to the table objects
     * Tables use the eventEmitter in order to send events to the client
     * and update the table data in the ui
     * @param string tableId
     */
    var eventEmitter = function (tableId) {
        return function (eventName, eventData) {
            io.sockets.in('table-' + tableId).emit(eventName, eventData);
        }
    }

    const stateDictionary = { 'InRobby': 0, 'WatchGame': 1, 'prepareGame': 2, 'SeatReserved': 3, 'DidSmallBlind': 4, 'DidBigBlind': 5, 'DidCall': 6, 'DidCheck': 7, 'DidRaise': 8, 'DidFold': 9, 'DidAllin': 10, 'DidOutReserve': 11, 'NewPlayer': 12 };

    function htmlEntities(str) {
        return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function convertToJSon(data) {
        try {
            if (typeof (data) === 'string') {
                return JSON.parse(data);
            } else {
                return data;
            }
        } catch (e) {
            return {};
        }
    }

    function convertDateToString(date) {
        var strDate = '';
        var yy = date.getFullYear();
        var mm = date.getMonth() + 1; // getMonth() is zero-based
        var dd = date.getDate();
        var hh = date.getHours();
        var mm_min = date.getMinutes();
        var ss = date.getSeconds();
        mm = (mm > 9 ? '' : '0') + mm;
        dd = (dd > 9 ? '' : '0') + dd;

        if (hh >= 13) {
            hh = hh - 12;
            hh = '오후 ' + hh;
        } else {
            hh = '오전 ' + hh;
        }

        mm_min = (mm_min > 9 ? '' : '0') + mm_min;
        ss = (ss > 9 ? '' : '0') + ss;

        strDate = yy + '-' + mm + '-' + dd + ' ' + hh + ':' + mm_min + ':' + ss;

        return strDate;
    }

    function convertDateToString_short(date) {
        var strDate = '';
        var yy = date.getFullYear();
        var mm = date.getMonth() + 1; // getMonth() is zero-based
        var dd = date.getDate();
        var hh = date.getHours();
        var mm_min = date.getMinutes();
        var ss = date.getSeconds();
        mm = (mm > 9 ? '' : '0') + mm;
        dd = (dd > 9 ? '' : '0') + dd;

        if (hh >= 13) {
            hh = '오후 ';
        } else {
            hh = '오전 ';
        }

        strDate = yy + '-' + mm + '-' + dd + ' ' + hh;

        return strDate;
    }

    function getLobbyTables() {
        // global.log('---called \"getLobbyTables\" in socket.js');
        lobbyTables = [];

        for (var tableId in tables) {
            // Sending the public data of the public tables to the lobby screen
            if (!tables[tableId].privateTable) {
                var lobbyTable = {};

                lobbyTable.id = tables[tableId].public.id;
                lobbyTable.name = tables[tableId].public.name;
                lobbyTable.seatsCount = tables[tableId].public.seatsCount;
                lobbyTable.playersSittingInCount = tables[tableId].playersSittingInCount;
                lobbyTable.playersSeatedCount = tables[tableId].public.playersSeatedCount;
                lobbyTable.playersWaitingCount = tables[tableId].playersWaitingCount;
                lobbyTable.bigBlind = tables[tableId].public.bigBlind;
                lobbyTable.smallBlind = tables[tableId].public.smallBlind;
                lobbyTable.minBuyIn = tables[tableId].public.minBuyIn;
                lobbyTable.password = tables[tableId].public.password;

                lobbyTables.push(lobbyTable);
            }
        }

        lobbyTables.sort(function (obj1, obj2) {
            var countCompare = obj2.playersSittingInCount - obj1.playersSittingInCount;

            var obj1No = parseInt(obj1.id.substr(5));
            var obj2No = parseInt(obj2.id.substr(5));

            if (countCompare > 0) {
                return 1;
            } else if (countCompare == 0) {
                if (obj1No > obj2No)
                    return 1;
                else
                    return -1;
            }
            else {
                return -1;
            }
        });

        return lobbyTables;
    }

    var smallBlindArr = [100, 200, 500, 1000, 2000, 3000, 5000, 10000, 20000]
    var minBuyInArr = [4, 8, 20, 40, 80, 120, 200, 400, 800]
        , tempMaxBuyIn = 100000000000;

    var roomMaxNo = 0;
    for (var i in minBuyInArr) {
        for (var j = 0; j < 2; j++) {
            no = i * 2 + j + 1;
            roomMaxNo = no;
            tables["room_" + (no - 1)] = new Table("room_" + (no - 1), "홀덤" + no, eventEmitter("room_" + (no - 1)), 10, 2 * smallBlindArr[i], smallBlindArr[i], tempMaxBuyIn, minBuyInArr[i] * 1000, false, "");
        }
    };

    function findPlayer(uid) {
        global.log('---- called findPlayer');
        for (var key in players) {

            if (uid == players[key].public.id) {
                return players[key]
            }
        }
        return null;
    }

    function findPlayerById(uid) {
        global.log('---- called findPlayer');

        for (var key in players) {

            if (uid == players[key].public.id) {
                return players[key]
            }
        }
        return null;

    }

    setInterval(async function () {
        var ids = [];
        for (id in players) {
            if (!players[id].isConnected) {
                ids.push(id);
            }
        }

        for (var i = 0; i < ids.length; i++) {
            await disconnect(ids[i]);
        }

        for (id in players) {
            players[id].isConnected = false;
            players[id].socket.emit("checkIsConnected", "");
        }

    }, 40000);

    async function disconnect(id) {
        // If the player was sitting on a table
        if (typeof players[id] !== 'undefined') {
            // If the player was sitting on a table
            if (players[id].disconnect)
                return;

            players[id].disconnect = true;
            if (players[id].sittingOnTable !== false && typeof tables[players[id].sittingOnTable] !== 'undefined') {
                // The seat on which the player was sitting
                var seat = players[id].seat;
                // The table on which the player was sitting
                var tableId = players[id].sittingOnTable;

                if (tables[tableId].gameIsOn && players[id].public.inHand) {
                    if (tables[tableId].public.activeSeat == seat) {
                        if (tables[tableId].public.phase == "bigBlind") {
                            if (players[id].public.state !== stateDictionary.NewPlayer) {
                                players[id].public.state = stateDictionary.DidBigBlind;
                            }
                            tables[tableId].playerPostedBigBlind();
                            tables[tableId].playerReserveFromDisconnect(seat);
                        } else if (tables[tableId].public.phase == "smallBlind") {
                            tables[tableId].playerPostedSmallBlind();
                            tables[tableId].playerReserveFromDisconnect(seat);
                        } else {
                            setTimeout(function () {
                                if (tables[tableId].seats[seat]) {
                                    tables[tableId].seats[seat].public.state = stateDictionary.DidFold;
                                    tables[tableId].playerFolded();
                                }
                            }, 10000);
                            global.log("---------------------playerReserveGoOut from disconnect");
                            tables[tableId].playerReserveFromDisconnect(seat);
                        }
                    } else {
                        tables[tableId].playerReserveFromDisconnect(seat);
                    }
                } else {
                    if (tables[tableId].gameIsOn && players[id].public.state == stateDictionary.DidFold) {
                        tables[tableId].playerReserveFromDisconnect(seat);
                    } else {
                        await tables[tableId].playerLeft(seat);
                        players[id].socket.leave('table-' + players[id].room);
                        players[id].socket.broadcast.emit('lobbyDataUpdate_app', { 'success': true, 'lobbyTables': getLobbyTables() });
                        players[id].room = null;
                    }
                }
            } else if (players[id].room) {
                var room = players[id].room;

                tables[room].playersWaitingCount--;

                if (tables[room].playersWaitingCount < 0)
                    tables[room].playersWaitingCount = 0;

                userService.updateChipsByAdd(players[id].id, 1 * players[id].joinRoomChip);
                players[id].joinRoomChip = 0;
                players[id].socket.broadcast.emit('lobbyDataUpdate_app', { 'success': true, 'lobbyTables': getLobbyTables() });
                players[id].room = null;
            }

            global.log("userservice disconnect" + players[id].public.name);

            var ip = players[id].socket.handshake.address;
            ip = ip.substr(ip.search(/\d/));

            var autoLogin = false;

            if (players[id].socket.autoLogin)
                autoLogin = players[id].socket.autoLogin;

            userService.disconnect(players[id].id, autoLogin);

            players[id].socket.disconnect();

            delete players[id];
        }
    }

    io.sockets.on('connection', function (socket) {
        global.log("Client is connected! socket_id : " + socket.id);

        socket.emit('connection_app', { success: true });

        socket.on('login_img_url', async function (data) {
            if (!socket.isLoginImgUrl) {
                global.log('!!! login_img_url event emited from client!');
                socket.isLoginImgUrl = true;

                let message = await Setting.getLoginImageUrl();

                let isLoginShow = await Setting.getLoginShow();

                if (!isLoginShow) {
                    socket.emit('login_img_url', { success: true, url: "error" });
                    return;
                }

                if (!message)
                    message = "error";

                socket.emit('login_img_url', { success: true, url: message });
            }
        });

        socket.on('registerNewUser_app', function(data) {
            console.log('registerNewUser_app');
            socket.broadcast.emit('registerNewUser_app', {});
        });

        socket.on('login_app_from_title', function (data) {
            global.log('!!! login_app_from_title event emited from client!');
            var dataObj = convertToJSon(data);

            var ip = socket.handshake.address;
            ip = ip.substr(ip.search(/\d/));

            socket.autoLogin = true;
            userService.authenticate(ip, dataObj)
                .then(function (data) {
                    if (data.success) {
                        socket.user = data.user;
                        socket.emit('login_app_from_title', { success: true, user: data.user });
                    } else {
                        socket.emit('login_app_from_title', { success: false, message: data.message });
                    }
                }).catch(err => global.log(err));
        });

        socket.on('login_app_from_login', function (data) {
            global.log('!!! login_app_from_login event emited from client!');
            var dataObj = convertToJSon(data);

            var ip = socket.handshake.address;
            ip = ip.substr(ip.search(/\d/));

            socket.autoLogin = false;

            userService.authenticate(ip, dataObj)
                .then(async function (data) {
                    if (data.success) {
                        socket.user = data.user;
                        socket.emit('login_app_from_login', { success: true, user: data.user });
                    } else {
                        if (data.message == 'duplicate') {
                            var userID = dataObj.userID;
                            var user = await User.findByUserID(userID);

                            if (user) {
                                for (id in players) {
                                    if (players[id].public.id == user.id) {
                                        await User.updateNoLogin(user.id, 1);

                                        await disconnect(players[id]);

                                        userService.authenticate(ip, dataObj)
                                            .then(async function (data) {
                                                if (data.success) {
                                                    socket.user = data.user;
                                                    socket.emit('login_app_from_login', { success: true, user: data.user });
                                                }
                                            });
                                    }
                                }
                            }
                        }else{
                            socket.emit('login_app_from_login', { success: false, message: data.message });
                        }
                    }
                }).catch(err => global.log(err));
        });

        socket.on('checkIsConnected', function (data) {
            global.log('!!! checkIsConnected event emited from client!');
            var dataObj = convertToJSon(data);

            if (dataObj.success) {
                if (players[socket.id]) {
                    players[socket.id].isConnected = true;
                }
            }
        });

        socket.on('send_notice', async function (data) {
            let message = await Setting.getNoiceMessage();
            let title = await Setting.getNoticeTitle();
            let scroll = await Setting.getNoticeScroll();
            socket.broadcast.emit('send_notice_app', { 'message': message, 'title': title, 'scroll': scroll, 'type': data.type });
        });

        socket.on('adminProcess', async function (data) {
            global.log('!!! adminProcess event emitted from admin')

            if (data.processName == 'replyToQuestion') {
                var QuestionArr = [];
                var userId = data.param.uid;
                var player = findPlayer(data.param.nickName);

                if (player !== null) {
                    global.log('Player Name=' + player.public.name);

                    Question.findAll({ where: { 'uid': userId } }).map(model => model.toJSON()).then(questions => {
                        for (var nId in questions) {

                            QuestionArr[nId] = {};
                            QuestionArr[nId].id = questions[nId].id;
                            QuestionArr[nId].title = questions[nId].title;
                            QuestionArr[nId].ncontent = questions[nId].ncontent;
                            QuestionArr[nId].replied = questions[nId].replied;
                            QuestionArr[nId].answer = questions[nId].answer;
                            QuestionArr[nId].createdDate = convertDateToString_short(questions[nId].createdDate);
                        }
                        player.socket.emit('getQuestions_app', { 'success': true, 'questions': QuestionArr });
                        global.log('\"getQuestions_app\" event emitted to' + player.public.name);
                    }).catch(err => {
                        global.log(err);
                        socket.emit('getQuestions_app', { 'success': false });
                    });
                }
            }

            if (data.processName == 'approvalExchange') {
                global.log(' ***  approvalExchange')
                var player = findPlayerById(data.param)

                if (player !== null) {
                    global.log('player.public.name=' + player.public.name);
                    socket = player.socket;
                    var user = User.findByPk(player.id);
                    if (user) {
                        player.chips = user.chips

                        global.log('!!! userInfo_app event emited from server!  success : true');
                        socket.emit('userInfo_app', { 'success': true, 'user': { ...user } });
                    } else {
                        global.log('!!! userInfo_app event emited from server!  success : false');
                        socket.emit('userInfo_app', { 'success': false });
                    }
                } else {
                    global.log('Can not find player!');
                }
            }
        });

        socket.on('userInfo', async function (uid, callback) {
            global.log('!!! userInfo event emited from client!');

            var user = User.findByPk(uid);
            if (user) {
                callback({ 'success': true, 'user': { ...user } });
            } else {
                callback({ 'success': false });
            }
        })

        socket.on('userInfo_app', async function (data) {
            // global.log('!!! userInfo_app event emited from client!');
            var dataObj = convertToJSon(data);
            var uid = dataObj.uid;

            var user = await User.findByPk(uid);

            if (players[socket.id]) {
                players[socket.id].public.name = user.nickName;
                players[socket.id].chips = user.chips;
            }

            if (user) {
                // global.log('!!! userInfo_app event emited from server!  success : true');
                socket.emit('userInfo_app', { 'success': true, 'user': { ...user } });
            } else {
                // global.log('!!! userInfo_app event emited from server!  success : false');
                socket.emit('userInfo_app', { 'success': false });
            }
        })

        socket.on('register', async function (uid, callback) {
            global.log('!!! register event emited from client!');

            var user = await User.findByPk(uid);
            if (user) {
                players[socket.id] = new Player(socket, user.nickName, user.chips, user.id, user.avatarNo);
                callback({ 'success': true });
            } else {
                callback({ 'success': false });
            }
        })

        socket.on("add_room", function (room) {
            global.log('!!! add_room event emited from client!');
            if (room.id) {
                var table = tables[room.id];
                if (table.public.playersSeatedCount > 0) {
                    socket.emit("add_room", { 'success': false, "message": "운영중인 방입니다." });
                } else {
                    tables[room.id].public.password = room.password;
                    tables[room.id].public.bigBlind = 2 * smallBlindArr[room.buyin];
                    tables[room.id].public.smallBlind = smallBlindArr[room.buyin];
                    tables[room.id].public.minBuyIn = minBuyInArr[room.buyin] * 1000;
                    tables[room.id].public.password = room.password;
                    socket.emit("add_room", { 'success': true });
                    lobbyTables = getLobbyTables();
                    socket.broadcast.emit('lobbyDataUpdate_app', { 'success': true, 'lobbyTables': lobbyTables });
                }
            } else {
                var no = -1;

                for (var i = 0; i < roomMaxNo; i++) {
                    var table = tables["room_" + i];

                    if (table == null || table == undefined) {
                        no = i;
                        break;
                    }
                }

                if (no < 0) {
                    roomMaxNo++;
                    no = roomMaxNo - 1;
                }

                global.log('!!! room_add event emited from client!');
                tables["room_" + no] = new Table("room_" + no, "홀덤" + (no + 1), eventEmitter("room_" + no), 10, 2 * smallBlindArr[room.buyin], smallBlindArr[room.buyin], tempMaxBuyIn, minBuyInArr[room.buyin] * 1000, false, room.password);
                socket.emit("add_room", { 'success': true });
                lobbyTables = getLobbyTables();
                socket.broadcast.emit('lobbyDataUpdate_app', { 'success': true, 'lobbyTables': lobbyTables });
            }
        });

        socket.on('get_room', function (id) {
            global.log('!!! get_room event emited from client!');
            socket.emit("get_room", {
                id: tables[id].public.id,
                name: tables[id].public.name,
                password: tables[id].public.password,
            });
        })

        socket.on('delete_room', function (id) {
            global.log('!!! delete_room event emited from client!');
            var table = tables[id];
            if (table) {
                if (table.public.playersSeatedCount > 0 || table.playersWaitingCount > 0) {
                    socket.emit('delete_room', { success: false, message: "지금 진행중인 방입니다." });
                } else {
                    delete tables[id];
                    socket.emit('delete_room', { success: true });

                    lobbyTables = getLobbyTables();
                    socket.broadcast.emit('lobbyDataUpdate_app', { 'success': true, 'lobbyTables': lobbyTables });
                }
            }
        })

        socket.on('register_app', async function (data) {
            global.log('!!! register_app event emited from client!');

            var dataObj = convertToJSon(data);
            var uid = dataObj.uid;

            var user = await User.findByPk(uid);

            if (user) {
                var ip = socket.handshake.address;
                ip = ip.substr(ip.search(/\d/));

                if (!socket.autoLogin) {
                    await User.recordLoginHistory(user.id, ip);
                    await LoginHistory.recordLoginHistory(user.id, ip);
                    var recommendFee = await User.updateRecommendFee(user.id);
                }

                user.loginFlag = true;
                user.nologin = 0;

                await User.updateNoLogin(user.id, user.nologin);
                await User.updateLoginFlag(user.id, user.loginFlag);
                await User.updateGameType(user.id, "올인");

                players[socket.id] = new Player(socket, user.nickName, user.chips, user.id, user.avatarNo);
                socket.emit('register_app', { 'success': true });
            } else {
                socket.emit({ 'success': false });
            }
        });

        socket.on('noticemessage_app', async function () {
            global.log('!!! noticemessage_app event emited from client!');
            let message = await Setting.getNoticeScroll();
            socket.emit('getNoticeMessage_app', { 'message': message });
        });

        /**
         *  When a player get lobbydata
         */
        socket.on('lobbyData', function (callback) {
            // global.log('!!! lobbyData event emited from client!');
            lobbyTables = getLobbyTables();

            callback({ 'success': true, 'lobbyTables': lobbyTables })
        })


        socket.on('lobbyData_app', function () {
            // global.log('!!! lobbyData_app event emited from client!');
            var lobbyTables = getLobbyTables();
            socket.emit('lobbyData_app', { 'success': true, 'lobbyTables': lobbyTables });
        });

        /**
         * Get Tables[tableId] data
         */
        socket.on('getTableData', function (tableId, callback) {
            global.log('!!! getTableData event emited from client!');
            if (typeof tables[tableId] !== 'undefined') {
                callback({ 'success': true, 'table': tables[tableId].public });
            }
        })

        socket.on('table_data', function (data) {
            global.log('!!! table_data event emited from client!');
            var dataObj = convertToJSon(data);
            var tableId = dataObj.tableId;

            if (typeof tables[tableId] !== 'undefined') {
                socket.emit('table-data', tables[tableId].public);
            }
        })

        socket.on('getTableFullData', function (tableId, callback) {
            global.log('!!! getTableFullData event emited from client!');
            if (typeof tables[tableId] !== 'undefined') {
                var result = [];
                var data = tables[tableId].seats;
                data.forEach(function (item, index, array) {
                    if (item != null) {
                        result[index] = {};
                        result[index].cards = item.cards;
                    }
                });

                callback({
                    'success': true,
                    'table': tables[tableId].public,
                    'seats': result,
                    'board': tables[tableId].commonCards
                });
            }
        });

        socket.on('getTablePlayersData_app', function (data) {
            global.log('!!! getTablePlayersData_app event emited from client!');
            var dataObj = convertToJSon(data);
            var tableId = dataObj.tableId;
            var tablePlayerData = [];
            var i = 0;
            for (var playerNo in tables[tableId].seats) {
                if (tables[tableId].seats[playerNo]) {
                    tablePlayerData[i] = {};
                    tablePlayerData[i].name = tables[tableId].seats[playerNo].public.name;
                    tablePlayerData[i].chips = tables[tableId].seats[playerNo].public.chipsInPlay;
                    i++;
                }
            }
            socket.emit('getTablePlayersData_app', { 'success': true, 'tableId': tableId, 'tablePlayerData': tablePlayerData });
        });

        socket.on('enterRoom_app', async function (data) {
            global.log('!!! enteroom_app event emited from client!');
            var dataObj = convertToJSon(data);
            var tableId = dataObj.tableId;

            var chips = 0;

            var ip = socket.handshake.address;
            ip = ip.substr(ip.search(/\d/));

            if (dataObj.chips)
                chips = parseInt(dataObj.chips);

            if (tables[tableId] == null || tables[tableId] == undefined) {
                socket.emit('enterRoom_app', { 'success': false, 'message': 'cancel' });
                return;
            }

            for (id in players) {
                if (players[id].public.state == stateDictionary.WatchGame && players[id].room && players[id].room == tableId) {
                    if (players[id].public.id == players[socket.id].public.id) {
                        socket.emit('enterRoom_app', { 'success': false, 'message': 'inplay' });
                        return;
                    }

                    var ipstr = players[id].socket.handshake.address;
                    ipstr = ipstr.substr(ipstr.search(/\d/));

                    if (ipCheck) {
                        if (ip == ipstr) {
                            socket.emit('enterRoom_app', { 'success': false, 'message': 'ip' });
                            return;
                        }
                    }
                }
            }

            if (tables[tableId].public.seats) {
                for (var i = 0; i < tables[tableId].public.seats.length; i++) {
                    if (tables[tableId].public.seats[i]) {
                        if (players[socket.id].public.id == tables[tableId].public.seats[i].id) {
                            socket.emit('enterRoom_app', { 'success': false, 'message': 'inplay' });
                            return;
                        }

                        var ipstr = tables[tableId].seats[i].socket.handshake.address;
                        ipstr = ipstr.substr(ipstr.search(/\d/));

                        if (ipCheck) {
                            if (ip == ipstr) {
                                socket.emit('enterRoom_app', { 'success': false, 'message': 'ip' });
                                return;
                            }
                        }
                    }
                }
            }

            for (var i = 0; i < tables[tableId].public.seatsToLeftFromFold.length; i++) {
                if (tables[tableId].public.seatsToLeftFromFold[i] != null) {
                    if (players[socket.id].public.id == tables[tableId].public.seatsToLeftFromFold[i].id) {
                        socket.emit('enterRoom_app', { 'success': false, 'message': 'inplay' });
                        return;
                    }
                }
            }

            if (typeof players[socket.id] !== 'undefined' && players[socket.id].room === null && tables[tableId]) {
                var user = await User.findByPk(players[socket.id].id);

                if (user.chips >= chips) {
                    // Add the room to the player's data
                    players[socket.id].room = tableId;
                    //chage player state
                    players[socket.id].public.state = stateDictionary.WatchGame;

                    await userService.updateChipsByAdd(user.id, -1 * chips);

                    players[socket.id].joinRoomChip = chips;

                    tables[tableId].playersWaitingCount++;

                    var lobbyTables = getLobbyTables();

                    socket.broadcast.emit('lobbyData_app', { 'success': true, 'lobbyTables': lobbyTables });

                    socket.emit('table-data', tables[tableId].public);

                    socket.join('table-' + tableId);
                } else {
                    socket.emit('enterRoom_app', { 'success': false, 'message': 'money' });
                }
            } else {
                socket.emit('enterRoom_app', { 'success': false, 'message': 'inplay' });
            }
        });

        /**
         * When a player leaves a room
         */
        socket.on('leaveRoom', async function () {
            global.log('!!! leaveRoom event emited from client!');
            if (typeof players[socket.id] !== 'undefined' && players[socket.id].room !== null && players[socket.id].sittingOnTable === false && !players[socket.id].leaveRoomState) {
                // Update User Chips
                players[socket.id].leaveRoomState = true;

                global.log('!!! leaveRoom event emited from client! ' + players[socket.id].public.name);

                await userService.updateChipsByAdd(players[socket.id].id, 1 * players[socket.id].public.chipsInPlay + 1 * players[socket.id].joinRoomChip);

                players[socket.id].joinRoomChip = 0;

                var user = await User.findByPk(players[socket.id].id)

                if (user) {
                    players[socket.id].chips = user.chips;
                }

                // Remove the player from the socket room
                socket.leave('table-' + players[socket.id].room);

                var room = players[socket.id].room;

                if (room) {
                    tables[room].playersWaitingCount--;
                    if (tables[room].playersWaitingCount < 0)
                        tables[room].playersWaitingCount = 0;
                }

                players[socket.id].public.state = stateDictionary.InRobby;

                players[socket.id].room = null;

                // Broadcast lobby table datas
                var lobbyTables = getLobbyTables();

                socket.emit('lobbyDataUpdate_app', { 'success': true, 'lobbyTables': lobbyTables });
                socket.broadcast.emit('lobbyDataUpdate_app', { 'success': true, 'lobbyTables': lobbyTables });

                players[socket.id].leaveRoomState = false;
            }
        });

        socket.on('checkEnterRoomPossible_app', function (data) {
            global.log('!!! checkEnterRoomPossible_app event emited from client!');
            var dataObj = convertToJSon(data);
            var tableId = dataObj.tableId;

            var ip = socket.handshake.address;
            ip = ip.substr(ip.search(/\d/));

            if (!players[socket.id]) {
                socket.emit('checkEnterRoomPossible_app', { 'success': false, 'possible': false });
                return;
            }

            if (tables[tableId] == null || tables[tableId] == undefined) {
                socket.emit('checkEnterRoomPossible_app', { 'success': false, 'possible': false });
                return;
            }

            for (id in players) {
                if (players[id].public.state == stateDictionary.WatchGame && players[id].room && players[id].room == tableId) {
                    if (players[id].public.id == players[socket.id].public.id) {
                        socket.emit('checkEnterRoomPossible_app', { 'success': false, 'possible': false });
                        return;
                    }

                    var ipstr = players[id].socket.handshake.address;
                    ipstr = ipstr.substr(ipstr.search(/\d/));

                    if (ipCheck) {
                        if (ip == ipstr) {
                            socket.emit('checkEnterRoomPossible_app', { 'success': false, 'possible': true });
                            return;
                        }
                    }
                }
            }

            if (tables[tableId].public.seats) {
                for (var i = 0; i < tables[tableId].public.seats.length; i++) {
                    if (tables[tableId].public.seats[i]) {
                        if (players[socket.id].public.id == tables[tableId].public.seats[i].id) {
                            socket.emit('checkEnterRoomPossible_app', { 'success': false, 'possible': false });
                            return;
                        }

                        var ipstr = tables[tableId].seats[i].socket.handshake.address;
                        ipstr = ipstr.substr(ipstr.search(/\d/));

                        if (ipCheck) {
                            if (ip == ipstr) {
                                socket.emit('checkEnterRoomPossible_app', { 'success': false, 'possible': true });
                                return;
                            }
                        }
                    }
                }
            }

            for (var i = 0; i < tables[tableId].public.seatsToLeftFromFold.length; i++) {
                if (tables[tableId].public.seatsToLeftFromFold[i] != null) {
                    if (players[socket.id].public.id == tables[tableId].public.seatsToLeftFromFold[i].id) {
                        socket.emit('checkEnterRoomPossible_app', { 'success': false, 'possible': false });
                        return;
                    }
                }
            }

            socket.emit('checkEnterRoomPossible_app', { 'success': true, 'possible': true });
        });

        socket.on('leaveTable_app', async function () {
            global.log('!!! leaveTable_app event emited from client!');
            // If the player was sitting on a table
            if (players[socket.id].sittingOnTable !== false && tables[players[socket.id].sittingOnTable] !== false) {
                // The seat on which the player was sitting
                var seat = players[socket.id].seat;
                // The table on which the player was sitting
                var tableId = players[socket.id].sittingOnTable;

                if (tables[tableId].gameIsOn
                    && players[socket.id].public.state !== stateDictionary.DidFold
                    && players[socket.id].public.state !== stateDictionary.WatchGame
                    && players[socket.id].public.state !== stateDictionary.SeatReserved) {
                    global.log("---------------------playerReserveGoOut from leaveTable_app");
                    tables[tableId].playerReserveGoOut(players[socket.id].seat);
                    socket.emit('leaveTable_app', { 'success': true });
                } else {
                    if (tables[tableId].gameIsOn && players[socket.id].public.state == stateDictionary.DidFold) {
                        await tables[tableId].playerLeftFromFold(seat);

                        var oldSocket = socket;
                        // Send the number of total chips back to the user
                        socket.emit('leaveTable_app', { 'success': true });
                        oldSocket.emit('leaveRoomByFoldOut');
                    } else {
                        // Remove the player from the seat
                        await tables[tableId].playerLeft(seat);

                        var oldSocket = socket;
                        // Send the number of total chips back to the user
                        socket.emit('leaveTable_app', { 'success': true });
                        oldSocket.emit('leaveRoom');
                    }
                }
            } else {
                // if player was only watching game
                socket.emit('leaveRoom');
            }
        });

        socket.on('leaveTableCancel_app', function () {
            global.log('!!! leaveTableCancel_app event emited from client!');
            // If the player was sitting on a table
            if (players[socket.id].sittingOnTable !== false && tables[players[socket.id].sittingOnTable] !== false) {
                // The seat on which the player was sitting
                var seat = players[socket.id].seat;
                // The table on which the player was sitting
                var tableId = players[socket.id].sittingOnTable;
                tables[tableId].playerReserveCanceled(seat);
            }
        });

        /**
         * When a player ask additionalChips 
         * @param function callback
         */
        socket.on('additionalChips_app', async function (data) {
            global.log('!!! additionalChips_app event emited from client!');
            // If the player was sitting on a table
            var dataObj = convertToJSon(data);
            var amount = parseInt(dataObj.amount);

            if (players[socket.id].sittingOnTable !== false && tables[players[socket.id].sittingOnTable] !== false && amount > 0) {
                var tableId = players[socket.id].sittingOnTable;

                if (tables[tableId].gameIsOn && tables[tableId].public.phase != "endround") {
                    if (tables[tableId].playerReserveAddChips(players[socket.id], amount))
                        socket.emit('additionalChips_app', { 'success': true });
                    else
                        socket.emit('additionalChips_app', { 'success': false, 'message': 'cancel' });
                } else {
                    socket.emit('additionalChips_app', { 'success': false, 'message': '' });
                }
            } else {
                socket.emit('additionalChips_app', { 'success': false, 'message': '' });
            }
        });

        socket.on('checkAdditionalChips_app', function () {
            global.log('!!! checkAdditionalChips_app event emited from client!');
            // If the player was sitting on a table
            if (players[socket.id].sittingOnTable !== false && tables[players[socket.id].sittingOnTable] !== false) {
                var tableId = players[socket.id].sittingOnTable;

                if (tables[tableId].gameIsOn && tables[tableId].public.phase != "endround") {
                    if (tables[tableId].playerCheckAddChips(players[socket.id]))
                        socket.emit('checkAdditionalChips_app', { 'success': true });
                    else
                        socket.emit('checkAdditionalChips_app', { 'success': false, 'message': 'cancel' });
                } else {
                    socket.emit('checkAdditionalChips_app', { 'success': false, 'message': '' });
                }
            } else {
                socket.emit('checkAdditionalChips_app', { 'success': false, 'message': '' });
            }
        });

        socket.on('sitOnTheTable_app', async function (data) {
            global.log('!!! sitOnTheTable_app event emited from client!');
            var dataObj = convertToJSon(data);
            var tableId = dataObj.tableId, seat = parseInt(dataObj.seat), chips = parseInt(dataObj.chips);
            if (
                // A seat has been specified
                typeof seat !== 'undefined'
                // A table id is specified
                && typeof tableId !== 'undefined'
                // The table exists
                && typeof tables[tableId] !== 'undefined'
                // The seat number is an integer and less than the total number of seats
                && typeof seat === 'number'
                && seat >= 0
                && seat < tables[tableId].public.seatsCount
                && typeof players[socket.id] !== 'undefined'
                // The seat is empty
                && tables[tableId].seats[seat] == null
                // The player had joined the room of the table
                && players[socket.id].room === tableId
                // The chips number chosen is a number
                && typeof chips !== 'undefined'
                && !isNaN(parseInt(chips))
                && isFinite(chips)
                // The chips number is an integer
                && chips % 1 === 0
                && chips >= 0
            ) {
                if (players[socket.id].sittingOnTable === false && players[socket.id].public.state != stateDictionary.SeatReserved) {
                    if (tables[tableId].gameIsOn && tables[tableId].public.seatsSittable[seat] > 0) {
                        if (players[socket.id].joinRoomChip > 0) {
                            tables[tableId].playerSatOnTheTable(players[socket.id], seat, chips);
                            socket.broadcast.emit('lobbyDataUpdate_app', { 'success': true, 'lobbyTables': getLobbyTables() });
                        }
                    } else if (!tables[tableId].gameIsOn) {
                        if (players[socket.id].joinRoomChip > 0) {
                            tables[tableId].playerSatOnTheTable(players[socket.id], seat, chips);
                            socket.broadcast.emit('lobbyDataUpdate_app', { 'success': true, 'lobbyTables': getLobbyTables() });
                        }
                    }
                } else if (players[socket.id].sittingOnTable && players[socket.id].public.state == stateDictionary.SeatReserved) {
                    tables[tableId].playerSatAnotherOnTheTable(players[socket.id], seat, chips);
                } else {
                    if (tables[tableId].gameIsOn && tables[tableId].public.seatsSittable[seat] > 0) {
                        tables[tableId].playerReserveAnotherSeat(players[socket.id], seat, chips);
                    }
                }
            } else {
                // If the user is not allowed to sit in, notify the user
                socket.emit('sitOnTheTable_app', { 'success': false });
            }
        });

        socket.on('postBlind_app', function (data) {
            global.log("=====================POSTBLIND_APP==================");

            var dataObj = convertToJSon(data);
            var postedBlind = dataObj.postedBlind;

            if (players[socket.id].sittingOnTable !== false) {
                var tableId = players[socket.id].sittingOnTable;
                var activeSeat = tables[tableId].public.activeSeat;
                if (tables[tableId]
                    && typeof tables[tableId].seats[activeSeat] !== 'undefined'
                    && typeof tables[tableId].seats[activeSeat].public !== 'undefined'
                    && tables[tableId].seats[activeSeat].socket.id === socket.id
                    && (tables[tableId].public.phase === 'smallBlind' || tables[tableId].public.phase === 'bigBlind')
                ) {
                    if (postedBlind = 'true') {
                        socket.emit('postBlind_app', { 'success': true });
                        if (tables[tableId].public.phase === 'smallBlind') {
                            // change player state if the player is not new
                            if (players[socket.id].public.state !== stateDictionary.NewPlayer) {
                                players[socket.id].public.state = stateDictionary.DidSmallBlind;
                            }
                            tables[tableId].playerPostedSmallBlind();
                        } else {
                            // change player state if the player is not new
                            if (players[socket.id].public.state !== stateDictionary.NewPlayer) {
                                players[socket.id].public.state = stateDictionary.DidBigBlind;
                            }
                            // The player posted the big blind
                            tables[tableId].playerPostedBigBlind();
                        }
                    }
                }
            }
        });

        socket.on('check_app', function () {
            global.log("=====================CHECK_APP==================");

            if (typeof players[socket.id].sittingOnTable !== 'undefined') {
                var tableId = players[socket.id].sittingOnTable;
                var activeSeat = tables[tableId].public.activeSeat;
                if (tables[tableId]
                    && tables[tableId].seats[activeSeat].socket.id === socket.id
                    && (!tables[tableId].public.biggestBet || (tables[tableId].public.biggestBet === players[socket.id].public.bet))
                    && ['preflop', 'flop', 'turn', 'river'].indexOf(tables[tableId].public.phase) > -1
                ) {
                    // Sending the callback first, because the next functions may need to send data to the same player, that shouldn't be overwritten
                    socket.emit('check_app', { 'success': true });

                    players[socket.id].public.state = stateDictionary.DidCheck;
                    tables[tableId].playerChecked();
                }
            }
        });

        socket.on('fold_app', async function () {
            global.log("=====================FOLD_APP==================");

            if (players[socket.id].sittingOnTable !== false) {
                var tableId = players[socket.id].sittingOnTable;
                var activeSeat = tables[tableId].public.activeSeat;

                if (tables[tableId] && tables[tableId].seats[activeSeat].socket.id === socket.id && ['preflop', 'flop', 'turn', 'river'].indexOf(tables[tableId].public.phase) > -1) {
                    // Sending the callback first, because the next functions may need to send data to the same player, that shouldn't be overwritten
                    socket.emit('fold_app', { 'success': true });

                    // change player state
                    players[socket.id].public.state = stateDictionary.DidFold;

                    tables[tableId].playerFolded();

                    if (tables[tableId].public.seatsToMove[activeSeat] == -2) {
                        await tables[tableId].playerLeftFromFold(activeSeat);

                        tables[tableId].public.seatsToMove[activeSeat] = -1;

                        setTimeout(function () {
                            socket.emit('leaveRoomByFoldOut');
                        }, 1000);
                    }
                }
            }
        });

        socket.on('call_app', function () {
            global.log("=====================CALL_APP==================");
            if (typeof players[socket.id].sittingOnTable !== 'undefined') {
                var tableId = players[socket.id].sittingOnTable;
                var activeSeat = tables[tableId].public.activeSeat;

                if (tables[tableId] && tables[tableId].seats[activeSeat].socket.id === socket.id && tables[tableId].public.biggestBet && ['preflop', 'flop', 'turn', 'river'].indexOf(tables[tableId].public.phase) > -1) {
                    // Sending the callback first, because the next functions may need to send data to the same player, that shouldn't be overwritten
                    // change player state
                    players[socket.id].public.state = stateDictionary.DidCall;

                    socket.emit('call_app', { 'success': true });

                    tables[tableId].playerCalled();
                }
            }
        });

        socket.on('bet', function (amount, callback) {
            global.log("bet-----------------");
            global.log(amount);
            if (typeof players[socket.id].sittingOnTable !== 'undefined') {
                var tableId = players[socket.id].sittingOnTable;
                var activeSeat = tables[tableId].public.activeSeat;

                if (tables[tableId] && tables[tableId].seats[activeSeat].socket.id === socket.id && !tables[tableId].public.biggestBet && ['preflop', 'flop', 'turn', 'river'].indexOf(tables[tableId].public.phase) > -1) {
                    // Validating the bet amount
                    amount = parseInt(amount);
                    if (amount && isFinite(amount) && amount <= tables[tableId].seats[activeSeat].public.chipsInPlay) {
                        if (amount < tables[tableId].seats[activeSeat].public.chipsInPlay) {
                            // change player state
                            players[socket.id].public.state = stateDictionary.DidRaise;
                        } else {
                            // change player state
                            players[socket.id].public.state = stateDictionary.DidAllin;
                        }

                        // Sending the callback first, because the next functions may need to send data to the same player, that shouldn't be overwritten
                        callback({ 'success': true });
                        tables[tableId].playerBetted(amount);
                    }
                }
            }
        });

        socket.on('bet_app', function (data) {
            global.log("=====================BET_APP==================");
            var dataObj = convertToJSon(data);
            var amount = dataObj.amount;

            if (typeof players[socket.id].sittingOnTable !== 'undefined') {
                var tableId = players[socket.id].sittingOnTable;
                var activeSeat = tables[tableId].public.activeSeat;
                if (tables[tableId] && tables[tableId].seats[activeSeat].socket.id === socket.id && !tables[tableId].public.biggestBet && ['preflop', 'flop', 'turn', 'river'].indexOf(tables[tableId].public.phase) > -1) {
                    // Validating the bet amount
                    amount = parseInt(amount);
                    if (amount && isFinite(amount) && amount <= tables[tableId].seats[activeSeat].public.chipsInPlay) {
                        if (amount < tables[tableId].seats[activeSeat].public.chipsInPlay) {
                            players[socket.id].public.state = stateDictionary.DidRaise;
                        } else {
                            players[socket.id].public.state = stateDictionary.DidAllin;
                        }

                        // Sending the callback first, because the next functions may need to send data to the same player, that shouldn't be overwritten
                        socket.emit('bet_app', { 'success': true });
                        tables[tableId].playerBetted(amount);
                    }
                }
            }
        });

        /**
         * When a player raises
         * @param function callback
         */
        socket.on('raise', function (amount, callback) {
            global.log("raise-----------------");

            //            global.log(amount);
            if (typeof players[socket.id].sittingOnTable !== 'undefined') {
                var tableId = players[socket.id].sittingOnTable;
                var activeSeat = tables[tableId].public.activeSeat;

                if (
                    // The table exists
                    typeof tables[tableId] !== 'undefined'
                    // The player who should act is the player who raised
                    && tables[tableId].seats[activeSeat].socket.id === socket.id
                    // The pot was betted 
                    && tables[tableId].public.biggestBet
                    // It's not a round of blinds
                    && ['preflop', 'flop', 'turn', 'river'].indexOf(tables[tableId].public.phase) > -1
                    // Not every other player is all in (in which case the only move is "call")
                    && !tables[tableId].otherPlayersAreAllIn()
                ) {
                    amount = parseInt(amount);
                    if (amount && isFinite(amount)) {
                        amount -= tables[tableId].seats[activeSeat].public.bet;
                        if (amount <= tables[tableId].seats[activeSeat].public.chipsInPlay) {

                            global.log('amount =' + amount);
                            global.log('tables[tableId].seats[activeSeat].public.chipsInPlay =' + tables[tableId].seats[activeSeat].public.chipsInPlay);
                            if (amount < tables[tableId].seats[activeSeat].public.chipsInPlay) {
                                // change player state
                                players[socket.id].public.state = stateDictionary.DidRaise;
                            } else {
                                // change player state
                                players[socket.id].public.state = stateDictionary.DidAllin;
                                global.log('@@@@@@@ All in in socket.io');
                            }

                            // Sending the callback first, because the next functions may need to send data to the same player, that shouldn't be overwritten
                            callback({ 'success': true });
                            // The amount should not include amounts previously betted
                            tables[tableId].playerRaised(amount);
                        }
                    }
                }
            }
        });

        socket.on('raise_app', function (data) {
            global.log("=====================RAISE_APP==================");
            var dataObj = convertToJSon(data);
            var amount = dataObj.amount;

            if (typeof players[socket.id].sittingOnTable !== 'undefined') {
                var tableId = players[socket.id].sittingOnTable;
                var activeSeat = tables[tableId].public.activeSeat;
                if (
                    // The table exists
                    typeof tables[tableId] !== 'undefined'
                    // The player who should act is the player who raised
                    && tables[tableId].seats[activeSeat].socket.id === socket.id
                    // The pot was betted 
                    && tables[tableId].public.biggestBet
                    // It's not a round of blinds
                    && ['preflop', 'flop', 'turn', 'river'].indexOf(tables[tableId].public.phase) > -1
                    // Not every other player is all in (in which case the only move is "call")
                    && !tables[tableId].otherPlayersAreAllIn()
                ) {
                    amount = parseInt(amount);
                    if (amount && isFinite(amount)) {
                        amount -= tables[tableId].seats[activeSeat].public.bet;
                        if (amount <= tables[tableId].seats[activeSeat].public.chipsInPlay) {
                            if (amount < tables[tableId].seats[activeSeat].public.chipsInPlay) {
                                // change player state
                                players[socket.id].public.state = stateDictionary.DidRaise;
                            } else {
                                // change player state
                                players[socket.id].public.state = stateDictionary.DidAllin;
                            }
                            // Sending the callback first, because the next functions may need to send data to the same player, that shouldn't be overwritten
                            socket.emit('raise_app', { 'success': true });
                            // The amount should not include amounts previously betted
                            tables[tableId].playerRaised(amount);
                        }
                    }
                }
            }
        });

        socket.on('getNotices', function (callback) {
            callback({ 'success': true, 'notices': noticeService.getAll() })
        })

        socket.on('game_exit_app', function () {
            global.log('!!! game_exit_app event emited from client!');
            socket.disconnect();
        });

        socket.on('getAwards_app', function () {
            global.log('!!! getAwards_app event emited from client!');

            Award.findAll().map(model => model.toJSON()).then(awards => {
                if (awards) {
                    var turnamentText = "터너먼트 쌤플텍스트입니다."
                    socket.emit('getAwards_app', { 'success': true, 'tournamentText': turnamentText, 'awards': awards })
                } else {
                    socket.emit('getAwards_app', { 'success': false })
                }
            })
        })

        function convertToJSon_forMultiline(data) {
            if (typeof (data) === 'string') {
                var temp = JSON.stringify(data)
                return JSON.parse(temp);
            } else {
                return data;
            }
        }

        String.prototype.replaceAll = function (search, replacement) {
            var target = this;
            global.log(search);
            return target.replace(new RegExp(search, 'g'), replacement);
        };

        socket.on('askQuestion_app', async function (data) {
            global.log('!!! askQuestion_app event emited from client!');

            var data0 = data.replaceAll("\r\n", "\\r\\n");
            var dataObj = convertToJSon(data0);

            var response = await questionService.register(dataObj);

            if (response.success) {
                var result = await alarmService.register({ kind: 'question' });
                socket.broadcast.emit('askQuestion', response);
                socket.emit('askQuestion_app', response);
            }
        })

        socket.on('getQuestions_app', function (data) {
            global.log('!!! getQuestions_app event emited from client!');

            var dataObj = convertToJSon(data);
            var userId = dataObj.uid;
            var QuestionArr = [];

            Question.findAll({ where: { 'uid': userId }, order: [['createdDate', 'ASC']] }).map(model => model.toJSON()).then(questions => {
                for (var nId in questions) {

                    QuestionArr[nId] = {};
                    QuestionArr[nId].id = questions[nId].id;
                    QuestionArr[nId].title = questions[nId].title;
                    QuestionArr[nId].ncontent = questions[nId].ncontent;
                    QuestionArr[nId].replied = questions[nId].replied;
                    QuestionArr[nId].answer = questions[nId].answer;
                    QuestionArr[nId].createdDate = convertDateToString_short(questions[nId].createdDate);
                }

                socket.emit('getQuestions_app', { 'success': true, 'questions': QuestionArr });
            }).catch(err => {
                global.log(err);
                socket.emit('getQuestions_app', { 'success': false });
            });
        })

        socket.on('recConvertHistory_app', function (data) {
            global.log('!!! recConvertHistory_app event emited from client!');
            var dataObj = convertToJSon(data);
            global.log(dataObj);

            var player = findPlayerById(dataObj.uid)

            convertHistoryService.register(dataObj)
                .then(function (response) {
                    global.log('recConvertHistory_app success!');
                    if (player) {
                        player.chips += parseInt(dataObj.chips);
                    }
                    socket.emit('recConvertHistory_app', response);

                })
                .catch(function (response) {
                    global.log('Rec Failed')
                });
        });

        socket.on('getConvertHistory_app', function (data) {
            global.log('!!! getconvertHistory_app event emited from client!');
            var dataObj = convertToJSon(data);
            var userId = dataObj.uid;

            var ConvertHistoryArr = [];
            ConvertHistory.findAll({ where: { 'userId': userId } }).map(model => model.toJSON()).then(convertHistories => {
                for (var nId in convertHistories) {
                    ConvertHistoryArr[nId] = {};
                    ConvertHistoryArr[nId].convertPoints = convertHistories[nId].convertPoints;
                    ConvertHistoryArr[nId].remaindPoints = convertHistories[nId].remaindPoints;
                    ConvertHistoryArr[nId].createdDate = convertDateToString(convertHistories[nId].createdDate);
                }

                socket.emit('getConvertHistory_app', { 'success': true, 'convertHistroies': ConvertHistoryArr });
            }).catch(e => {
                global.log(err);
                socket.emit('getconvertHistory_app', { 'success': false });
            });
        });


        /**
         *  When a player charge chips
         */

        socket.on('charge_app', function (data) {
            global.log('!!! charge_app event emited from client!');
            var dataObj = convertToJSon(data);

            var chargeData = {};
            chargeData.userId = dataObj.uid;
            chargeData.exchangeChips = parseInt(dataObj.chips);
            chargeData.exchangeMethod = "충전";
            chargeData.depositHolder = dataObj.depositHolder;

            exchangeHistoryService.register(chargeData)
                .then(function (response) {
                    if (response.success) {
                        global.log(' Rec charge data success ');
                        socket.emit('charge_app', { 'success': true, 'res': 'Record success' });
                    } else {
                        socket.emit('charge_app', { 'success': false, 'res': response.res ? response.res : "" });
                    }
                })
                .catch(function (response) {
                    global.log('Rec Failed')
                });

            var alarmData = { kind: "charge" };
        });

        socket.on('chipsToMoney_app', function (data) {
            global.log('!!! chipsToMoney_app event emited from client!');
            var dataObj = convertToJSon(data);

            var userId = dataObj.uid, chips = parseInt(dataObj.chips), bankName = dataObj.bankName, bankAccountNumber = dataObj.bankAccountNumber,
                depositHolder = dataObj.depositHolder, connectPlace = dataObj.connectPlace, currencyExPassword = dataObj.currencyExPassword;

            var chipsToMoneyData = {};
            chipsToMoneyData.userId = dataObj.uid;
            chipsToMoneyData.exchangeChips = parseInt(dataObj.chips);
            chipsToMoneyData.exchangeMethod = "환전";
            chipsToMoneyData.depositHolder = dataObj.depositHolder;
            chipsToMoneyData.bankName = dataObj.bankName;
            chipsToMoneyData.bankAccountNumber = dataObj.bankAccountNumber;
            chipsToMoneyData.connectPlace = dataObj.connectPlace;
            chipsToMoneyData.currencyExPassword = dataObj.currencyExPassword;

            exchangeHistoryService.register(chipsToMoneyData)
                .then(function (response) {
                    if (response.success) {
                        global.log('Rec chipsToMoney data success ');
                        socket.emit('chipsToMoney_app', { 'success': true, 'res': 'Record success' });
                    } else {
                        socket.emit('chipsToMoney_app', { 'success': false, 'res': response.res ? response.res : "" });
                    }
                    global.log(response);
                })
                .catch(function (response) {
                    global.log('Rec Failed')
                });

            var alarmData = { kind: "chipsToMoney" };
        });


        /**
         *  When give some chips to another person
         */
        socket.on('recGiftHistory_app', async function (data) {
            global.log('!!! recGiftHistory_app event emited from client!');
            var dataObj = convertToJSon(data);

            if (socket.user) {
                if (dataObj.nickNameToGift == socket.user.userID) {
                    data = { 'success': false, 'res': 'give to himeself' };
                    socket.emit('recGiftHistory_app', data);
                    return;
                }
                giftHistoryService.register(dataObj)
                    .then(async function (response) {
                        var user = await User.findByPk(players[socket.id].id)
                        if (user) {
                            players[socket.id].chips = user.chips;
                        } else {
                            global.log('!!! Can not find user');
                        }

                        socket.emit('recGiftHistory_app', response);
                    })
                    .catch(function (response) {
                        global.log('Rec Failed')
                    });
            } else {
                data = { 'success': false, 'res': 'not login' };
                socket.emit('recGiftHistory_app', data);
            }
        });

        socket.on('getNotices_app', function () {
            global.log('!!! getNotices_app event emited from client!');
            var NoticesArray = [];
            Notice.findAll({ order: [['regdate', 'ASC']] }).map(model => model.toJSON()).then(notices => {
                if (notices) {
                    for (var nId in notices) {
                        NoticesArray[nId] = {};
                        NoticesArray[nId].title = notices[nId].title;
                        NoticesArray[nId].ncontent = notices[nId].ncontent;
                        NoticesArray[nId].createdDate = convertDateToString(notices[nId].createdDate);
                    }
                    socket.emit('getNotices_app', { 'success': true, 'notices': NoticesArray });
                } else {
                    socket.emit('getNotices_app', { 'success': false });
                }
            });
        })

        socket.on('getGiftHistory_app', function (data) {
            global.log('!!! getGiftHistory_app event emited from client!');
            var dataObj = convertToJSon(data);
            var uid = dataObj.uid;
            var giftHistoryArr = [];
            GiftHistory.findAll({ where: { [Op.or]: [{ sentUID: { [Op.eq]: uid } }, { sentUID: { [Op.eq]: uid } }] } }).map(model => model.toJSON()).then(giftHistories => {
                for (var nId in giftHistories) {
                    giftHistoryArr[nId] = {};
                    giftHistoryArr[nId].sentUserID = giftHistories[nId].sentUserID;
                    giftHistoryArr[nId].receiveUserID = giftHistories[nId].receiveUserID;
                    giftHistoryArr[nId].sentChips = giftHistories[nId].sentChips;
                    giftHistoryArr[nId].createdDate = convertDateToString(giftHistories[nId].createdDate);
                }

                socket.emit('getGiftHistory_app', { 'success': true, 'giftHistroies': giftHistoryArr });
            }).catch(err => {
                global.log(err);
                socket.emit('getGiftHistory_app', { 'success': false });
            });
        });


        socket.on('changeOption_app', async function (data) {
            global.log('!!! changeAvatar_app event emited from client!');

            var dataObj = convertToJSon(data);

            var user = await User.findByPk(dataObj.uid);
            if (user) {
                user.avatarNo = dataObj.avatarNo;
                await User.updateBackDesk(user.id, dataObj.backDesk);
                await User.updateFrontDesk(user.id, dataObj.frontDesk);
                await User.updateFelt(user.id, dataObj.felt);
                await User.updateBackground(user.id, dataObj.background);
                await User.updateAvatarNo(user.id, dataObj.avatarNo);
                players[socket.id].public.avartaNo = parseInt(dataObj.avatarNo);

                socket.emit('changeAvatar_app', { 'success': true, 'user': user });
            } else {
                socket.emit('changeAvatar_app', { 'success': false });
            }

        });

        socket.on('changeWinLost_app', async function (data) {
            global.log('!!! changeWinLost_app event emited from client!');

            var dataObj = convertToJSon(data);

            var user = User.findByPk(dataObj.uid);
            if (user) {
                user.winNumber = dataObj.winNumber;
                user.lostNumber = dataObj.lostNumber;
                socket.emit('changeWinLost_app', { 'success': true, 'user': user });
            } else {
                socket.emit('changeWinLost_app', { 'success': false });
            }
        });

        /**
         * When a message from a player is sent
         * @param string message
         */
        socket.on('sendMessage', async function (message) {
            global.log('!!! sendMessage event emitted from client')
            message = message.trim();

            if (players[socket.id].seat == null) {
                return;
            }

            var blockWords = await blockWordService.getAll();

            for (var i in blockWords) {
                message = message.replaceAll(blockWords[i].word, "***");
            }

            if (message && typeof players[socket.id].room !== 'undefined') {
                socket.emit('receiveMessage', { 'message': message, 'sender': players[socket.id].seat });
                socket.broadcast.to('table-' + players[socket.id].room).emit('receiveMessage', { 'message': message, 'sender': players[socket.id].seat });
            }
        });

        /**
         * When a player disconnects
         */
        socket.on('disconnect', async function () {
            global.log("=====================NETWORK_DISCONNECT==================");
            // If the socket points to a player object
            await disconnect(socket.id);
        });
    });

}
