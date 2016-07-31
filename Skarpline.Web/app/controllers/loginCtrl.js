'use strict';
app.controller('loginCtrl', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.loginData = {};
    $scope.message = "";

    $scope.login = function () {
        authService.login($scope.loginData).then(function () {
            $location.path('/chat');
        },
         function (err) {
             $scope.message = err.error_description;
         });
    };

    $scope.openRegisterPage = function () {
        $location.path('/signup');
    }

}]);