(function () {
    'use strict';

    angular.module('SalonApp')
        .directive('staffForm', function () {
            return {
                restrict: 'E',
                scope: {
                    staff: '=',
                    onSave: '&',
                    onRemove: '&'
                },
                templateUrl: 'Scripts/Salon/Shared/Directives/StaffForm/StaffForm.view.html',
                controller: ['$scope', '$rootScope', 'StaffService', 'toastr', 'MsgBox', function ($scope, $rootScope, StaffService, toastr, MsgBox) {

                    init();

                    function init() {
                        if (!$scope.staff) {
                            $scope.staff = {};
                        }

                        $scope.ddlPositionOptions = {
                            optionLabel: 'Lựa chọn vị trí',
                            dataTextField: 'Name',
                            dataValueField: 'PositionId',
                            dataSource: new kendo.data.DataSource({
                                type: "odata",
                                transport: {
                                    read: {
                                        url: "/api/staff-position",
                                        dataType: "json",
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
                        };
                    }

                    $scope.saveServiceGroup = function () {
                        if (!$scope.staff.StaffId) {
                            StaffService.addStaff($scope.staff)
                                .then(function () {
                                    toastr.success("Thêm khách hàng thành công");

                                    if (angular.isFunction($scope.onSave)) {
                                        $scope.onSave();
                                    }
                                });
                        }
                        else {
                            StaffService.updateStaff($scope.staff)
                                .then(function () {
                                    toastr.success("Cập nhật khách hàng thành công");

                                    if (angular.isFunction($scope.onSave)) {
                                        $scope.onSave();
                                    }
                                });
                        }
                    };

                    $scope.removeStaff = function () {
                        MsgBox.confirm('Xóa nhân viên', 'Bạn có chắc chắn muốn xóa nhân viên <b>' + $scope.staff.Name + '</b> này không?')
                            .then(function (resp) {
                                if (resp) {
                                    StaffService.deleteStaff($scope.staff.StaffId)
                                        .then(function () {
                                            toastr.success("Xóa nhân viên thành công");

                                            if (angular.isFunction($scope.onRemove)) {
                                                $scope.onRemove();
                                            }
                                        },
                                        function (e) {
                                            toastr.error(e.Message, "Xóa nhân viên thất bại");
                                        });
                                }
                            })
                    }
                }]
            }

        });
})();