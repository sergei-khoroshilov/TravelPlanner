(function () {
    var app = angular.module("TravelPlannerApp", ["ngRoute", "ngMessages", "ngStorage"]);

    var config = function ($routeProvider) {
        $routeProvider
            .when("/", {
                conditionalRedirectTo: [
                    { forRoles: ['Admin', 'Manager'], redirectTo: "/users" },
                    { forRoles: ['User'], redirectTo: "/users/:userId/trips" }
                ]
            })
            .when("/error", { templateUrl: "/client/views/error.html", allowAnonymous: true })
            .when("/login", { templateUrl: "/client/views/users/login.html", allowAnonymous: true })
            .when("/users", { templateUrl: "/client/views/users/list.html", roles: ['Admin', 'Manager'] })
            .when("/users/new", { templateUrl: "/client/views/users/details.html", allowAnonymous: true })
            .when("/users/:id", { templateUrl: "/client/views/users/details.html" })
            .when("/users/:userId/trips", { templateUrl: "/client/views/trips/list.html", roles: ['User', 'Admin'] })
            .when("/users/:userId/trips/plan", { templateUrl: "/client/views/trips/plan.html", roles: ['User', 'Admin'] })
            .when("/users/:userId/trips/:id", { templateUrl: "/client/views/trips/details.html", roles: ['User', 'Admin'] })
            .otherwise({ redirectTo: "/" });
    };

    app.config(config);
    app.constant("userApiUrl", "/api/users/");
}());
