(function () {
    'use strict';

    angular.module('SalonApp')
        .directive('billForm', function () {
            return {
                restrict: 'E',
                scope: {
                    bill: '=',
                    onSave: '&',
                    onRemove: '&'
                },
                templateUrl: 'Scripts/Salon/Shared/Directives/BillForm/BillForm.view.html',
                controller: ['$scope', '$rootScope', 'ServiceService', 'UltiService', '$kWindow', 'toastr', 'constant', 'BillService', 'MsgBox', function ($scope, $rootScope, ServiceService, UltiService, $kWindow, toastr, constant, BillService, MsgBox) {

                    if (!$scope.bill) {
                        $scope.bill = {};
                    }
                    else {
                        if($scope.bill.CustomerSugest){

                        }
                            $scope.bill.CustomerSugest = [];

                        $scope.bill.CustomerSugest[0] = $scope.bill.Customer;
                    }

                    if (!$scope.bill.BillServices)
                        $scope.bill.BillServices = [];

                    $scope.autofindCustomerOptions = {
                        dataSource: new kendo.data.DataSource({
                            type: "odata",
                            transport: {
                                read: {
                                    url: "/Api/Customer",
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
                        dataTextField: "CustomerName",
                        filter: "contains",
                        select: function (e) {
                            var item = e;
                            console.log(item);
                        }
                    };

                    $scope.bfServiceGridOptions = {
                        dataSource: new kendo.data.DataSource ({
                            transport: {
                                read: function (e) {
                                    e.success($scope.bill.BillServices);
                                }
                            },
                            pageSize: 10,
                            aggregate: [
                                {
                                    field: 'Price',
                                    aggregate: 'sum'
                                }
                            ]
                        }),
                        sortable: true,
                        pageable: true,
                        columns: [{
                            field: "Service",
                            title: "Dịch vụ sử dụng",
                            template: '{{dataItem.Name}}',
                            footerTemplate: 'Tổng: '
                        }, {
                            field: "Price",
                            title: "Giá",
                            format: "{0:n0}",
                            aggregates: ["sum"],
                            footerTemplate: '#=kendo.toString(sum,"n0")#'
                        }, {
                            field: "DiscountPrice",
                            title: 'Ưu đãi',
                            format: "{0:n0}",
                            template: '<input type="text" class="discount-text" ng-model="dataItem.DiscountRatio" ng-show="dataItem.DiscountType == 2" ng-change="updateRealPrice(dataItem)"/>\
                                        <input type="text" class="discount-text" ng-model="dataItem.DiscountPrice" ng-show="dataItem.DiscountType == 1" ng-change="updateRealPrice(dataItem)"/>\
                                            <kendo-button class="k-button k-primary discount-btn" ng-click="changeDiscountType(dataItem)">{{dataItem.DiscountType == 1? "VND" : "%" }}</kendo-button>',
                            width: '170px'
                        }, {
                            field: 'RealPrice',
                            title: 'Thanh toán',
                            template: '{{dataItem.RealPrice | number:0}}',
                            format: "{0:n0}",
                            footerTemplate: '{{sumRealPrice() | number:0}}'
                        }, {
                            template: ' <button class="btn btn-icon-only btn-xs red" ng-click="removeUseService(dataItem)"><i class="fa fa-remove"></i></button>'
                        }]
                    }

                    $scope.ddlServiceGroupOptions = {
                        optionLabel: 'Lựa chọn nhóm dịch vụ',
                        dataTextField: 'Name',
                        dataValueField: 'ServiceGroupId',
                        dataSource: new kendo.data.DataSource({
                            type: "odata",
                            transport: {
                                read: {
                                    url: "/Api/ServiceGroup",
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

                    $scope.ddlServiceOptions = {
                        optionLabel: 'Lựa chọn dịch vụ',
                        dataTextField: 'Name',
                        dataValueField: 'ServiceId',
                        cascadeFrom: "ServiceGroup",
                        dataSource: new kendo.data.DataSource({
                            type: "odata",
                            transport: {
                                read: {
                                    url: "/Api/Service",
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

                    $scope.dllMainStaffOptions = {
                        optionLabel: 'Lựa chọn nhân viên chính',
                        dataTextField: 'Name',
                        dataValueField: 'StaffId',
                        dataSource: new kendo.data.DataSource({
                            type: "odata",
                            transport: {
                                read: {
                                    url: "/api/staff?PositionId=" + constant.staff.mainStaff,
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

                    $scope.ddlSubStaffOptions = {
                        optionLabel: 'Lựa chọn nhân viên phụ',
                        dataTextField: 'Name',
                        dataValueField: 'StaffId',
                        dataSource: new kendo.data.DataSource({
                            type: "odata",
                            transport: {
                                read: {
                                    url: "/api/staff?PositionId=" + constant.staff.subStaff,
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

                    $scope.addServiceForCustomer = function () {
                        
                        if ($scope.bill.CustomerSugest != null && $scope.bill.CustomerSugest[0] != undefined) {
                            ServiceService.getServiceForCustomer($scope.bill.CustomerSugest[0].CustomerId, $scope.ServiceId)
                                .then(function (resp) {
                                    if (resp != null) {
                                        resp.DiscountType = 1;
                                        resp.DiscountPrice = 0;
                                        resp.DiscountRatio = 0;
                                        resp.RealPrice = resp.Price - resp.DiscountPrice;

                                        $scope.bill.BillServices.unshift(resp);
                                        $scope.bfServiceGridOptions.dataSource.read();
                                    }
                                });
                        }
                        else {
                            toastr.error('Bạn chưa chọn khách hàng');
                        }
                    };

                    function getIndexByIdUseService(item) {
                        for (var i = 0; i < $scope.bill.BillServices.length; i++) {
                            if ($scope.bill.BillServices.PriceBookId == item.PriceBookId) {
                                return i;
                            }
                        }

                        return -1;
                    }

                    $scope.removeUseService = function (item) {
                        $scope.bill.BillServices.splice(getIndexByIdUseService(item), 1);
                        $scope.bfServiceGridOptions.dataSource.remove(item);
                    }

                    $scope.updateRealPrice = function (item) {
                        var newPrice = 0;

                        if (item.DiscountType == 1) {
                            newPrice = item.Price - item.DiscountPrice;
                        }
                        else {
                            if (item.DiscountRatio > 100)
                                item.DiscountRatio = 100;

                            newPrice = item.Price - ((item.Price / 100) * item.DiscountRatio);
                        }
                        
                        console.log(newPrice);

                        $scope.bfServiceGridOptions.dataSource.getByUid(item.uid).RealPrice = newPrice;
                    }

                    $scope.changeDiscountType = function (item) {
                        if (item.DiscountType == 1) {
                            $scope.bfServiceGridOptions.dataSource.getByUid(item.uid).DiscountType = 2;
                        }
                        else {
                            $scope.bfServiceGridOptions.dataSource.getByUid(item.uid).DiscountType = 1;
                        }

                        updateRealPrice(item);
                    }

                    $scope.sumRealPrice = function () {
                        var sum = 0;
                        var data = $scope.bfServiceGridOptions.dataSource.data();
                        for (var i = 0; i < data.length; i++) {
                            sum += data[i].RealPrice;
                        }

                        $scope.bill.Price = sum;

                        return sum;
                    }

                    $scope.addNewCustomer = function () {
                        var windowInstance = $kWindow.open({
                            options: {
                                modal: true,
                                title: 'Thêm khách hàng mới',
                                resizable: true,
                                visible: false,
                                width: '900px',
                                height: '600px'
                            },
                            template: '<customer-form></customer-form>',
                            controller: ['$scope', function ($scope) {

                            }]
                        });
                    }

                    $scope.saveBill = function () {
                        $scope.bill.CustomerId = $scope.bill.CustomerSugest[0].CustomerId;

                        var postData = angular.copy($scope.bill);

                        delete postData.Customer;

                        if (!$scope.bill.Id) {
                            BillService.addBill(postData)
                                .then(function (data) {
                                    toastr.success("Tạo hóa đơn thành công");

                                    if (angular.isFunction($scope.onSave)) {
                                        $scope.onSave();
                                    }

                                });
                        }
                        else {
                            BillService.updateBill(postData)
                               .then(function (data) {
      
                                   toastr.success("Sửa hóa đơn thành công");

                                   if (angular.isFunction($scope.onSave)) {
                                       $scope.onSave();
                                   }

                               });
                        }                        
                    }

                    $scope.removeBill = function () {
                        MsgBox.confirm('Xóa hóa đơn', 'Bạn có chắc chắn muốn xóa hóa đơn <b>HD' + $scope.bill.Id + '</b> này không?')
                            .then(function (resp) {
                                if (resp) {
                                    BillService.deleteBill($scope.bill.Id)
                                        .then(function () {
                                            toastr.success("Xóa hóa đơn thành công");

                                            if (angular.isFunction($scope.onRemove)) {
                                                $scope.onRemove();
                                            }
                                        },
                                        function (e) {
                                            toastr.error(e.Message, "Xóa hóa đơn thất bại");
                                        });
                                }
                            })
                    }

                    $scope.openWdPrintBill = function () {
                        var windowInstance = $kWindow.open({
                            option: {
                                modal: true,
                                title: 'Chọn mẫu in',
                                resizable: true,
                                visible: false,
                                width: '900px',
                                height: '400px',
                                top: '10px'
                            }, 
                        })
                    }
                }]
            }

        });
})();