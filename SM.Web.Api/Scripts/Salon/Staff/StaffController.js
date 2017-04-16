(function () {
    'use strict';

    angular.module('SalonApp')
        .controller('StaffController', StaffController);

    /* @ngInject */
    function StaffController($scope, $rootScope, $kWindow) {

        $scope.filter = {};
        $scope.showRowNumer = 15;

        $scope.customerGridOptions = {
            dataSource: new kendo.data.DataSource({
                type: "odata",
                transport: {
                    read: {
                        url: "/api/staff?Includes=Position",
                        dataType: 'json',
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('Authorization', 'Basic ' + $rootScope.globals.currentUser.authdata)
                        }
                    }
                },
                schema: {
                    data: "Data",
                    total: "Total",
                },
                pageSize: $scope.showRowNumer,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true
            }),
            sortable: true,
            pageable: true,
            columns: [{
                field: "StaffId",
                title: "#",
                width: '50px'
            }, {
                field: "Name",
                title: "Họ và Tên",
            }, {
                field: "Gender",
                title: "Giới tính",
                template: '{{dataItem.Gender ? "Nữ" : "Nam" }}'
            }, {
                field: "Phone",
                title: "Điện thoại",
            }, {
                field: "BirthDay",
                title: "Ngày sinh",
                template: '{{dataItem.BirthDay | date: "dd/MM/yyyy"}}'
            }, {
                field: "PositionName",
                title: "Vị trí",
            }]
        }

        $scope.addNewStaff = function () {
            var windowInstance = $kWindow.open({
                options: {
                    modal: true,
                    title: 'Thêm nhân viên',
                    resizable: true,
                    visible: false,
                    width: '550px',
                    height: '400px'
                },
                template: '<staff-form on-save="onSave()"></staff-form>',
                controller: ['$scope', '$windowInstance', function ($scope, $windowInstance) {
                    $scope.onSave = function () {
                        $windowInstance.close(true);
                    }
                }]
            });

            windowInstance.result.then(function (resp) {
                if (resp) {
                    $scope.staffGrid.dataSource.read();
                    $scope.staffGrid.refresh();
                }
            })
        }

        $scope.refreshStaffGrid = function () {
            $scope.staffGrid.dataSource.read();
            $scope.staffGrid.refresh();
        }

        $scope.changeRowNumber = function () {
            $scope.staffGrid.setOptions({
                dataSource: {
                    pageSize: $scope.showRowNumer
                }
            })
        }

        $scope.applyFilter = function () {
            var filters = [];

            if (!angular.isUndefined($scope.filter.keyword)) {
                filters.push({
                    field: 'Name',
                    operator: "contains",
                    value: $scope.filter.keyword
                });
            }

            if (filters.length > 0) {
                $scope.staffGrid.dataSource.filter({
                    logic: "and",
                    filters: filters
                });
            }
            else {
                $scope.staffGrid.dataSource.filter({});
            }
        }
    }

})();