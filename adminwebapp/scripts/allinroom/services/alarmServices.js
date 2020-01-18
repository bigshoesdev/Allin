app.factory('alarmServices', function($rootScope, $http){
	return {
		getAlarmList: function(token){
			return $http({
				url: '/alarms/',
				method: 'GET',
				headers: {
					'Content-Type': 'application/json',
					'Authorization': 'Bearer ' + token
				}
			});
		},

		removeAlarm: function(token, id) {
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