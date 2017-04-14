(function (app) {
    var UserListController = function ($scope, userService, constants, auth) {

        $scope.auth = auth;

        userService.getAll().then(
            function (response) {
                $scope.users = response.data;
            });

        $scope.getUserTypeName = function (userType) {
            return constants.UserTypes[userType];
        };
    };
    UserListController.$inject = ["$scope", "userService", "constants", "Auth"];
    
    app.controller("UserListController", UserListController);

}(angular.module("TravelPlannerApp")));