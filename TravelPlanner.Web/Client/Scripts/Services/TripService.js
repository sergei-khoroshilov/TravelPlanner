(function (app) {
    var tripService = function ($http, $httpParamSerializer) {
        var urlTemplate = "/api/users/{userId}/trips/";

        var getUserUrl = function (userId) {
            return urlTemplate.replace("{userId}", userId);
        };

        var getTripUrl = function (userId, tripId) {
            return getUserUrl(userId) + tripId;
        };

        var getAll = function (userId) {
            var url = getUserUrl(userId);
            return $http.get(url);
        };

        var getById = function (userId, tripId) {
            var url = getTripUrl(userId, tripId);
            return $http.get(url);
        };

        var filter = function (userId, filter) {
            var qr = $httpParamSerializer(filter);
            var url = getUserUrl(userId);
            if (qr != '') {
                url = url + '?' + qr;
            }

            return $http.get(url);
        }

        var create = function (userId, trip) {
            var url = getUserUrl(userId);
            return $http.post(url, trip);
        };

        var update = function (userId, trip) {
            var url = getTripUrl(userId, trip.id);
            return $http.put(url, trip);
        }

        var remove = function(userId, tripId) {
            var url = getTripUrl(userId, tripId);
            return $http.delete(url);
        }

        return {
            getAll: getAll,
            getById: getById,
            filter: filter,
            create: create,
            update: update,
            remove: remove
        };
    };

    app.factory("tripService", tripService);
}(angular.module("TravelPlannerApp")));
