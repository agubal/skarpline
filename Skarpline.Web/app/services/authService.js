'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', '$location',
function ($http, $q, localStorageService, $location) {

    var serviceBase = 'http://localhost:9995/';
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: ""
        };

        var _saveRegistration = function (registration) {
            _logOut();
            return $http.post(baseUrl + 'api/users/register', registration).then(function (response) {
                return response;
            });
        };

        var _login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
            var deferred = $q.defer();

            $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                localStorageService.set('authorizationData', {
                    token: response.access_token,
                    userName: loginData.userName,
                    userId: response.userId
                });

                _authentication.isAuth = true;
                _authentication.userName = loginData.userName;
                _authentication.userId = response.userId;
                deferred.resolve(response);

            }).error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        var _logOut = function () {

            localStorageService.remove('authorizationData');
            _authentication.isAuth = false;
            _authentication.userName = "";
            $location.path('/login');
        };

        var _fillAuthData = function () {
            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
            }
        }

        authServiceFactory.saveRegistration = _saveRegistration;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;

        return authServiceFactory;
    }]);