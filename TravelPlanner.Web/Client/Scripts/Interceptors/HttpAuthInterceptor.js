(function (app) {

    app.factory('httpAuthInterceptor', ['$q', '$location', '$log', 'Auth',
    function ($q, $location, $log, auth) {
        return {
            request: function (config) {
                var token = auth.token();

                if (typeof token != 'undefined') {
                    config.headers['Authorization'] = 'Bearer ' + token;
                }

                return config;
            },

            responseError: function (rejection) {
                $log.error(rejection);

                if (rejection.status != 400) {

                    if (rejection.status === 401 || rejection.status === 403) {
                        auth.logout();
                        $location.path('/error');
                        return $q(function () { return null; });
                    } else {
                        $location.path('/error');
                        return $q(function () { return null; });
                    }
                }

                return $q.reject(rejection);
            }
        };
    }]);

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('httpAuthInterceptor');
    });

}(angular.module("TravelPlannerApp")));
