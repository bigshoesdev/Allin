/**
 * The seat directive. It requires two attributes.
 * seatIndex: The index of the player in the "seats" array
 * cellNumber: The number of the cell in the grid (used for styles)
 */
app.directive( 'seatshow', [function() {
	return {
		restrict: 'E',
		templateUrl: '/partials/admin/tables/seatshow.html',
		replace: true,
		scope: {
			player: '=',
			privateData: '=',
			dealerSeat: '='
		},
		link: function(scope, element, attributes) {
			scope.seatIndex = parseInt(attributes.seatIndex);
			scope.cellNumber = parseInt(attributes.cellNumber);

			scope.getCardClass = function (seat, card) {
				if (typeof scope.privateData !== 'undefined' && scope.privateData && scope.privateData.cards && scope.privateData.cards[card]) {
					return 'card-' + scope.privateData.cards[card];
				}
				else {
					return 'card-back';
				}
			};
			scope.seatOccupied = function (seat) {
				return typeof scope.player !== 'undefined' && scope.player && scope.player.name;
			};
		}
	};
}]);