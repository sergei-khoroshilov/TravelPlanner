(function (app) {

    var UserAddController = function ($scope, $location, $routeParams, userService, constants, auth) {
        $scope.user = { type: 1 };

        $scope.auth = auth;

        $scope.id = $routeParams.id;

        if (typeof $scope.id === 'undefined') {
            $scope.id = 'new';
        }

        $scope.action = ($scope.id == "new") ? "create" : "edit";

        $scope.title = ($scope.action == "create")
                     ? "Create user"
                     : "Edit user";

        $scope.userTypes = Object.keys(constants.UserTypes).map(function (type) {
            return { id: type, name: constants.UserTypes[type] };
        });

        $scope.updateUserType = function () {
            $scope.userType = $scope.userTypes[0];
            for (var i in $scope.userTypes) {
                var type = $scope.userTypes[i];
                if (type.id == $scope.user.type) {
                    $scope.userType = type;
                    break;
                }
            }
        };

        $scope.updateUserType();

        if ($scope.action == "edit") {
            userService.getById($scope.id).then(
            function (response) {
                $scope.user = response.data;
                $scope.updateUserType();
            });
        }

        $scope.save = function (user) {
            if ($scope.action == "create") {
                $scope.create(user);
            } else {
                $scope.update(user);
            }
        };

        $scope.create = function (user) {
            user.type = $scope.userType.id;

            userService.create(user).then(
                function (response) {
                    toastr.success('User successfully created');
                    $location.path('#/users');
                },
                function (response) {
                    showValidationErrors($scope, response, "Failed to create user");
                });
        };

        $scope.update = function (user) {
            user.type = $scope.userType.id;

            userService.update(user).then(
                function (response) {
                    toastr.success('User successfully updated');
                    $location.path('/');
                },
                function (response) {
                    showValidationErrors($scope, response, "Failed to update user");
                });
        };
    };
    UserAddController.$inject = ["$scope", "$location", "$routeParams", "userService", "constants", 'Auth'];

    app.controller("UserAddController", UserAddController);

}(angular.module("TravelPlannerApp")));
