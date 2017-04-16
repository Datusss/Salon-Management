(function () {
    'use strict';

    angular.module('SalonApp')
        .controller('ServiceController', ServiceController);

    /* @ngInject */
    function ServiceController($scope, $rootScope, $kWindow) {

        $scope.filter = {};
        $scope.showRowNumer = 15;

        $scope.serviceGridOptions = {
            dataSource: new kendo.data.DataSource({
                type: "odata",
                transport: {
                    read: {
                        url: "/api/service?Includes=ServiceGroup",
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
                field: "ServiceId",
                title: "#",
                width: '50px'
            }, {
                field: "Name",
                title: "Tên dịch vụ",
            }, {
                field: "Price",
                title: "Giá",
                format: '{0:n0}'
            }, {
                title: "Nhóm dịch vụ",
                template: '{{dataItem.ServiceGroup ? dataItem.ServiceGroup.Name : "" }}'
            }, {
                field: "Rank",
                title: "Thứ tự",
            }]
        }

        $scope.serviceGroupListOptions = {
            dataSource: new kendo.data.HierarchicalDataSource({
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
            template: '<span class="wrap-sgroup-tree-item"><span>{{dataItem.Name}}</span> <a href=""><i class="fa fa-pencil-square-o"></i></a></span>',
        }

        $scope.addNewService = function () {
            var windowInstance = $kWindow.open({
                options: {
                    modal: true,
                    title: 'Thêm dịch vụ',
                    resizable: true,
                    visible: false,
                    width: '550px',
                    height: '400px'
                },
                template: '<service-form on-save="onSave()"></service-form>',
                controller: ['$scope', '$windowInstance', function ($scope, $windowInstance) {
                    $scope.onSave = function () {
                        $windowInstance.close(true);
                    }
                }]
            });

            windowInstance.result.then(function (resp) {
                if (resp) {
                    $scope.serviceGrid.dataSource.read();
                    $scope.serviceGrid.refresh();
                }
            })
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
                template: '<service-group-form></service-group-form>',
                controller: ['$scope', function ($scope) {

                }]
            });
        };

        $scope.refreshServiceGrid = function () {
            $scope.serviceGrid.dataSource.read();
            $scope.serviceGrid.refresh();
        }

        $scope.changeRowNumber = function () {
            $scope.serviceGrid.setOptions({
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

            console.log($scope.filter.selectGroupService);

            if ($scope.filter.selectGroupService) {
                filters.push({
                    field: 'ServiceGroupId',
                    operator: "eq",
                    value: $scope.filter.selectGroupService.ServiceGroupId
                });
            }

            if (filters.length > 0) {
                $scope.serviceGrid.dataSource.filter({
                    logic: "and",
                    filters: filters
                });
            }
            else {
                $scope.serviceGrid.dataSource.filter({});
            }
        }
    }

})();