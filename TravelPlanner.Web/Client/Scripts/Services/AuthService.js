(function (app) {
    var authService = function ($rootScope, $sessionStorage, constants) {

        var auth = {};

        auth.currentUser = function () {
            return $sessionStorage.authUser;
        };

        auth.token = function () {
            return $sessionStorage.token;
        }

        auth.isLoggedIn = function () {
            return $sessionStorage.authUser != null;
        };

        auth.init = function () {
            if (auth.isLoggedIn()) {
                $rootScope.authUser = auth.currentUser();
            }
        };

        auth.setToken = function (token) {
            $sessionStorage.token = token;
        };

        auth.setUser = function (user) {
            $sessionStorage.authUser = user;
            $rootScope.authUser = auth.currentUser();
        }

        auth.hasRole = function (userType) {
            if (!auth.isLoggedIn()) {
                return false;
            }

            var type = auth.currentUser().type;
            return constants.UserTypes[type] == userType;
        }

        auth.logout = function () {
            delete $sessionStorage.token;
            delete $sessionStorage.authUser;
            delete $rootScope.authUser;
        };
        /*
        auth.checkPermissionForView = function (view) {
            if (!view.requiresAuthentication) {
                return true;
            }

            return userHasPermissionForView(view);
        };

        var userHasPermissionForView = function (view) {
            if (!auth.isLoggedIn()) {
                return false;
            }

            if (!view.permissions || !view.permissions.length) {
                return true;
            }

            return auth.userHasPermission(view.permissions);
        };


        auth.userHasPermission = function (permissions) {
            if (!auth.isLoggedIn()) {
                return false;
            }

            var found = false;
            angular.forEach(permissions, function (permission, index) {
                if ($sessionStorage.authUser.user_permissions.indexOf(permission) >= 0) {
                    found = true;
                    return;
                }
            });

            return found;
        };
        */

        return auth;
    };

    app.factory("Auth", authService);
}(angular.module("TravelPlannerApp")));
