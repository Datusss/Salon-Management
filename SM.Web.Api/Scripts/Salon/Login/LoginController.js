(function () {
    'use strict';

    angular.module('SalonApp')
        .controller('LoginController', LoginController);

    /* @ngInject */
    function LoginController($scope, $rootScope, $location, AuthenticationService) {

        init();

        function init() {
            AuthenticationService.ClearCredentials();
        }

        $scope.Login = function () {
            AuthenticationService.Login($scope.Username, $scope.Passowrd, function (resp) {
                if (angular.isObject(resp)) {
                    AuthenticationService.SetCredentials(resp, $scope.Passowrd);

                    $location.path("/");
                }
                else {
                    alert('Đăng nhập thất bại');
                }
            });
        }
    }

})();