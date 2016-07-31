'use strict';
app.factory('apiClientService', [
    '$http', 'baseUrl', 'localStorageService', function ($http, baseUrl, localStorageService) {

        $http.defaults.useXDomain = true;
        var apiClientFactory = {};

        var addAuth = function () {
            console.log("GetAuth:");
            var authData = localStorageService.get('authorizationData');
            if (authData) {
                $http.defaults.headers.common.Authorization = 'Bearer ' + authData.token;
            }
        }

        var get = function (url) {
            addAuth();
            return $http.get(baseUrl + url);

        }

        var post = function (url, body) {
            addAuth();
            return $http.post(baseUrl + url, body);
        }

        var del = function (url) {
            addAuth();
            return $http.delete(baseUrl + url);
        }

        apiClientFactory.get = get;
        apiClientFactory.post = post;
        apiClientFactory.delete = del;
        return apiClientFactory;
    }
]);