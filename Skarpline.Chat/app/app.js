var app = angular.module('chat', ['ngRoute', 'angular-loading-bar', 'LocalStorageModule'])
.constant("baseUrl", "http://localhost:9995/api/")
.constant("host", "http://localhost:9995/");

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeCtrl",
        templateUrl: "/app/views/home.html"
    });
    $routeProvider.when("/chat", {
        controller: "chatCtrl",
        templateUrl: "/app/views/chat.html"
    });

    $routeProvider.when("/login", {
        controller: "loginCtrl",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupCtrl",
        templateUrl: "/app/views/signup.html"
    });

    $routeProvider.otherwise({ redirectTo: "/chat" });
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);