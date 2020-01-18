var Deck = require('./deck'),
	Pot = require('./pot');

const db = require('_helpers/ms_db');
const gatewayDB = require('_helpers/gateway_db');
const User = gatewayDB.User;
const userService = require('../app_modules/users/ms_user.service');
const GameHistory = db.GameHistory;


/**
 * The table "class"
 * @param string	id (the table id)
 * @param string	name (the name of the table)
 * @param object 	deck (the deck object that the table will use)
 * @param function 	eventEmitter (function that emits the events to the players of the room)
 * @param int 		seatsCount (the total number of players that can play on the table)
 * @param int 		bigBlind (the current big blind)
 * @param int 		smallBlind (the current smallBlind)
 * @param int 		maxBuyIn (the maximum amount of chips that one can bring to the table)
 * @param int 		minBuyIn (the minimum amount of chips that one can bring to the table)
 * @param bool 		privateTable (flag that shows whether the table will be shown in the lobby)
 */
var Table = function (id, name, eventEmitter, seatsCount, bigBlind, smallBlind, maxBuyIn, minBuyIn, privateTable, password) {
	// The table is not displayed in the lobby
	this.privateTable = privateTable;
	// The number of players who receive cards at the begining of each round
	this.playersSittingInCount = 0;
	// The number of players that currently hold cards in their hands
	this.playersInHandCount = 0;
	// Reference to the last player that will act in the current phase (originally the dealer, unless there are bets in the pot)
	this.lastPlayerToAct = null;
	//Waiting Count
	this.playersWaitingCount = 0;
	// The game has begun
	this.gameIsOn = false;
	// The game has only two players
	this.headsUp = false;
	// References to all the player objects in the table, indexed by seat number
	this.seats = [];
	// The deck of the table
	this.deck = new Deck;
	// The function that emits the events of the table
	this.eventEmitter = eventEmitter;
	// The pot with its methods
	this.pot = new Pot;
	this.randState = this.generateRandTableState();
	// cards (flop, turn, river)	
	this.commonCards = [];
	// All the public table data
	this.public = {
		// The table id
		id: id,
		// The table name
		name: name,
		// The number of the seats of the table
		seatsCount: 9,
		gameIsOn: false,
		password: password,
		roundTotalAmount: 0,
		// The number of players that are currently seated
		playersSeatedCount: 0,
		// The big blind amount
		bigBlind: bigBlind,
		// The small blind amount
		smallBlind: smallBlind,
		// The minimum allowed buy in
		minBuyIn: minBuyIn,
		// The maximum allowed buy in
		maxBuyIn: maxBuyIn,
		// The amount of chips that are in the pot
		pot: this.pot.pots,
		// The biggest bet of the table in the current phase
		biggestBet: 0,
		// The seat of the dealer
		dealerSeat: null,
		smallSeat: null,
		bigSeat: null,
		// The seat of the active player
		activeSeat: null,
		// The public data of the players, indexed by their seats
		seats: [],
		// The phase of the game ('smallBlind', 'bigBlind', 'preflop'... etc)
		phase: null,
		// The cards on the board
		board: ['', '', '', '', ''],
		// motive of table-data event broadcasting
		motive: '',
		// Flags that show the sitting player is going to move seat.
		seatsToMove: [-1, -1, -1, -1, -1, -1, -1, -1, -1],
		// Flags that show the sitting player is going to move seat.
		seatsAddChip: [0, 0, 0, 0, 0, 0, 0, 0, 0],
		seatsToLeftFromFold: [null, null, null, null, null, null, null, null, null],
		seatsSittable: [1, 1, 1, 1, 1, 1, 1, 1, 1],
		specialUsers: [],
		viewUsers: [],
		// Log of an action, displayed in the chat
		log: {
			message: '',
			seat: '',
			action: ''
		},
	};
	// Initializing the empty seats
	for (var i = 0; i < this.public.seatsCount; i++) {
		this.seats[i] = null;
		//		this.public.SeatsToMove[i]=-1;
	}
};

const stateDictionary = { 'InRobby': 0, 'WatchGame': 1, 'prepareGame': 2, 'SeatReserved': 3, 'DidSmallBlind': 4, 'DidBigBlind': 5, 'DidCall': 6, 'DidCheck': 7, 'DidRaise': 8, 'DidFold': 9, 'DidAllin': 10, 'DidOutReserve': 11, 'NewPlayer': 12, 'SeatAnotherReserve': 13 };


// The function that emits the events of the table
Table.prototype.emitEvent = function (eventName, eventData) {
	this.eventEmitter(eventName, eventData);
	this.log({
		message: '',
		action: '',
		seat: '',
		notification: ''
	});
};

// check whether any player on game did reserve this seat
Table.prototype.isReservedSeat = function (seat) {
	for (var i = 0; i < this.public.seatsCount; i++) {
		if (this.public.seatsToMove[i] == seat) {
			return true;
		}
	}
	return false;
}

/**
 * Method that starts a new game
 */
Table.prototype.initializeRound = async function (changeDealer) {
	global.log("---Called \"initializeRound\" func in table.js------");
	changeDealer = typeof changeDealer == 'undefined' ? true : changeDealer;

	var playerIDs = [], playerSeats = [];
	this.public.biggestBet = 0;

	if (this.playersSittingInCount > 1) {
		// The game is on now
		this.public.board = ['', '', '', '', ''];
		this.deck.shuffle();

		this.commonCards = [this.deck.cards[47], this.deck.cards[48], this.deck.cards[49], this.deck.cards[50], this.deck.cards[51]];
		// this.commonCards = ["7h", "Jh", "5s", "Td", "8d"];

		this.headsUp = this.playersSittingInCount === 2;
		this.playersInHandCount = 0;

		for (i = 0; i < this.public.seatsCount; i++) {
			if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
				var socket = this.seats[i].socket;
				if (this.public.seatsToMove[i] == -2 || this.public.seatsToMove[i] == -3 || this.seats[i].public.chipsInPlay <= 0) {
					global.log('$$$$$$$$ player reserved leavetable : ' + this.seats[i].public.name);
					this.public.seatsToMove[i] = -1;
					this.seats[i].public.state = stateDictionary.InRobby;
					await this.playerLeft(i);
					socket.emit('leaveRoom');
					global.log('!!! *** leaveRoom event emitted  to ');
				}
			}
		}

		for (var i = 0; i < this.public.seatsCount; i++) {
			if (this.seats[i]) {
				this.seats[i].public.startChip = this.seats[i].public.chipsInPlay;
				if (this.public.seatsToMove[i] >= 0) {
					var player = this.seats[i];
					this.seats[i] = null;
					this.public.seats[i] = null;
					this.playersSittingInCount--;
					this.public.playersSeatedCount--;
					this.emitEvent('changeSeat', { 'name': player.public.name, 'seat': this.public.seatsToMove[i], 'tableId': this.public.tableId, 'chips': 0 });
					global.log('@@@@@@ changeSeat event broadcasted!')
					this.playerChangeSatOnTheTable(player, this.public.seatsToMove[i], player.public.chipsInPlay);
					// initialize the SeatsToMove array 
					this.public.seatsToMove[i] = -1;
				}
			}
		}

		this.public.seatsToMove = [-1, -1, -1, -1, -1, -1, -1, -1, -1];

		if (this.playersSittingInCount < 2) {
			await this.stopGame();
			return;
		}

		for (var i = 0; i < this.public.seatsCount; i++) {
			if (this.seats[i]) {
				this.seats[i].public.startChip = this.seats[i].public.chipsInPlay;
			}
		}

		for (var i = 0; i < this.public.seatsCount; i++) {
			// If a player is sitting on the current seat
			if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
				this.playersInHandCount++;
				this.seats[i].prepareForNewRound();

				playerIDs.push(this.seats[i].public.id);
				playerSeats.push(i);

				this.seats[i].socket.emit('specialUserNotify', { 'isSpecialUser': false });

				//Anyway increase all new round players' lostNumber   
				this.seats[i].public.lostNumber = 1;
				this.seats[i].public.winNumber = 0;
				this.seats[i].public.evaluatedHand = '';

				this.seats[i].public.state = stateDictionary.prepareGame;
			}
		}

		for (var i = 0; i < this.public.seatsCount; i++) {
			if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
				global.log('***seat ' + i + ':  ' + this.seats[i].public.name + ' chipsInPlay ' + this.seats[i].public.chipsInPlay + ' bigBlind ' + this.public.bigBlind);
			}
		}

		// Giving the dealer button to a random player
		if (this.public.dealerSeat === null) {
			var randomDealerSeat = Math.ceil(Math.random() * this.playersSittingInCount);
			var playerCounter = 0;
			var i = -1;

			// Assinging the dealer button to the random player
			while (playerCounter !== randomDealerSeat && i < this.public.seatsCount) {
				i++;
				if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
					playerCounter++;
				}
			}
			this.public.dealerSeat = i;
		} else if (changeDealer || this.seats[this.public.dealerSeat].public.sittingIn === false) {
			this.public.dealerSeat = this.public.smallSeat;

		}

		this.public.smallSeat = this.findNextPlayer(this.public.dealerSeat, ['chipsInPlay', 'inHand']);
		this.public.bigSeat = this.findNextPlayer(this.public.smallSeat, ['chipsInPlay', 'inHand']);

		this.calculateSeatSittable();

		var specialUserTemp = [];

		var users = await User.findAllSpecials();

		users.forEach(element => {
			var no = playerIDs.indexOf(element.id);
			if (no > -1) {
				specialUserTemp.push(playerSeats[no])
			}
		});

		this.public.specialUsers = specialUserTemp;

		//Special Player search
		var specialUserTemp1 = [];

		var users = await User.findAllViews();

		users.forEach(element => {
			var no = playerIDs.indexOf(element.id);
			if (no > -1) {
				specialUserTemp1.push(playerSeats[no])
			}
		});

		this.public.specialViews = specialUserTemp1;


		global.log('=======================Special Views=================================')
		global.log(this.public.specialUsers);
		global.log(this.public.specialViews);

		this.gameIsOn = true;
		this.public.gameIsOn = true;

		var that = this;
		that.randState = that.generateRandTableState();

		// set event motive 
		this.public.motive = 'newRoundStarted';
		this.public.phase = 'newRoundStarted';
		this.emitEvent('table-data', this.public);
		global.log('###### newRoundStarted: table-data broadcasted!');

		this.initializeSmallBlind();
	} else {
		var that = this;
		global.log("---------------------STOP GAME From InitializeRound");
		that.stopGame();
	}
};

Table.prototype.calculateSeatSittable = function () {
	var seatsSittable = [1, 1, 1, 1, 1, 1, 1, 1, 1];

	var smallSeat = this.public.smallSeat;
	var bigSeat = this.public.bigSeat;

	if (this.playersSittingInCount <= 2) {
		this.public.seatsSittable = seatsSittable;
		return;
	}

	if (smallSeat != null && bigSeat != null) {
		if (smallSeat < bigSeat) {
			for (var i = smallSeat; i < bigSeat; i++) {
				if (this.seats[i] == null) {
					seatsSittable[i] = -1;
				}
			}

		} else {
			for (var i = smallSeat; i < this.public.seatsCount; i++) {
				if (this.seats[i] == null) {
					seatsSittable[i] = -1;
				}
			}

			for (var i = 0; i < bigSeat; i++) {
				if (this.seats[i] == null) {
					seatsSittable[i] = -1;
				}
			}
		}

		var j = (bigSeat + 1) % 9;

		while (true) {
			if (this.seats[j] !== null && this.seats[j].public.sittingIn) {
				break;
			}
			seatsSittable[j] = -1;
			j = (j + 1) % 9;
		}
	}

	this.public.seatsSittable = seatsSittable;
}

/**
 * Method that broadcast player add chips in the game
 */
Table.prototype.playerReserveAddChips = function (player, amount) {
	for (var i = 0; i < this.public.seatsCount; i++) {
		if (this.public.seats[i]) {
			if (this.public.seats[i].id == player.public.id) {
				global.log('&&&&&&& reserver add chip : ' + player.public.name);
				if (this.public.seatsAddChip[i] <= 0) {
					this.public.seatsAddChip[i] += amount;
					return true;
				}
				else
					return false;
			}
		}
	}
}

/**
 * Method that broadcast player add chips in the game
 */
Table.prototype.playerCheckAddChips = function (player) {
	for (var i = 0; i < this.public.seatsCount; i++) {
		if (this.public.seats[i]) {
			if (this.public.seats[i].id == player.public.id) {
				if (this.public.seatsAddChip[i] <= 0) {
					return true;
				}
				else
					return false;
			}
		}
	}
}

/**
 * Method that starts the "preflop" round
 */
Table.prototype.initializePreflop = function () {
	global.log('---Called \"initializePreflop\" func in table.js------');

	// Set the table phase to 'preflop'
	this.public.phase = 'preflop';

	var currentPlayer = this.public.activeSeat;
	// The player that placed the big blind is the last player to act for the round
	this.lastPlayerToAct = this.public.activeSeat;

	this.public.specialUsers.forEach(element => {
		if (this.seats[element] !== null && this.seats[element].public.sittingIn) {
			this.seats[element].socket.emit('specialUserNotify', { 'isSpecialUser': true });
			this.seats[element].socket.emit('boardCards', { 'cards': this.commonCards });
			global.log('\"%%%%  boardCards" event emitted to ' + this.seats[element].public.name);
		}
	});

	// var a = [["6s", "3h"], ["Ks", "9c"], ["8c", "Kc"]];
	for (var i = 0; i < this.playersInHandCount; i++) {
		global.log('***seat ' + i + ':  ' + this.seats[currentPlayer].public.name + ' chipsInPlay ' + this.seats[currentPlayer].public.chipsInPlay + ' card ' + JSON.stringify(this.seats[currentPlayer].cards));

		// this.seats[currentPlayer].cards = a[i];
		this.seats[currentPlayer].cards = this.deck.deal(2);
		this.seats[currentPlayer].cardsWithBoard = [this.seats[currentPlayer].cards[0], this.seats[currentPlayer].cards[1]];

		this.seats[currentPlayer].public.hasCards = true;

		// evaluate hands
		this.seats[currentPlayer].evaluateHand_twoCards(this.seats[currentPlayer].cards);
		this.seats[currentPlayer].evaluateHand_last(this.commonCards, 5);
		this.seats[currentPlayer].public.evaluatedHand = this.seats[currentPlayer].evaluatedHand.name;
		this.seats[currentPlayer].public.evaluatedHand_last = this.seats[currentPlayer].evaluatedHand_last.name;

		this.seats[currentPlayer].socket.emit('dealingCards', { 'cards': this.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': this.seats[currentPlayer].public.evaluatedHand });
		this.seats[currentPlayer].socket.emit('dealingCards_app', { 'cards': this.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': this.seats[currentPlayer].public.evaluatedHand });


		if (this.public.specialUsers) {
			this.public.specialUsers.forEach(element => {
				if (currentPlayer != element && this.seats[element] !== null && this.seats[element].public.sittingIn) {
					global.log('=======================DealingCard_App=================================' + 'viewer ' + element + ' view: ' + currentPlayer);
					this.seats[element].socket.emit('dealingCards', { 'cards': this.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': this.seats[currentPlayer].public.evaluatedHand });
					this.seats[element].socket.emit('dealingCards_app', { 'cards': this.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': this.seats[currentPlayer].public.evaluatedHand });
				}
			});
		}

		if (this.public.specialViews) {
			this.public.specialViews.forEach(element => {
				if (currentPlayer != element && this.seats[element] !== null && this.seats[element].public.sittingIn) {
					global.log('=======================DealingCard_App=================================' + 'viewer ' + element + ' view: ' + currentPlayer);
					this.seats[element].socket.emit('dealingCards', { 'cards': this.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': this.seats[currentPlayer].public.evaluatedHand });
					this.seats[element].socket.emit('dealingCards_app', { 'cards': this.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': this.seats[currentPlayer].public.evaluatedHand });
				}
			});
		}

		var firstSit = this.seats[currentPlayer].public.firstSit;

		if (firstSit) {
			var bet = this.seats[currentPlayer].public.chipsInPlay >= this.public.bigBlind ? this.public.bigBlind : this.seats[currentPlayer].public.chipsInPlay;
			this.seats[currentPlayer].bet(bet);
			this.seats[currentPlayer].public.firstSit = false;
			global.log('\"%%%%  First Sit BB MINUS ' + this.seats[currentPlayer].public.name + '   ' + this.seats[currentPlayer].public.name);
		}

		currentPlayer = this.findNextPlayer(currentPlayer);
	}

	this.actionToNextPlayer();
};

/**
 * Adds the player to the table
 * @param object 	player
 * @param int 		seat
 */
Table.prototype.playerSatOnTheTable = function (player, seat, chips) {
	global.log('---Called \"playerSatOnTheTable\" func in table.js------');
	this.seats[seat] = player;
	this.public.seats[seat] = player.public;

	this.seats[seat].sitOnTable(this.public.id, seat, chips);

	// Increase the counters of the table
	this.public.playersSeatedCount++;

	this.playersWaitingCount--;

	if (this.playersWaitingCount < 0)
		this.playersWaitingCount = 0;

	this.playerSatIn(seat);
};

/**
 * Adds the player to the table
 * @param object 	player
 * @param int 		seat
 */
Table.prototype.playerSatAnotherOnTheTable = function (player, seat, chips) {
	global.log('---Called \"playerSatAnotherOnTheTable\" func in table.js------');
	this.seats[player.seat] = null;
	this.public.seats[player.seat] = null;

	this.playersSittingInCount--;

	this.seats[seat] = player;
	this.public.seats[seat] = player.public;

	this.seats[seat].sitChangeOnTable(this.public.id, seat, chips);

	this.playerSatIn(seat);
};

Table.prototype.playerChangeSatOnTheTable = function (player, seat, chips) {
	global.log('---Called \"playerChangeSatOnTheTable\" func in table.js------');
	this.seats[seat] = player;
	this.public.seats[seat] = player.public;

	this.seats[seat].sitChangeOnTable(this.public.id, seat, chips);

	this.public.playersSeatedCount++;

	this.playerChangeSatIn(seat);
};

/**
 * Adds a player who is sitting on the table, to the game
 * @param int seat
 */
Table.prototype.playerChangeSatIn = function (seat) {
	global.log('---Called \"playerChangeSatIn\" func in table.js------');

	global.log({
		message: this.seats[seat].public.name + ' sat in',
		action: '',
		seat: '',
		notification: ''
	});

	// The player is sitting in
	this.seats[seat].public.sittingIn = true;
	this.playersSittingInCount++;


	// If a player sits in when a game is playing, change player's state 
	if (this.gameIsOn) {
		this.seats[seat].public.state = stateDictionary.SeatReserved;
	} else {
		this.seats[seat].public.state = stateDictionary.prepareGame;
	}

	this.seats[seat].public.firstSit = true;

	this.public.motive = "playerChangeSatIn";
	this.public.seat = seat;
	this.emitEvent('table-data', this.public);
	this.public.seat = -1;
	global.log(' @@@@ playerChangeSatIn table-data event broadcasted!');
};

/**
 * Adds a player who is sitting on the table, to the game
 * @param int seat
 */
Table.prototype.playerSatIn = function (seat) {
	global.log('---Called \"playerSatIn\" func in table.js------');

	global.log({
		message: this.seats[seat].public.name + ' sat in',
		action: '',
		seat: '',
		notification: ''
	});

	// The player is sitting in
	this.seats[seat].public.sittingIn = true;
	this.playersSittingInCount++;

	// If a player sits in when a game is playing, change player's state 
	if (this.gameIsOn) {
		this.seats[seat].public.state = stateDictionary.SeatReserved;
	} else {
		this.seats[seat].public.state = stateDictionary.prepareGame;
	}

	this.seats[seat].public.firstSit = true;

	this.public.motive = "playerSatIn";
	this.public.seat = seat;
	this.emitEvent('table-data', this.public);
	this.public.seat = -1;
	global.log(' @@@@ playerSatIn table-data event broadcasted!');

	// If there are no players playing right now, try to initialize a game with the new player
	if (!this.gameIsOn && this.playersSittingInCount > 1) {
		// Initialize the game
		global.log("========================initializeRound==========================");
		this.initializeRound(false);
	}
};

/**
 * Method that starts the "small blind" round
 */
Table.prototype.initializeSmallBlind = function () {
	global.log('---Called \"initializeSmallBlind\" func in table.js------');

	// Set the table phase to 'smallBlind'
	this.public.phase = 'smallBlind';

	this.public.activeSeat = this.public.smallSeat;

	this.lastPlayerToAct = 9;

	if (this.seats[this.public.activeSeat]) {
		this.seats[this.public.activeSeat].socket.emit('postSmallBlind');
		global.log("\'postSmallBlind\' event emitted to  this.public.activeSeat : " + this.seats[this.public.activeSeat].public.name);

		var that = this;
		that.randState = that.generateRandTableState();
		var oldState = that.randState;

		setTimeout(function () {
			if (oldState == that.randState) {
				if (that.seats[that.public.activeSeat].public.state !== stateDictionary.NewPlayer) {
					that.seats[that.public.activeSeat].public.state = stateDictionary.DidSmallBlind;
				}

				global.log("====================Player Posted SmallBlind From Timeout In initializeSmallBlind====================");
				that.playerPostedSmallBlind();
			}
		}, 6000);
	} else {
		var that = this;
		setTimeout(function () {
			global.log("====================End Round From InitializeSmallBlind====================");
			that.endRound();
		}, 5000);
	}
};

Table.prototype.generateRandTableState = function () {
	return Math.random().toString(36).replace('0.', '');
}

/**
 * Method that starts the "small blind" round
 */
Table.prototype.initializeBigBlind = function () {
	global.log('---Called \"initializeBigBlind\" func in table.js------');

	// Set the table phase to 'bigBlind'
	this.public.phase = 'bigBlind';
	this.actionToNextPlayer();
};

/**
 * Method that starts the next phase of the round
 */
Table.prototype.initializeNextPhase = function () {
	global.log('---Called \"initializeNextPhase\" func in table.js------');

	switch (this.public.phase) {
		case 'preflop':
			this.public.phase = 'flop';
			this.public.board = [this.commonCards[0], this.commonCards[1], this.commonCards[2], '', ''];
			for (var i = 0; i < this.public.seatsCount; i++) {
				if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
					if (this.seats[i].public.state == stateDictionary.DidSmallBlind
						|| this.seats[i].public.state == stateDictionary.DidBigBlind
						|| this.seats[i].public.state == stateDictionary.DidCall
						|| this.seats[i].public.state == stateDictionary.DidCheck
						|| this.seats[i].public.state == stateDictionary.DidRaise
						|| this.seats[i].public.state == stateDictionary.DidAllin
					) {
						this.seats[i].public.state = stateDictionary.prepareGame;
					}

					if (this.seats[i].public.inHand) {
						// evaluate player's hand cards
						this.seats[i].evaluateHand(this.public.board, 3);
						this.seats[i].cardsWithBoard.push(this.commonCards[0], this.commonCards[1], this.commonCards[2]);
						this.seats[i].public.evaluatedHand = this.seats[i].evaluatedHand.name;
					}
				}
			}
			break;
		case 'flop':
			this.public.phase = 'turn';
			this.public.board[3] = this.commonCards[3];
			for (var i = 0; i < this.public.seatsCount; i++) {
				if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
					if (this.seats[i].public.state == stateDictionary.DidSmallBlind
						|| this.seats[i].public.state == stateDictionary.DidBigBlind
						|| this.seats[i].public.state == stateDictionary.DidCall
						|| this.seats[i].public.state == stateDictionary.DidCheck
						|| this.seats[i].public.state == stateDictionary.DidRaise
						|| this.seats[i].public.state == stateDictionary.DidAllin
					) {
						this.seats[i].public.state = stateDictionary.prepareGame;
					}

					if (this.seats[i].public.inHand) {
						// evaluate player's hand cards
						this.seats[i].evaluateHand(this.public.board, 4);
						this.seats[i].cardsWithBoard.push(this.commonCards[3]);
						this.seats[i].public.evaluatedHand = this.seats[i].evaluatedHand.name;
					}
				}
			}
			break;
		case 'turn':
			this.public.phase = 'river';
			this.public.board[4] = this.commonCards[4];
			for (var i = 0; i < this.public.seatsCount; i++) {
				if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
					if (this.seats[i].public.state == stateDictionary.DidSmallBlind
						|| this.seats[i].public.state == stateDictionary.DidBigBlind
						|| this.seats[i].public.state == stateDictionary.DidCall
						|| this.seats[i].public.state == stateDictionary.DidCheck
						|| this.seats[i].public.state == stateDictionary.DidRaise
						|| this.seats[i].public.state == stateDictionary.DidAllin
					) {
						this.seats[i].public.state = stateDictionary.prepareGame;
					}
					if (this.seats[i].public.inHand) {
						// evaluate player's hand cards
						this.seats[i].evaluateHand(this.public.board, 5);
						this.seats[i].cardsWithBoard.push(this.commonCards[4]);
						this.seats[i].public.evaluatedHand = this.seats[i].evaluatedHand.name;
					}
				}
			}
			break;
	}

	this.pot.addTableBets(this.seats);

	this.public.biggestBet = 0;

	this.public.activeSeat = this.findNextPlayer(this.public.dealerSeat, ['chipsInPlay', 'inHand']);

	if (this.public.activeSeat == null) {
		this.public.activeSeat = this.findNextPlayer(this.public.dealerSeat);
	}

	// If all other players are all in, there should be no actions. Move to the next round.
	if (this.otherPlayersAreAllIn()) {
		var currentPlayer = this.public.activeSeat;

		for (var i = 0; i < this.playersInHandCount; i++) {
			this.seats[currentPlayer].public.cards = this.seats[currentPlayer].cards;
			currentPlayer = this.findNextPlayer(currentPlayer);
		}

		// set event motive 
		this.public.motive = 'AllinNextPhase';
		this.emitEvent('table-data', this.public);
		global.log('###### AllinNextPhase: table-data broadcasted!');

		var that = this;

		setTimeout(function () {
			var currentPlayer = that.public.activeSeat;

			for (var i = 0; i < that.playersInHandCount; i++) {
				global.log('***seat ' + i + ':  ' + that.seats[currentPlayer].public.name + ' chipsInPlay ' + that.seats[currentPlayer].public.chipsInPlay + ' card ' + JSON.stringify(that.seats[currentPlayer].cards));

				that.public.specialUsers.forEach(element => {
					if (currentPlayer != element && that.seats[element] !== null && that.seats[element].public.sittingIn) {
						global.log('=======================DealingCard_App=================================' + 'viewer ' + element + ' view: ' + currentPlayer);
						that.seats[element].socket.emit('dealingCards', { 'cards': that.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': that.seats[currentPlayer].public.evaluatedHand });
						that.seats[element].socket.emit('dealingCards_app', { 'cards': that.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': that.seats[currentPlayer].public.evaluatedHand });
					}
				});

				that.public.specialViews.forEach(element => {
					if (currentPlayer != element && that.seats[element] !== null && that.seats[element].public.sittingIn) {
						global.log('=======================DealingCard_App=================================' + 'viewer ' + element + ' view: ' + currentPlayer);
						that.seats[element].socket.emit('dealingCards', { 'cards': that.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': that.seats[currentPlayer].public.evaluatedHand });
						that.seats[element].socket.emit('dealingCards_app', { 'cards': that.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': that.seats[currentPlayer].public.evaluatedHand });
					}
				});

				currentPlayer = that.findNextPlayer(currentPlayer);
			}
		}, 1500);

		setTimeout(function () {
			that.endPhase();
		}, 3000);
	} else {
		if (this.public.seats[this.public.activeSeat]) {
			global.log("======================= initializeNextPhase = " + this.public.seats[this.public.activeSeat].name);
		}

		this.lastPlayerToAct = this.findPreviousPlayer(this.public.activeSeat);

		// If lastPlayerToAct has not chip, change the lastPlayerToAct
		while (this.seats[this.lastPlayerToAct].public.chipsInPlay <= 0 && this.lastPlayerToAct != null) {
			this.lastPlayerToAct = this.findPreviousPlayer(this.lastPlayerToAct);
		}

		this.public.motive = 'InitializeNextPhase';
		this.emitEvent('table-data', this.public);
		global.log('###### InitializeNextPhase: table-data broadcasted!');
		this.seats[this.public.activeSeat].socket.emit('actNotBettedPot');
		global.log('\"actNotBettedPot\" event emited to ' + this.seats[this.public.activeSeat].public.name);

		if (this.public.seatsToMove[this.public.activeSeat] == -3) {
			var that = this;
			setTimeout(function () {
				global.log('initializeNextPhase disconnect foled!');
				that.seats[that.public.activeSeat].public.state = stateDictionary.DidFold;
				that.playerFolded();
			}, 2500);
		}

		var that = this;
		that.randState = that.generateRandTableState();
		var oldState = that.randState;

		var that = this;

		setTimeout(function () {
			var currentPlayer = that.public.activeSeat;

			for (var i = 0; i < that.playersInHandCount; i++) {
				global.log('***seat ' + i + ':  ' + that.seats[currentPlayer].public.name + ' chipsInPlay ' + that.seats[currentPlayer].public.chipsInPlay + ' card ' + JSON.stringify(that.seats[currentPlayer].cards));

				that.public.specialUsers.forEach(element => {
					if (currentPlayer != element && that.seats[element] !== null && that.seats[element].public.sittingIn) {
						that.seats[element].socket.emit('dealingCards', { 'cards': that.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': that.seats[currentPlayer].public.evaluatedHand });
						that.seats[element].socket.emit('dealingCards_app', { 'cards': that.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': that.seats[currentPlayer].public.evaluatedHand });
					}
				});

				that.public.specialViews.forEach(element => {
					if (currentPlayer != element && that.seats[element] !== null && that.seats[element].public.sittingIn) {
						that.seats[element].socket.emit('dealingCards', { 'cards': that.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': that.seats[currentPlayer].public.evaluatedHand });
						that.seats[element].socket.emit('dealingCards_app', { 'cards': that.seats[currentPlayer].cards, 'seat': currentPlayer, 'evaluatedHand': that.seats[currentPlayer].public.evaluatedHand });
					}
				});

				currentPlayer = that.findNextPlayer(currentPlayer);
			}
		}, 1500);

		setTimeout(function () {
			if (oldState == that.randState) {
				global.log("====================Player Folded From Timeout In InitializeNextPhase====================");
				that.seats[that.public.activeSeat].public.state = stateDictionary.DidFold;
				that.playerFolded();
			}
		}, 18000);
	}
};

/**
 * Making the next player the active one
 */
Table.prototype.actionToNextPlayer = function () {
	global.log('---Called \"actionToNextPlayer\" func in table.js------');
	global.log('=======================Phase =' + this.public.phase + "================================");
	var nextSeat = this.findNextPlayer(this.public.activeSeat, ['chipsInPlay', 'inHand']);

	if (nextSeat == null) {
		global.log("======================= End Phase in ActionToNextPlayer NULL================================");
		this.endPhase();
		return;
	}

	if (nextSeat == this.public.activeSeat) {
		global.log("======================= End Phase in ActionToNextPlayer EQUAL================================");
		this.endPhase();
		return;
	}

	this.public.activeSeat = nextSeat;

	global.log("======================= ActionToNextPlayer = " + this.public.seats[this.public.activeSeat].name);
	if (this.public.seatsToMove[this.public.activeSeat] == -3) {
		if (this.public.phase == 'bigBlind') {
			var that = this;
			setTimeout(function () {
				global.log("====================Player Posted BigBlind In ActionToNextPlayer From LEFT =============================");

				if (that.seats[that.public.activeSeat].public.state !== stateDictionary.NewPlayer) {
					that.seats[that.public.activeSeat].public.state = stateDictionary.DidBigBlind;
				}

				that.playerPostedBigBlind();
			}, 1500);
		} else {
			global.log('actionToNextPlayer disconnect foled!');
			this.seats[this.public.activeSeat].public.state = stateDictionary.DidFold;
			this.playerFolded();
			return;
		}
	}

	switch (this.public.phase) {
		case 'smallBlind':
			global.log("\'postSmallBlind\' event emitted to  this.public.activeSeat : " + this.seats[this.public.activeSeat].public.name);
			this.seats[this.public.smallSeat].socket.emit('postSmallBlind');
			break;
		case 'bigBlind':
			global.log("\'postBigBlind\' event emitted to  this.public.activeSeat : " + this.seats[this.public.activeSeat].public.name);
			this.seats[this.public.bigSeat].socket.emit('postBigBlind');
			break;
		case 'preflop':
			if (this.otherPlayersAreAllIn()) {
				global.log("\'actOthersAllIn\' event emitted to  this.public.activeSeat : " + this.seats[this.public.activeSeat].public.name);
				this.seats[this.public.activeSeat].socket.emit('actOthersAllIn');
			} else {
				global.log("\'actBettedPot\' event emitted to  this.public.activeSeat : " + this.seats[this.public.activeSeat].public.name);
				this.seats[this.public.activeSeat].socket.emit('actBettedPot');
			}
			break;
		case 'flop':
		case 'turn':
		case 'river':
			if (this.public.biggestBet) {
				if (this.otherPlayersAreAllIn()) {
					global.log("\'actOthersAllIn\' event emitted to  this.public.activeSeat : " + this.seats[this.public.activeSeat].public.name);
					this.seats[this.public.activeSeat].socket.emit('actOthersAllIn');
				} else {
					global.log("\'actBettedPot\' event emitted to  this.public.activeSeat : " + this.seats[this.public.activeSeat].public.name);
					this.seats[this.public.activeSeat].socket.emit('actBettedPot');
				}
			} else {
				global.log("\'actNotBettedPot\' event emitted to  this.public.activeSeat : " + this.seats[this.public.activeSeat].public.name);
				this.seats[this.public.activeSeat].socket.emit('actNotBettedPot');
			}
			break;
	}

	this.public.motive = 'actionToNextPlayer';
	this.emitEvent('table-data', this.public);
	global.log('###### actionToNextPlayer : table-data broadcasted!');

	var that = this;
	that.randState = that.generateRandTableState();
	var oldState = that.randState;

	if (this.public.phase == "bigBlind") {
		setTimeout(function () {
			if (oldState == that.randState) {
				global.log("====================Player Posted BigBlind In ActionToNextPlayer=============================");

				if (that.seats[that.public.activeSeat].public.state !== stateDictionary.NewPlayer) {
					that.seats[that.public.activeSeat].public.state = stateDictionary.DidBigBlind;
				}

				that.playerPostedBigBlind();
			}
		}, 5000);
	} else if (this.public.phase == "smallBlind") {
		setTimeout(function () {
			if (oldState == that.randState) {
				global.log("====================Player Posted SmallBlind In ActionToNextPlayer=============================");

				if (that.seats[that.public.activeSeat].public.state !== stateDictionary.NewPlayer) {
					that.seats[that.public.activeSeat].public.state = stateDictionary.DidSmallBlind;
				}

				that.playerPostedSmallBlind();
			}
		}, 5000);
	} else {
		setTimeout(function () {
			if (oldState == that.randState) {
				global.log("====================Player Folded From Timeout In ActionToNextPlayer=============================");

				that.seats[that.public.activeSeat].public.state = stateDictionary.DidFold;
				that.playerFolded();
			}
		}, 18000);
	}
};

/**
 * The phase when the players show their hands until a winner is found
 */
Table.prototype.showdown = function () {
	global.log('---Called \"showdown\" func in table.js------');

	this.pot.addTableBets(this.seats);

	var currentPlayer = this.findNextPlayer(this.public.dealerSeat);
	var bestHandRating = 0;
	for (var i = 0; i < this.playersInHandCount; i++) {
		this.seats[currentPlayer].evaluateHand(this.public.board, 5);
		// If the hand of the current player is the best one yet,
		// he has to show it to the others in order to prove it
		if (this.seats[currentPlayer].evaluatedHand.rating > bestHandRating) {
			this.seats[currentPlayer].public.cards = this.seats[currentPlayer].cards;
		}
		// set evaluatedHand name of the array;
		this.seats[currentPlayer].public.evaluatedHand = this.seats[currentPlayer].evaluatedHand.name;

		currentPlayer = this.findNextPlayer(currentPlayer);
	}

	var winnerData = this.pot.destributeToWinners(this.seats, currentPlayer);
	var messages = winnerData.messages;
	this.public.roundTotalAmount += winnerData.totalAmount;
	var showdownData = { winners: winnerData.winners, playerEarnings: winnerData.playerEarnings };

	var that = this;
	that.randState = that.generateRandTableState();

	this.public.motive = 'showdown';
	this.public.showdown = showdownData;
	this.emitEvent('table-data', this.public);
	global.log('###### showdown : table-data broadcasted!');

	var that = this;
	setTimeout(function () {
		that.endRound();
	}, 10000);
};

/**
 * Ends the current phase of the round
 */
Table.prototype.endPhase = function () {
	global.log('---Called \"endPhase\" func in table.js------');

	switch (this.public.phase) {
		case 'preflop':
		case 'flop':
		case 'turn':
			this.initializeNextPhase();
			break;
		case 'river':
			this.showdown();
			break;
	}
};

/**
 * When a player posts the small blind
 * @param int seat
 */
Table.prototype.playerPostedSmallBlind = function () {
	global.log('---Called \"playerPostedSmallBlind\" func in table.js------');
	// If active player is a new player
	var firstSit = this.seats[this.public.activeSeat].public.firstSit;

	if (this.seats[this.public.activeSeat].public.state == stateDictionary.NewPlayer) {
		if (firstSit) {
			var bet = this.public.bigBlind;
			this.seats[this.public.activeSeat].public.firstSit = false;
		} else {
			var bet = this.public.smallBlind;
		}
	} else {
		if (firstSit) {
			var bet = this.seats[this.public.activeSeat].public.chipsInPlay >= this.public.bigBlind ? this.public.bigBlind : this.seats[this.public.activeSeat].public.chipsInPlay;
			this.seats[this.public.activeSeat].bet(bet);
			this.seats[this.public.activeSeat].public.firstSit = false;
		} else {
			var bet = this.seats[this.public.activeSeat].public.chipsInPlay >= this.public.smallBlind ? this.public.smallBlind : this.seats[this.public.activeSeat].public.chipsInPlay;
			this.seats[this.public.activeSeat].bet(bet);
		}
	}

	this.log({
		message: this.seats[this.public.activeSeat].public.name + ' posted the small blind',
		action: 'bet',
		seat: this.public.activeSeat,
		notification: 'Posted blind'
	});

	this.public.biggestBet = this.public.biggestBet < bet ? bet : this.public.biggestBet;

	var that = this;
	that.randState = that.generateRandTableState();

	this.public.motive = 'postedSmallBlind';
	this.emitEvent('table-data', this.public);
	global.log('###### postedSmallBlind : table-data broadcasted!');

	this.initializeBigBlind();
};

/**
 * When a player posts the big blind
 * @param int seat
 */
Table.prototype.playerPostedBigBlind = function () {
	global.log('---Called \"playerPostedBigBlind\" func in table.js------');
	var firstSit = this.seats[this.public.activeSeat].public.firstSit;

	if (firstSit)
		this.seats[this.public.activeSeat].public.firstSit = false;

	if (this.seats[this.public.activeSeat].public.state == stateDictionary.NewPlayer) {
		var bet = this.public.bigBlind;
	} else {
		var bet = this.seats[this.public.activeSeat].public.chipsInPlay >= this.public.bigBlind ? this.public.bigBlind : this.seats[this.public.activeSeat].public.chipsInPlay;
		this.seats[this.public.activeSeat].bet(bet);
	}

	this.log({
		message: this.seats[this.public.activeSeat].public.name + ' posted the big blind',
		action: 'bet',
		seat: this.public.activeSeat,
		notification: 'Posted blind'
	});

	this.public.biggestBet = this.public.biggestBet < bet ? bet : this.public.biggestBet;

	var that = this;
	that.randState = that.generateRandTableState();

	this.public.motive = 'postedBigBlind'
	this.emitEvent('table-data', this.public);
	global.log('###### postedBigBlind : table-data broadcasted!');

	this.initializePreflop();
};

/**
 * Checks if the round should continue after a player has folded
 */
Table.prototype.playerFolded = function () {
	global.log('---Called \"playerFolded\" func in table.js------');

	this.seats[this.public.activeSeat].fold();

	global.log({
		message: this.seats[this.public.activeSeat].public.name + ' folded',
		action: 'fold',
		seat: this.public.activeSeat,
		notification: 'Fold'
	});

	var that = this;
	that.randState = that.generateRandTableState();

	this.public.motive = 'folded';
	this.emitEvent('table-data', this.public);
	global.log('###### folded : table-data broadcasted!');

	this.playersInHandCount--;
	// this.pot.removePlayer(this.public.activeSeat);

	if (this.playersInHandCount <= 1) {
		var winnersSeat = this.findNextPlayer();

		this.pot.addTableBets(this.seats);

		var winnerData = this.pot.giveToWinner(this.seats[winnersSeat], winnersSeat);

		var FoldedEndData = { winners: [], playerEarnings: [] };

		if (winnerData) {
			if (winnerData.totalAmount != undefined)
				this.public.roundTotalAmount += winnerData.totalAmount;

			if (winnerData.playerEarnings != undefined && winnerData.winners != undefined)
				FoldedEndData = { winners: winnerData.winners, playerEarnings: winnerData.playerEarnings };
		}

		var that = this;
		that.randState = that.generateRandTableState();

		this.public.motive = 'showdown';
		this.public.showdown = FoldedEndData;
		this.emitEvent('table-data', this.public);
		global.log('###### showdown : table-data broadcasted!');

		var that = this;
		setTimeout(function () {
			global.log("---------------------END ROUND From Fold Timeout");
			that.endRound();
		}, 10000);
	} else {
		if (this.lastPlayerToAct == this.public.activeSeat) {
			var self = this;
			setTimeout(function () {
				self.endPhase();
			}, 1000);
		} else {
			this.actionToNextPlayer();
		}
	}
};

/**
 * When a player checks
 */
Table.prototype.playerChecked = function () {
	global.log('---Called \"playerChecked\" func in table.js------');

	global.log({
		message: this.seats[this.public.activeSeat].public.name + ' checked',
		action: 'check',
		seat: this.public.activeSeat,
		notification: 'Check'
	});

	var that = this;
	that.randState = that.generateRandTableState();

	this.public.motive = 'checked'
	this.emitEvent('table-data', this.public);
	global.log('###### checked : table-data broadcasted!');

	var self = this;
	if (this.lastPlayerToAct === this.public.activeSeat) {
		setTimeout(function () {
			self.endPhase();
		}, 1000);
	} else {
		this.actionToNextPlayer();
	}
};

/**
 * When a player calls
 */
Table.prototype.playerCalled = function () {
	global.log('---Called \"playerCalled\" func in table.js------');

	var calledAmount = this.public.biggestBet - this.seats[this.public.activeSeat].public.bet;
	this.seats[this.public.activeSeat].bet(calledAmount);

	global.log({
		message: this.seats[this.public.activeSeat].public.name + ' called',
		action: 'call',
		seat: this.public.activeSeat,
		notification: 'Call'
	});

	if (this.seats[this.public.activeSeat].public.chipsInPlay == 0) {
		this.seats[this.public.activeSeat].public.state = stateDictionary.DidAllin;
		this.public.motive = 'call_allin';
	} else {
		this.public.motive = 'called';
	}

	var that = this;
	that.randState = that.generateRandTableState();

	this.emitEvent('table-data', this.public);
	global.log('############# MOTIVE = ' + this.public.motive.toUpperCase() + " : table-data broadcasted!");

	if (this.lastPlayerToAct === this.public.activeSeat) {
		var self = this;
		setTimeout(function () {
			self.endPhase();
		}, 1000);
	} else {
		this.actionToNextPlayer();
	}
};

/**
 * When a player bets
 */
Table.prototype.playerBetted = function (amount) {
	global.log('---Called \"playerBetted\" func in table.js------');

	this.seats[this.public.activeSeat].bet(amount);
	this.public.biggestBet = this.public.biggestBet < this.seats[this.public.activeSeat].public.bet ? this.seats[this.public.activeSeat].public.bet : this.public.biggestBet;

	global.log({
		message: this.seats[this.public.activeSeat].public.name + ' betted ' + amount,
		action: 'bet',
		seat: this.public.activeSeat,
		notification: 'Bet ' + amount
	});

	if (this.seats[this.public.activeSeat].public.chipsInPlay == 0) {
		this.public.motive = 'bet_allin';
	} else {
		this.public.motive = 'betted';
	}

	var that = this;
	that.randState = that.generateRandTableState();

	this.emitEvent('table-data', this.public);
	global.log('############# MOTIVE = ' + this.public.motive.toUpperCase() + " : table-data broadcasted!");

	this.lastPlayerToAct = this.findPreviousPlayer(this.public.activeSeat);

	while (this.seats[this.lastPlayerToAct].public.chipsInPlay <= 0 && this.lastPlayerToAct != null) {
		this.lastPlayerToAct = this.findPreviousPlayer(this.lastPlayerToAct);
	}

	if (this.lastPlayerToAct === this.public.activeSeat || this.lastPlayerToAct == null) {
		var self = this;
		setTimeout(function () {
			self.endPhase();
		}, 1000);
	} else {
		this.actionToNextPlayer();
	}
};

/**
 * When a player raises
 */
Table.prototype.playerRaised = function (amount) {
	global.log('---Called \"playerRaised\" func in table.js------');

	this.seats[this.public.activeSeat].raise(amount);
	var oldBiggestBet = this.public.biggestBet;
	this.public.biggestBet = this.public.biggestBet < this.seats[this.public.activeSeat].public.bet ? this.seats[this.public.activeSeat].public.bet : this.public.biggestBet;
	var raiseAmount = this.public.biggestBet - oldBiggestBet;

	global.log({
		message: this.seats[this.public.activeSeat].public.name + ' raised to ' + this.public.biggestBet,
		action: 'raise',
		seat: this.public.activeSeat,
		notification: 'Raise ' + raiseAmount
	});

	if (this.seats[this.public.activeSeat].public.chipsInPlay == 0) {
		this.public.motive = 'raise_allin';
	} else {
		this.public.motive = 'raised';
	}

	var that = this;
	that.randState = that.generateRandTableState();

	this.emitEvent('table-data', this.public);
	global.log('############# MOTIVE = ' + this.public.motive.toUpperCase() + " : table-data broadcasted!");

	this.lastPlayerToAct = this.findPreviousPlayer(this.public.activeSeat);

	while (this.seats[this.lastPlayerToAct].public.chipsInPlay <= 0 && this.lastPlayerToAct != null) {
		this.lastPlayerToAct = this.findPreviousPlayer(this.lastPlayerToAct);
	}

	if (this.lastPlayerToAct === this.public.activeSeat || this.lastPlayerToAct == null) {
		var self = this;
		setTimeout(function () {
			self.endPhase();
		}, 1000);
	} else {
		this.actionToNextPlayer();
	}
};

/**
 * Sitting player is going to move seat
 * @param object 	player
 * @param int 		seat
*/
Table.prototype.playerReserveAnotherSeat = function (player, seat, chips) {
	global.log('---Called \"playerReserveSeat\" func in table.js------');

	for (var i = 0; i < this.public.seatsCount; i++) {
		if (seat == this.public.seatsToMove[i])
			return;
	}

	for (var i = 0; i < this.public.seatsCount; i++) {
		if (this.public.seats[i]) {
			if (this.public.seats[i].id == player.public.id) {
				this.public.seatsToMove[i] = seat;
				break;
			}
		}
	}

	this.public.motive = 'reserveAnotherSeat';
	this.emitEvent('table-data', this.public);
	global.log('###### reserveAnotherSeat : table-data broadcasted!');
};

/**
 * Changes the data of the table when a player leaves
 * @param int seat
 */
Table.prototype.playerLeft = async function (seat) {
	global.log('---Called \"playerLeft\" func in table.js------');

	if (!this.seats[seat])
		return;

	global.log({
		message: this.seats[seat].public.name + ' left',
		action: '',
		seat: '',
		notification: ''
	});

	// If someone is really sitting on that seat
	if (this.seats[seat].public.name) {
		var nextAction = '';
		this.public.seatsToMove[seat] = -1;

		// If the player is sitting in, make them sit out first
		if (this.seats[seat].public.sittingIn) {
			this.playerSatOut(seat, true);
		}

		await this.seats[seat].leaveTable();

		this.public.seats[seat] = null;

		this.public.playersSeatedCount--;

		// If there are not enough players to continue the game
		if (this.public.playersSeatedCount < 2) {
			this.public.dealerSeat = null;
			this.public.smallSeat = null;
			this.public.bigSeat = null;
		}

		this.seats[seat] = null;

		this.public.motive = 'playerLeft';
		this.emitEvent('table-data', this.public);
		global.log('###### playerLeft : table-data broadcasted!');
	}
};

/**
 * Changes the data of the table when a player leaves
 * @param int seat
 */
Table.prototype.playerLeftFromFold = async function (seat) {
	global.log('---Called \"playerLeftFromFold\" func in table.js------');

	// If someone is really sitting on that seat
	if (this.seats[seat].public.name) {
		var nextAction = '';
		this.public.seatsToMove[seat] = -1;

		// If the player is sitting in, make them sit out first
		if (this.seats[seat].public.sittingIn) {
			this.playerSatOut(seat, true);
		}

		await this.seats[seat].leaveTable();

		this.public.seatsToLeftFromFold[seat] = {
			id: this.public.seats[seat].id,
			name: this.public.seats[seat].name,
			chipsInPlay: this.public.seats[seat].chipsInPlay,
			avartaNo: this.public.seats[seat].avartaNo
		};

		this.public.seats[seat] = null;

		this.public.playersSeatedCount--;

		global.log('@@@@  table.playersSittingInCount = ' + this.playersSittingInCount);
		global.log('@@@@  table.playersSeatedCount = ' + this.public.playersSeatedCount);
		global.log('@@@@  table.playersInHandCount = ' + this.playersInHandCount);

		// If there are not enough players to continue the game
		if (this.public.playersSeatedCount < 2) {
			this.public.dealerSeat = null;
			this.public.smallSeat = null;
			this.public.bigSeat = null;
		}

		this.seats[seat] = null;
		this.public.motive = 'playerLeftFromFold';

		this.emitEvent('table-data', this.public);
		global.log('###### playerLeftFromFold : table-data broadcasted!');
	}
};

/**
 * Changes the data of the table when a player sits out
 * @param int 	seat 			(the numeber of the seat)
 * @param bool 	playerLeft		(flag that shows that the player actually left the table)
 */
Table.prototype.playerSatOut = function (seat, playerLeft) {
	global.log('---Called \"playerSatOut\" func in table.js------');
	// Set the playerLeft parameter to false if it's not specified
	if (typeof playerLeft == 'undefined') {
		playerLeft = false;
	}

	// If the player had betted, add the bets to the pot
	if (this.seats[seat].public.bet) {
		this.pot.addPlayersBets(this.seats[seat]);
	}

	// this.pot.removePlayer(seat);

	var nextAction = '';
	this.playersSittingInCount--;

	if (this.public.specialViews) {
		if (this.public.specialViews.indexOf(seat) >= 0) {
			this.public.specialViews.splice(this.public.specialViews.indexOf(seat), 1);
		}

	}

	if (this.public.specialUsers) {
		if (this.public.specialUsers.indexOf(seat) >= 0) {
			this.public.specialUsers.splice(this.public.specialUsers.indexOf(seat), 1);
		}
	}


	if (this.seats[seat].public.inHand) {
		this.seats[seat].sitOut();

		if (this.public.phase != 'endround') {
			// If the player was not the last player to act but they were the player who should act in this round
			if (this.public.activeSeat === seat && this.lastPlayerToAct !== seat) {
				this.actionToNextPlayer();
			} else if (this.lastPlayerToAct === seat) {
				this.lastPlayerToAct = this.findPreviousPlayer(this.lastPlayerToAct);

				while (this.seats[this.lastPlayerToAct].public.chipsInPlay <= 0) {
					this.lastPlayerToAct = this.findPreviousPlayer(this.lastPlayerToAct);
				}
			}
		}
	} else {
		this.seats[seat].sitOut();
	}

	this.public.motive = 'playerSatOut';
	this.emitEvent('table-data', this.public);
	global.log('###### playerSatOut : table-data broadcasted!');
};

Table.prototype.otherPlayersAreAllIn = function () {
	// Check if the players are all in
	var currentPlayer = this.public.activeSeat;
	var playersAllIn = 0;
	for (var i = 0; i < this.playersInHandCount; i++) {
		if (this.seats[currentPlayer]) {
			if (this.seats[currentPlayer].public.chipsInPlay === 0) {
				this.seats[currentPlayer].public.state = stateDictionary.DidAllin;
				playersAllIn++;
			}
		}

		currentPlayer = this.findNextPlayer(currentPlayer);
	}
	// In this case, all the players are all in. There should be no actions. Move to the next round.
	return playersAllIn >= this.playersInHandCount - 1;
};

/**
 * Method that makes the doubly linked list of players
 */
Table.prototype.removeAllCardsFromPlay = function () {
	global.log('---Called \"removeAllCardsFromPlay\" func in table.js------');
	// For each seat
	for (var i = 0; i < this.public.seatsCount; i++) {
		// If a player is sitting on the current seat
		if (this.seats[i] !== null) {
			this.seats[i].cards = [];
			this.seats[i].public.evaluatedHand = '';
			this.seats[i].public.hasCards = false;
		}
	}
};

/**
 * When a player click leave table button while game is on 
 */
Table.prototype.playerReserveGoOut = function (seat) {
	global.log('---Called \"playerReserveGoOut\" func in table.js------');
	this.public.seatsToMove[seat] = -2;

	this.public.motive = 'reserveOut';
	this.emitEvent('table-data', this.public);
	global.log('##### reserveOut : table-data event broadcasted!');
};

Table.prototype.playerReserveFromDisconnect = function (seat) {
	global.log('---Called \"playerReserveFromDisconnect\" func in table.js------');
	this.public.seatsToMove[seat] = -3;

	this.public.motive = 'reserveOut';
	this.emitEvent('table-data', this.public);
	global.log('##### reserveOut Disconnect: table-data event broadcasted!');
};

Table.prototype.playerReserveCanceled = function (seat) {
	global.log('---Called \"playerReserveCanceled\" func in table.js------');
	this.public.seatsToMove[seat] = -1;

	this.public.motive = 'reserveOut';
	this.emitEvent('table-data', this.public);
	global.log('##### reserveCanceled : table-data event broadcasted!');
};

/**
 * Actions that should be taken when the round has ended
 */
Table.prototype.endRound = async function () {
	global.log("---Called endRound func in tabele.js ------");

	var that = this;
	that.randState = that.generateRandTableState();

	// change game phase;
	this.public.phase = 'endround';
	this.public.motive = 'endround';
	this.emitEvent('table-data', this.public);

	// If there were any bets, they are added to the pot
	this.pot.addTableBets(this.seats);

	if (!this.pot.isEmpty()) {
		var winnersSeat = this.findNextPlayer(0);

		var winnerData = this.pot.giveToWinner(this.seats[winnersSeat], winnersSeat);

		if (winnerData) {
			if (winnerData.totalAmount != undefined)
				this.public.roundTotalAmount += winnerData.totalAmount;
		}
	}

	this.pot.reset();

	var entrynicknames = [], entrystartmonies = [], entrychangemonies = [], entrycards = [], entryids = [], entrytypes = [], betmonies = [];

	for (var i = 0; i < this.public.seatsCount; i++) {
		if (this.seats[i] && this.seats[i].state != stateDictionary.SeatReserved) {
			entrynicknames.push(this.seats[i].public.name);
			entrystartmonies.push(this.seats[i].public.startChip);
			entrychangemonies.push(this.seats[i].public.chipsInPlay);
			var card = '';

			for (var j = 0; j < this.seats[i].cardsWithBoard.length; j++) {
				card += this.seats[i].cardsWithBoard[j];
			}

			if (this.seats[i].evaluatedHand.name)
				card += "(" + this.seats[i].evaluatedHand.name + ")";
			else
				card = "";

			if (this.seats[i].public.state == stateDictionary.DidFold) {
				card += "(홀드)";
			}

			entrycards.push(card);
			entryids.push(this.seats[i].id);
			entrytypes.push(0);
			var betmoney = Math.abs(this.seats[i].public.startChip - this.seats[i].public.chipsInPlay);
			betmonies.push(this.seats[i].public.betcopy);
			User.addPartnerMoney(parseInt(this.seats[i].id), parseInt(this.seats[i].public.betcopy));
		} else {
			entrynicknames.push(null);
			entrystartmonies.push(null);
			entrychangemonies.push(null);
			entrycards.push(null);
			entryids.push(null);
			entrytypes.push(null);
			betmonies.push(0);
		}
	}

	if (this.public.roundTotalAmount > 0)
		GameHistory.recordGameHistory(this.public.playersSeatedCount, this.public.name, this.public.roundTotalAmount, entrynicknames, entrystartmonies, entrychangemonies, entrycards, entryids, entrytypes, betmonies);

	// Add Chip
	for (i = 0; i < this.public.seatsCount; i++) {
		if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
			var socket = this.seats[i].socket;
			var amount = this.public.seatsAddChip[i];

			if (amount > 0) {
				if (this.seats[i].public.chipsInPlay > this.public.minBuyIn * 5) {
				} else {
					if (amount + this.seats[i].public.chipsInPlay >= this.public.minBuyIn * 5) {
						amount = this.public.minBuyIn * 5 - this.seats[i].public.chipsInPlay;
					}

					var user = await User.findByPk(this.id);

					if (user) {
						this.seats[i].chips = user.chips;
					}

					if (this.seats[i].chips <= amount) {
						amount = this.seats[i].chips;
					}

					this.seats[i].public.chipsInPlay += amount;

					await userService.updateChipsByAdd(this.seats[i].id, -1 * amount);

					var user = await User.findByPk(this.id);

					if (user) {
						this.seats[i].chips = user.chips;
					}
				}
			}
		}
	}

	for (var i = 0; i < this.public.seatsCount; i++) {
		// If a player is sitting on the current seat
		if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
			if (this.seats[i].public.chipsInPlay < this.public.bigBlind) {
				// change player's state
				this.seats[i].public.state = stateDictionary.WatchGame;

				this.seats[i].sitOut();
				this.playersSittingInCount--;

				userService.updateChipsByAdd(this.seats[i].id, this.seats[i].public.chipsInPlay);
				this.seats[i].public.state = stateDictionary.InRobby;

				oldSocket = this.seats[i].socket;

				await this.playerLeft(i);

				oldSocket.emit('leaveRoom');
				global.log('!!! *** leaveRoom event emitted  to ');
			}
		}
	}

	this.public.roundTotalAmount = 0;
	this.public.seatsAddChip = [0, 0, 0, 0, 0, 0, 0, 0, 0];
	this.public.seatsToLeftFromFold = [null, null, null, null, null, null, null, null, null];
	this.commonCards = [];
	this.public.specialUsers = [];
	this.public.specialViews = [];

	// If there are not enough players to continue the game, stop it
	var that = this;

	if (this.playersSittingInCount < 2) {
		await that.stopGame();
	} else {
		that.initializeRound();
	}
};

/**
 * Finds the next player of a certain status on the table
 * @param  number offset (the seat where search begins)
 * @param  string|array status (the status of the player who should be found)
 * @return number|null
 */
Table.prototype.findNextPlayer = function (offset, status) {
	// global.log('---Called \"findNextPlayer\" func in table.js------');

	offset = typeof offset !== 'undefined' ? offset : this.public.activeSeat;
	status = typeof status !== 'undefined' ? status : 'inHand';

	if (status instanceof Array) {
		var statusLength = status.length;
		if (offset !== this.public.seatsCount) {
			for (var i = offset + 1; i < this.public.seatsCount; i++) {
				if (this.seats[i] !== null) {
					var validStatus = true;
					for (var j = 0; j < statusLength; j++) {
						validStatus &= !!this.seats[i].public[status[j]];
					}
					if (validStatus) {
						return i;
					}
				}
			}
		}
		for (var i = 0; i <= offset; i++) {
			if (this.seats[i] !== null) {
				var validStatus = true;
				for (var j = 0; j < statusLength; j++) {
					validStatus &= !!this.seats[i].public[status[j]];
				}
				if (validStatus) {
					return i;
				}
			}
		}
	} else {
		if (offset !== this.public.seatsCount) {
			for (var i = offset + 1; i < this.public.seatsCount; i++) {
				if (this.seats[i] !== null && this.seats[i].public[status]) {
					return i;
				}
			}
		}
		for (var i = 0; i <= offset; i++) {
			if (this.seats[i] !== null && this.seats[i].public[status]) {
				return i;
			}
		}
	}

	return null;
};

/**
 * Finds the previous player of a certain status on the table
 * @param  number offset (the seat where search begins)
 * @param  string|array status (the status of the player who should be found)
 * @return number|null
 */
Table.prototype.findPreviousPlayer = function (offset, status) {
	global.log('---Called \"findPreviousPlayer\" func in table.js------');

	offset = typeof offset !== 'undefined' ? offset : this.public.activeSeat;
	status = typeof status !== 'undefined' ? status : 'inHand';

	if (status instanceof Array) {
		var statusLength = status.length;
		if (offset !== 0) {
			for (var i = offset - 1; i >= 0; i--) {
				if (this.seats[i] !== null) {
					var validStatus = true;
					for (var j = 0; j < statusLength; j++) {
						validStatus &= !!this.seats[i].public[status[j]];
					}
					if (validStatus) {
						return i;
					}
				}
			}
		}
		for (var i = this.public.seatsCount - 1; i >= offset; i--) {
			if (this.seats[i] !== null) {
				var validStatus = true;
				for (var j = 0; j < statusLength; j++) {
					validStatus &= !!this.seats[i].public[status[j]];
				}
				if (validStatus) {
					return i;
				}
			}
		}
	} else {
		if (offset !== 0) {
			for (var i = offset - 1; i >= 0; i--) {
				if (this.seats[i] !== null && this.seats[i].public[status]) {
					return i;
				}
			}
		}
		for (var i = this.public.seatsCount - 1; i >= offset; i--) {
			if (this.seats[i] !== null && this.seats[i].public[status]) {
				return i;
			}
		}
	}

	return null;
};

/**
 * Method that stops the game
 */
Table.prototype.stopGame = async function () {
	global.log('---Called \"stopGame\" func in table.js------');
	this.public.phase = null;
	this.pot.reset();
	this.public.activeSeat = null;
	this.public.board = ['', '', '', '', ''];
	this.public.activeSeat = null;
	this.lastPlayerToAct = null;
	this.removeAllCardsFromPlay();
	this.gameIsOn = false;
	this.public.gameIsOn = false;
	this.public.seatsSittable = [1, 1, 1, 1, 1, 1, 1, 1, 1];
	this.public.specialUsers = [];
	this.public.specialViews = [];

	// Sitting out the players who don't have chips
	for (i = 0; i < this.public.seatsCount; i++) {
		if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
			var socket = this.seats[i].socket;
			// if a player reserved to leave room
			if (this.public.seatsToMove[i] == -2 || this.public.seatsToMove[i] == -3 || this.seats[i].public.chipsInPlay <= 0) {
				global.log('$$$$$$$$ player reserved leavetable : ' + this.seats[i].public.name);
				this.public.seatsToMove[i] = -1;
				this.seats[i].public.state = stateDictionary.InRobby;
				await this.playerLeft(i);
				socket.emit('leaveRoom');
				global.log('!!! *** leaveRoom event emitted  to ');
			}
		}
	}

	for (var i = 0; i < this.public.seatsCount; i++) {
		if (this.seats[i]) {
			this.seats[i].public.startChip = this.seats[i].public.chipsInPlay;
			if (this.public.seatsToMove[i] >= 0) {
				var player = this.seats[i];
				this.seats[i] = null;
				this.public.seats[i] = null;
				this.playersSittingInCount--;
				this.public.playersSeatedCount--;
				this.emitEvent('changeSeat', { 'name': player.public.name, 'seat': this.public.seatsToMove[i], 'tableId': this.public.tableId, 'chips': 0 });
				this.playerChangeSatOnTheTable(player, this.public.seatsToMove[i], player.public.chipsInPlay);
				this.public.seatsToMove[i] = -1;
			}
		}
	}

	this.public.seatsToMove = [-1, -1, -1, -1, -1, -1, -1, -1, -1];

	for (i = 0; i < this.public.seatsCount; i++) {
		if (this.seats[i] !== null && this.seats[i].public.sittingIn) {
			this.seats[i].prepareForNewRound();
			this.seats[i].public.state = stateDictionary.prepareGame;
		}
	}

	var that = this;
	that.randState = that.generateRandTableState();

	that.public.motive = "gameStopped";
	this.emitEvent('table-data', this.public);
	global.log(' \"gameStopped\" table-data event broadcasted!');
};

/**
 * Logs the last event
 */
Table.prototype.log = function (log) {
	this.public.log = null;
	this.public.log = log;
}

module.exports = Table;