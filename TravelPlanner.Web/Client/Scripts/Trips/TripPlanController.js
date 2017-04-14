(function (app) {

    app.controller('TripPlanController',
        ['$scope', '$route', '$routeParams', '$filter', 'tripService',
        function ($scope, $route, $routeParams, $filter, tripService) {

            $scope.userId = $routeParams.userId;

            $scope.getNextMonthFilter = function (now) {
                var filter = {};

                var nextMonth = now.add(1, 'month');
                $scope.nextMonth = nextMonth.format('MMMM YYYY');

                var startMonth = nextMonth.clone();
                startMonth.startOf('month');

                var endMonth = nextMonth.clone();
                endMonth.endOf('month');

                // end date should be >= first day of next month

                filter.toStart = startMonth.format('YYYY-MM-DD');

                // start date should be <= last day of next month

                filter.fromEnd = endMonth.format('YYYY-MM-DD');

                return filter;
            };

            var now = moment();
            var filter = $scope.getNextMonthFilter(now);

            tripService.filter($scope.userId, filter).then(
                function (response) {
                    var trips = response.data;

                    trips.sort(function (a, b) {
                        return a.startDate.localeCompare(b.startDate);
                    });

                    $scope.trips = trips;
                });
        }]);

}(angular.module("TravelPlannerApp")));
