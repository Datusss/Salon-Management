(function () {
    'use strict';

    angular.module('SalonApp')
        .controller('BillController', BillController);

    /* @ngInject */
    function BillController($scope, $rootScope, $kWindow, BillService) {
       
        $scope.filter = {};
        $scope.showRowNumer = 15;

        $scope.billGridOptions = {
            dataSource: new kendo.data.DataSource ({
                type: "odata",
                transport: {
                    read: {
                        url: "/Api/Bill?Includes=Customer&Includes=BillServices",
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
        
        $scope.openWdCreateBill = function () {
            var windowInstance = $kWindow.open({
                options: {
                    modal: true,
                    title: 'Tạo hóa đơn',
                    resizable: true,
                    visible: false,
                    width: '900px',
                    height: '400px',
                    top: '10px'
                },
                template: '<bill-form on-save="onSave()"></bill-form>',
                controller: ['$scope', '$windowInstance', function ($scope, $windowInstance) {
                    $scope.onSave = function () {
                        $windowInstance.close(true);
                    }
                }]
            });

            windowInstance.result.then(function (resp) {
                if (resp) {
                    $scope.billGrid.dataSource.read();
                    $scope.billGrid.refresh();
                }
            })
        }

        $scope.filterBill = function () {

        }

        $scope.getDetailBill = function (billId) {
            var bill = {};

            BillService.getBillById(billId, ['BillServices'])
                .then(function (resp) {
                    bill = resp;
                });

            return bill;
        }

        $scope.refreshBillGrid = function () {
            $scope.billGrid.dataSource.read();
            $scope.billGrid.refresh();
        }

        $scope.changeRowNumber = function () {
            $scope.billGrid.setOptions({
                dataSource: {
                    pageSize: $scope.showRowNumer
                }
            })
        }

        $scope.applyFilter = function () {
            var filters = [];

            if (!angular.isUndefined($scope.filter.keyword)) {
                filters.push({
                    field: 'Customer.CustomerName',
                    operator: "contains",
                    value: $scope.filter.keyword
                });
            }

            if ($scope.filter.startDate) {
                filters.push({
                    field: 'CreatedDate',
                    operator: "gte",
                    value: $scope.filter.startDate
                });
            }

            if ($scope.filter.endDate) {
                filters.push({
                    field: 'CreatedDate',
                    operator: "lte",
                    value: $scope.filter.endDate
                });
            }

            if (filters.length > 0) {
                $scope.billGrid.dataSource.filter({
                    logic: "and",
                    filters: filters
                });
            }
            else {
                $scope.billGrid.dataSource.filter({});
            }
        }
    }

})();