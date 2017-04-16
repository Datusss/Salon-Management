(function () {
    'use strict';

    angular.module('SalonApp')
        .controller('CustomerController', CustomerController);

    /* @ngInject */
    function CustomerController($scope, $rootScope, $kWindow) {

        $scope.filter = {};
        $scope.showRowNumer = 15;

        $scope.customerGridOptions = {
            dataSource: new kendo.data.DataSource({
                type: "odata",
                transport: {
                    read: {
                        url: "/api/customer?Includes=Bills",
                        dataType: 'json',
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('Authorization', 'Basic ' + $rootScope.globals.currentUser.authdata)
                        }
                    }
                },
                schema: {
                    data: "Data",
                    total: "Total"
                },
                pageSize: $scope.showRowNumer,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true
            }),
            sortable: true,
            pageable: true,
            columns: [{
                field: "CustomerId",
                title: "#",
                width: '50px'
            }, {
                field: "CustomerName",
                title: "Họ và Tên",
            }, {
                field: "Gender",
                title: "Giới tính",
                template: '{{dataItem.Gender ? "Nữ" : "Nam" }}'
            }, {
                field: "BirthDay",
                title: "Ngày sinh",
                template: '{{dataItem.BirthDay | date: "dd/MM/yyyy"}}'
            }, {
                field: "City",
                title: "Thành phố",
            }, {
                field: "TotalCost",
                title: "Tổng thanh toán",
                format: "{0:n0}"
            }]
        }

        $scope.addNewCustomer = function () {
            var windowInstance = $kWindow.open({
                options: {
                    modal: true,
                    title: 'Thêm khách hàng mới',
                    resizable: true,
                    visible: false,
                    width: '400px',
                    height: '450px'
                },
                template: '<customer-form on-save="onSave()"></customer-form>',
                controller: ['$scope', '$windowInstance', function ($scope, $windowInstance) {
                    $scope.onSave = function () {
                        $windowInstance.close(true);
                    }
                }]
            });

            windowInstance.result.then(function (resp) {
                if (resp) {
                    $scope.customerGrid.dataSource.read();
                    $scope.customerGrid.refresh();
                }
            })
        }

        $scope.refreshCustomerGrid = function () {
            $scope.customerGrid.dataSource.read();
            $scope.customerGrid.refresh();
        }

        $scope.changeRowNumber = function () {
            $scope.customerGrid.setOptions({
                dataSource: {
                    pageSize: $scope.showRowNumer
                }
            })
        }

        $scope.applyFilter = function () {
            var filters = [];

            if (!angular.isUndefined($scope.filter.keyword)) {
                filters.push({
                    field: 'CustomerName',
                    operator: "contains",
                    value: $scope.filter.keyword
                });
            }

            if (filters.length > 0) {
                $scope.customerGrid.dataSource.filter({
                    logic: "and",
                    filters: filters
                });
            }
            else {
                $scope.customerGrid.dataSource.filter({});
            }
        }

        $scope.getCustomerBillGridOptions = function (CustomerId) {
            var options = {
                dataSource: new kendo.data.DataSource({
                    type: "odata",
                    transport: {
                        read: {
                            url: "/Api/Bill?Includes=Customer&Includes=BillServices&$filter=CustomerId eq " + CustomerId,
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
                    pageSize: 10,
                    serverPaging: true,
                }),
                sortable: true,
                pageable: true,
                columns: [{
                    field: "Id",
                    title: "Mã hóa đơn",
                    template: 'HD{{dataItem.Id}}',
                }, {
                    field: "CustomerName",
                    title: "Khách hàng",
                }, {
                    field: "CustomerPhone",
                    title: "Điện thoại",
                }, {
                    field: "CreatedDate",
                    title: "Ngày tháng",
                    template: "{{dataItem.CreatedDate | date: 'dd/MM/yyyy hh:mm'}}"
                }, {
                    field: "Price",
                    title: "Thanh toán",
                    format: "{0:n0}"
                }]
            }

            return options;
        }
    }

})();