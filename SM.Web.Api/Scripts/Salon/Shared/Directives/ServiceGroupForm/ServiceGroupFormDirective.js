(function () {
    'use strict';

    angular.module('SalonApp')
        .directive('serviceGroupForm', function () {
            return {
                restrict: 'E',
                scope: {
                    serviceGroup: '=',
                    onSave: '&'
                },
                templateUrl: 'Scripts/Salon/Shared/Directives/ServiceGroupForm/ServiceGroupForm.view.html',
                controller: ['$scope', 'ServiceGroupService', 'toastr', function ($scope, ServiceGroupService, toastr) {

                    if (!$scope.serviceGroup) {
                        $scope.serviceGroup = {};
                    }

                    $scope.saveServiceGroup = function () {
                        if (!$scope.serviceGroup.ServiceGroupId) {
                            ServiceGroupService.addServiceGroup($scope.serviceGroup)
                                .then(function () {
                                    toastr.success("Thêm nhóm dịch vụ thành công");

                                    if (angular.isFunction($scope.onSave)) {
                                        $scope.onSave();
                                    }
                                });
                        }
                        else {
                            ServiceGroupService.updateServiceGroup($scope.serviceGroup)
                                .then(function () {
                                    toastr.success("Cập nhật nhóm dịch vụ thành công");

                                    if (angular.isFunction($scope.onSave)) {
                                        $scope.onSave();
                                    }
                                });
                        }
                    };
                }]
            }

        });
})();