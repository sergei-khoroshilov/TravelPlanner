(function (app) {
    var userService = function ($http, userApiUrl) {

        var getAll = function () {
            return $http.get(userApiUrl);
        };

        var getById = function (id) {
            return $http.get(userApiUrl + id);
        };

        var create = function (user) {
            return $http.post(userApiUrl, user);
        };

        var update = function (user) {
            return $http.put(userApiUrl + user.id, user);
        };

        return {
            getAll: getAll,
            getById: getById,
            create: create,
            update: update
        };
    };

    app.factory("userService", userService);
}(angular.module("TravelPlannerApp")));
