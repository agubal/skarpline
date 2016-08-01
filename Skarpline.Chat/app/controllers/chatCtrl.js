'use strict';
app.controller('chatCtrl', ['$scope', 'authService', '$location', '$interval', 'host' , 'apiClientService',
function ($scope, authService, $location, $interval, host, apiClientService) {

    if (!authService.authentication || !authService.authentication.isAuth) {
        $location.path('/login');
    }
    $scope.name = authService.authentication.userName;
    $scope.message = '';
    $scope.typingUsers = [];

    var connection = $.hubConnection(host + "signalr", { useDefaultPath: false });
    $scope.chatHub = connection.createHubProxy('chatHub');;
    $scope.chatHub.on('addMessage', function (message) {
        $scope.messages.splice(0, 0, message);
        $scope.$apply();
    });
    $scope.chatHub.on('startTypig', function (name, text) {
        if (name === $scope.name) {
            return;
        }

        for (var i = 0; i < $scope.typingUsers.length; i++) {
            if ($scope.typingUsers[i].sender === name) {
                $scope.typingUsers[i].text = text;
                $scope.$apply();
                return;
            }
        }
        $scope.typingUsers.push({
            sender: name,
            text: text
        });
        $scope.$apply();
    });
    $scope.chatHub.on('stopTypig', function (name) {
        var index = getTypingMessagePositionByName(name);
        console.log(index);
        if (index === -1) {
            return;
        }
        $scope.typingUsers.splice(index);
    });
    connection.start();

    apiClientService.get("messages?$top=20&$orderby=Date desc").then(function (result) {
        console.log(result);
        $scope.messages = result.data.Items;
    });


    $scope.sendMessage = function () {
        $scope.chatHub.invoke("SendMessage", $scope.message, authService.authentication.userId, authService.authentication.userName);
        $scope.message = '';
    };

    var lastTypeinDate = Date.now();
    var isTyping = false;

    $scope.iAmTyping = function () {
        lastTypeinDate = Date.now();
        isTyping = true;
        $scope.chatHub.invoke("StartTyping", authService.authentication.userName, $scope.message);
    }

    $interval(function () {
        if (isTyping && (Date.now() - lastTypeinDate) > 1000) {
            $scope.chatHub.invoke("StopTyping", authService.authentication.userName);
            isTyping = false;
        }

    },
    500);

    $scope.isMyMessage = function(userId) {
        return userId === authService.authentication.userId;
    }

    $scope.exitChat = function () {
        connection.stop();
        authService.logOut();
    }

    function getTypingMessagePositionByName(name) {

        for (var i = 0; i < $scope.typingUsers.length; i++) {
            if ($scope.typingUsers[i].sender === name) {
                return i;
            }
        }
        return -1;
    }
}]);