(function (app) {

    app.controller('LoginController',
        ['$scope', '$routeParams', '$http', '$location', '$route', 'Auth', 'constants',
        function ($scope, $routeParams, $http, $location, $route, auth, constants) {

            $scope.user = {};

            $scope.auth = auth;
            $scope.authUser = { name: '', email: '' };
            $scope.authUserTypeName = '';

            $scope.$watch('auth.isLoggedIn()', function () {
                if (auth.isLoggedIn()) {
                    $scope.authUser = auth.currentUser();
                    $scope.authUserTypeName = constants.UserTypes[$scope.authUser.type];
                } else {
                    $scope.authUser = { name: '', email: '' };
                    $scope.authUserTypeName = '';
                }
            });

            $scope.login = function (user) {
                var credentials = {
                    grant_type: 'password',
                    username: user.email,
                    password: user.password
                };

                var options = { url: '/api/token' };

                $http({
                    method: 'POST',
                    url: '/api/token',
                    data: $.param(credentials),
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                }).then(function (response) {
                    auth.setToken(response.data.access_token);

                    $http.get('/api/profile').then(
                        function (response) {
                            auth.setUser(response.data);
                            $location.path('/');
                        }, function (response) {
                            showLoginError($scope, response, "Failed to login");
                        });
                }, function (response) {
                    showLoginError($scope, response, "Failed to login");
                });
            };

            $scope.logout = function () {
                auth.logout();
                $route.reload();
            };

        }]);

}(angular.module("TravelPlannerApp")));
