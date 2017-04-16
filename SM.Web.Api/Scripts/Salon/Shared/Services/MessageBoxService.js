(function () {
    'use strict';

    angular.module('SalonApp')
        .factory('MsgBox', CustomerService);

    /* @ngInject */
    function CustomerService($q, $http, $kWindow) {
        var service = {};

        service.confirm = confirm;

        return service;

        function confirm(title, message) {
            var defer = $q.defer();

            var wdInstance = $kWindow.open({
                options: {
                    modal: true,
                    title: title,
                    resizable: true,
                    visible: false,
                    width: '500px',
                    height: '150px',
                    top: '10px'
                },
                template: '<h3 class="dialog-message">' + message + '</h3>\
                          <article class="clb mb15 tblBox dialog-button">\
                                <kendo-button class="k-button k-primary" ng-click="ok()"><i class="fa fa-check"></i> Đồng ý</kendo-button>\
                                <kendo-button ng-click="cancel()" class="k-button"><i class="fa fa-remove"></i> Không đồng ý</kendo-button>\
                          </article>',
                controller: ['$scope', '$windowInstance', function ($scope, $windowInstance) {
                    $scope.ok = function () {
                        $windowInstance.close(true);
                    }

                    $scope.cancel = function () {
                        $windowInstance.close(false);
                    }
                }]
            });

            wdInstance.result.then(function (resp) {
                defer.resolve(resp);
            })

            return defer.promise;
        }
    }

})();