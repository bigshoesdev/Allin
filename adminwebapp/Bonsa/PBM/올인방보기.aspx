<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="올인방보기.aspx.cs" Inherits="게임관리_올인방보기" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" runat="Server">
    <style>
        a {
            font-size: 9pt;
        }

        span {
            font-size: 9pt;
        }
    </style>
    
    <link href="../../css/allin/allin.css"  rel="stylesheet"  type="text/css" />
    <script src="../../scripts/socket.io.js" type="text/javascript"></script>
    <script src="../../scripts/angular.min.js" type="text/javascript"></script>
    <script src="../../scripts/angular-ui-router.js" type="text/javascript"></script>

    <script type="text/javascript">
        var socket = window.socket;
        
        if(!socket)
        {
            socket = io.connect("<%= ConfigurationManager.ConnectionStrings["AllinSocketString"].ConnectionString %>");
            window.socket = socket;
        }

        var tableID;

        var app = angular.module('app', ['ui.router'])
            .config(function ($stateProvider, $urlRouterProvider, $locationProvider) {

            });


        app.run(function ($rootScope, alarmServices) {

            $rootScope.alarmDisplay = function () {
            }

        });

        app.controller('TableCtrl', ['$scope', '$rootScope', '$stateParams', '$timeout',
            function ($scope, $rootScope, $stateParams, $timeout) {
                $scope.table = {};
                $scope.tableSeats = [];
                $scope.actionState = '';
                $scope.table.dealerSeat = null;
                $scope.savedAlarmLen = $rootScope.alarmLength;
                var tableTimeout;

                // Existing listeners should be removed
                socket.removeAllListeners();

                $scope.startTimeout = function () {
                    console.log('@@@@@@@@@@ getTablefulldata');
                    tableID = $(".roomID").val();
                    socket.emit('getTableFullData', $(".roomID").val(), function (response) {
                        if (response.success) {
                            console.log('getTableFullData success : ');
                            $scope.tableSeats = response.seats;
                            $scope.table = response.table;
                            $scope.table.board = response.board;
                        }
                    });
                    tableTimeout = $timeout($scope.startTimeout, 3000);
                };

                $scope.startTimeout();

                $scope.potText = function () {
                    if (typeof $scope.table.pot !== 'undefined' && $scope.table.pot[0].amount) {
                        var potText = 'Pot: ' + $scope.table.pot[0].amount;

                        var potCount = $scope.table.pot.length;
                        if (potCount > 1) {
                            for (var i = 1; i < potCount; i++) {
                                potText += ' - Sidepot: ' + $scope.table.pot[i].amount;
                            }
                        }
                        return potText;
                    }
                };

                $scope.getCardClass = function (seat, card) {
                    if (typeof $scope.tableSeats !== 'undefined' && typeof $scope.tableSeats[seat] !== 'undefined' && $scope.tableSeats[seat] && typeof $scope.tableSeats[seat].cards !== 'undefined' && typeof $scope.tableSeats[seat].cards[card] !== 'undefined') {
                        return 'card-' + $scope.tableSeats[seat].cards[card];
                    } else {
                        return 'card-back';
                    }
                };
                $scope.seatOccupied = function (seat) {
                    return $scope.table.seats !== 'undefined' && typeof $scope.table.seats[seat] !== 'undefined' && $scope.table.seats[seat] && $scope.table.seats[seat].name;
                };

                // Leaving the socket room
                $scope.leaveRoom = function () {

                };

                $scope.alarmStartTimeout = function () {
                    $rootScope.alarmDisplay();
                    $scope.alarmCheckTimeout = $timeout($scope.alarmStartTimeout, 2000);
                };
                $scope.alarmStartTimeout();

                $scope.$on("$destroy", function (event) {
                    $timeout.cancel(tableTimeout);
                    $timeout.cancel($scope.alarmCheckTimeout);
                });
            }
        ]);

        app.directive('seatshow', [function () {
            return {
                restrict: 'E',
                templateUrl: '/scripts/allinroom/partial/seatshow.html',
                replace: true,
                scope: {
                    player: '=',
                    privateData: '=',
                    dealerSeat: '='
                },
                link: function (scope, element, attributes) {
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

        app.factory('alarmServices', function ($rootScope, $http) {
            return {
                getAlarmList: function (token) {
                    return $http({
                        url: '/alarms/',
                        method: 'GET',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + token
                        }
                    });
                },

                removeAlarm: function (token, id) {
                    return $http({
                        url: '/alarms/' + id,
                        method: 'DELETE',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + token
                        }
                    });
                }
            }
        });

    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap></td>
            <td class="clsSysTitle">
                <h5>올인
                    <asp:Label ID="lblChannel" runat="server"></asp:Label>&nbsp;실시간게임방정보</h5>
            </td>
        </tr>
    </table>

    <input id="roomID" runat="server" type="hidden" class="roomID" />

    <div ng-app="app">
        <div ng-controller="TableCtrl">
            <div id="content-wrapper">
                <div class="container-fluid" style="overflow-x: auto;">
                    <div id="pokertable-wrap">
                        <div id="pokertable">
                            <div id="felt"></div>
                        </div>
                        <div class="seatrow">
                            <div class="cell">
                                <seatshow player="table.seats[0]" private-data="tableSeats[0]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
                                    seat-index="0" cell-number="0" class="top left"></seatshow>
                            </div>
                            <div class="cell">
                                <seatshow player="table.seats[1]" private-data="tableSeats[1]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
                                    seat-index="1" cell-number="1"></seatshow>
                            </div>
                            <div class="cell">
                                <seatshow player="table.seats[2]" private-data="tableSeats[2]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
                                    seat-index="2" cell-number="2"></seatshow>
                            </div>
                            <div class="cell">
                                <seatshow player="table.seats[3]" private-data="tableSeats[3]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
                                    seat-index="3" cell-number="3" class="top right"></seatshow>
                            </div>
                        </div>
                        <div class="seatrow">
                            <div class="cell">
                                <!-- <seatshow player="table.seats[9]" private-data="tableSeats[9]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
						        seat-index="9" cell-number="9"></seatshow> -->
                            </div>
                            <div class="double-cell">
                                <div id="pot-wrap">
                                    <span id="pot" ng-show="table.pot[0].amount">{{potText()}}</span>
                                </div>
                                <div id="board-wrap">
                                    <div class="pokercard-container">
                                        <div class="pokercard card-{{table.board[0]}}" ng-show="table.board[0]"></div>
                                    </div>
                                    <div class="pokercard-container">
                                        <div class="pokercard card-{{table.board[1]}}" ng-show="table.board[1]"></div>
                                    </div>
                                    <div class="pokercard-container">
                                        <div class="pokercard card-{{table.board[2]}}" ng-show="table.board[2]"></div>
                                    </div>
                                    <div class="pokercard-container">
                                        <div class="pokercard card-{{table.board[3]}}" ng-show="table.board[3]"></div>
                                    </div>
                                    <div class="pokercard-container">
                                        <div class="pokercard card-{{table.board[4]}}" ng-show="table.board[4]"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="cell">
                                <seatshow player="table.seats[4]" private-data="tableSeats[4]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
                                    seat-index="4" cell-number="4"></seatshow>
                            </div>
                        </div>
                        <div class="seatrow">
                            <div class="cell">
                                <seatshow player="table.seats[8]" private-data="tableSeats[8]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
                                    seat-index="8" cell-number="8" class="bottom left"></seatshow>
                            </div>
                            <div class="cell">
                                <seatshow player="table.seats[7]" private-data="tableSeats[7]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
                                    seat-index="7" cell-number="7"></seatshow>
                            </div>
                            <div class="cell">
                                <seatshow player="table.seats[6]" private-data="tableSeats[6]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
                                    seat-index="6" cell-number="6"></seatshow>
                            </div>
                            <div class="cell">
                                <seatshow player="table.seats[5]" private-data="tableSeats[5]" active-seat="table.activeSeat" dealer-seat="table.dealerSeat"
                                    seat-index="5" cell-number="5" class="bottom right"></seatshow>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /.container-fluid -->
            </div>
        </div>
    </div>

</asp:Content>

