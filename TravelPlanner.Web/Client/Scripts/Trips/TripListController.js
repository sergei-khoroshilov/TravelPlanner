(function (app) {

    app.controller('TripListController',
        ['$scope', '$route', '$routeParams', '$filter', 'tripService',
        function ($scope, $route, $routeParams, $filter, tripService) {

            $scope.userId = $routeParams.userId;
            $scope.filter = {};

            tripService.getAll($scope.userId).then(
                function (response) {
                    $scope.trips = response.data;
                });

            $scope.calcDaysToStart = function (startDate) {
                var startDateString = $filter('date')(startDate, 'yyyy-MM-dd');
                var startDateMoment = moment(startDateString, moment.ISO_8601);
                var now = moment();

                // Remove time
                now = moment({
                    year: now.year(),
                    month: now.month(),
                    day: now.date()
                });

                var days = startDateMoment.diff(now, 'days');

                if (days < 0) {
                    days = 0;
                }

                return days;
            };

            $scope.formatDaysToStart = function (startDate) {
                var days = $scope.calcDaysToStart(startDate);
                return days > 0 ? days : '';
            };

            $scope.delete = function (trip) {
                if (!confirm('Do you really want to delete this trip?')) {
                    return;
                }

                tripService.remove($scope.userId, trip.id).then(
                    function (response) {
                        toastr.success('Trip successfully deleted');
                        $route.reload();
                    },
                    function (response) {
                        toastr.error('Failed to delete trip');
                    });
            };

            $scope.applyFilter = function (filter) {
                tripService.filter($scope.userId, filter).then(
                    function (response) {
                        $scope.trips = response.data;
                    },
                    function (response) {
                        toastr.error('Failed to apply filter');
                    });
            };

            $scope.clearFilter = function () {
                $scope.filter = {};
                $scope.applyFilter($scope.filter);
            };
        }]);

}(angular.module("TravelPlannerApp")));
