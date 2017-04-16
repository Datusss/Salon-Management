(function () {
    'use strict';

    angular.module('SalonApp')
        .directive('serviceForm', function () {
            return {
                restrict: 'E',
                scope: {
                    service: '=',
                    onSave: '&',
                    onRemove: '&'
                },
                templateUrl: 'Scripts/Salon/Shared/Directives/ServiceForm/ServiceForm.view.html',
                controller: ['$scope', '$rootScope', 'ServiceService', '$kWindow', 'toastr', 'MsgBox', function ($scope, $rootScope, ServiceService, $kWindow, toastr, MsgBox) {

                    init();

                    function init() {
                        if (!$scope.service) {
                            $scope.service = {};
                        }
                        
                        $scope.serviceGroupDropDownOptions = {
                            dataSource: new kendo.data.DataSource({
                                type: "odata",
                                transport: {
                                    read: {
                                        url: "/api/servicegroup",
                                        dataType: 'json',
                                        beforeSend: function (xhr) {
                                            xhr.setRequestHeader('Authorization', 'Basic ' + $rootScope.globals.currentUser.authdata)
                                        }
                                    }
                                },
                                schema: {
                                    data: "Data",
                                    total: "Total",
                                }
                            }),
                            dataTextField: "Name",
                            dataValueField: "ServiceGroupId",
                        }
                    }

                    $scope.saveServiceGroup = function () {
                        if (!$scope.service.ServiceId) {
                            ServiceService.addService($scope.service)
                                .then(function () {
                                    toastr.success("Thêm dịch vụ thành công");

                                    if (angular.isFunction($scope.onSave)) {
                                        $scope.onSave();
                                    }
                                });
                        }
                        else {
                            ServiceService.updateService($scope.service)
                                .then(function () {
                                    toastr.success("Cập nhật thông tin dịch vụ thành công");

                                    if (angular.isFunction($scope.onSave)) {
                                        $scope.onSave();
                                    }
                                });
                        }
                    };

                    $scope.addNewServiceGroup = function () {
                        var windowInstance = $kWindow.open({
                            options: {
                                modal: true,
                                title: 'Thêm nhóm dịch vụ',
                                resizable: true,
                                visible: false,
                                width: '550px',
                                height: '205px'
                            },
                            template: '<service-group-form on-save="onSave()"></service-group-form>',
                            controller: ['$scope', '$windowInstance', function ($scope, $windowInstance) {
                                $scope.onSave = function () {
                                    $windowInstance.close(true);
                                }
                            }]
                        });

                        windowInstance.result.then(function (resp) {
                            if (resp) {
                                $scope.ddlServiceGroup.dataSource.read();
                            }
                        })
                    };

                    $scope.removeService = function () {
                        MsgBox.confirm('Xóa dịch vụ', 'Bạn có chắc chắn muốn xóa dịch vụ <b>' + $scope.service.Name + '</b> này không?')
                            .then(function (resp) {
                                if (resp) {
                                    ServiceService.deleteService($scope.service.ServiceId)
                                        .then(function () {
                                            toastr.success("Xóa dịch vụ thành công");

                                            if (angular.isFunction($scope.onRemove)) {
                                                $scope.onRemove();
                                            }
                                        },
                                        function (e) {
                                            toastr.error(e.Message, "Xóa dịch vụ thất bại");
                                        });
                                }
                            })
                    }
                }]
            }

        });
})();