(function (app) {

    app.controller('TripAddController',
        ['$scope', '$routeParams', '$location', '$filter', 'tripService',
        function ($scope, $routeParams, $location, $filter, tripService) {

            $scope.userId = $routeParams.userId;
            $scope.id = $routeParams.id;

            $scope.action = ($scope.id == "new") ? "create" : "edit";

            $scope.title = ($scope.action == "create")
                         ? "Create trip"
                         : "Edit trip";

            $scope.prepareDate = function (date) {
                return $filter('date')(date, 'yyyy-MM-dd');
            };

            $scope.trip = { userId: $scope.userId };
            if ($scope.action == "edit") {
                tripService.getById($scope.userId, $scope.id).then(
                    function (response) {
                        $scope.trip = response.data;
                        $scope.trip.startDate = $scope.prepareDate($scope.trip.startDate);
                        $scope.trip.endDate = $scope.prepareDate($scope.trip.endDate);
                    },
                    function (response) {
                        showValidationErrors($scope, response, "Failed to get trip");
                    });
            }

            $scope.save = function (trip) {
                trip.userId = $scope.userId;

                if ($scope.action == "create") {
                    $scope.create(trip);
                } else {
                    $scope.update(trip);
                }
            };

            $scope.getTripsUrl = function (userId) {
                return '/users/' + userId + '/trips/';
            };

            $scope.create = function (trip) {
                tripService.create($scope.userId, trip).then(
                    function (response) {
                        toastr.success('Trip successfully created');
                        $location.path($scope.getTripsUrl($scope.userId));
                    },
                    function (response) {
                        showValidationErrors($scope, response, "Failed to create trip");
                    });
            };

            $scope.update = function (trip) {
                tripService.update($scope.userId, trip).then(
                    function (response) {
                        toastr.success('Trip successfully saved');
                        $location.path($scope.getTripsUrl($scope.userId));
                    },
                    function (response) {
                        showValidationErrors($scope, response, "Failed to update trip");
                    });
            };
        }]);

}(angular.module("TravelPlannerApp")));
