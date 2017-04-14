(function (app) {

    app.run(['$rootScope', '$location', 'Auth',
    function ($rootScope, $location, auth) {
        $rootScope.$on('$routeChangeStart', function (event, url) {

            if (url.allowAnonymous) {
                return;
            }

            if (!auth.isLoggedIn()) {
                event.preventDefault();
                $location.path('/login');
                return;
            }

            if (Array.isArray(url.roles)) {
                var hasRole = false;

                for (var index in url.roles) {
                    var role = url.roles[index];
                    if (auth.hasRole(role)) {
                        hasRole = true;
                        break;
                    }
                }

                if (!hasRole) {
                    $location.path('/');
                    return;
                }
            }

            if (Array.isArray(url.conditionalRedirectTo)) {
                for (var index in url.conditionalRedirectTo) {
                    var condRedirect = url.conditionalRedirectTo[index];

                    for (var roleIndex in condRedirect.forRoles) {
                        var role = condRedirect.forRoles[roleIndex];

                        if (auth.hasRole(role)) {
                            var redirectTo = condRedirect.redirectTo.replace(':userId', auth.currentUser().id);
                            $location.path(redirectTo);
                            return;
                        }
                    }
                }
            }
        });
    }]);

}(angular.module("TravelPlannerApp")));
