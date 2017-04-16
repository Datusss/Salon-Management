(function () {
    'use strict';

    angular.module('SalonApp')
        .directive('customerForm', function () {
            return {
                restrict: 'E',
                scope: {
                    customer: '=',
                    onSave: '&',
                    onRemove: '&'
                },
                templateUrl: 'Scripts/Salon/Shared/Directives/CustomerForm/CustomerForm.view.html',
                controller: ['$scope', '$rootScope', 'CustomerService', 'toastr', 'MsgBox', function ($scope, $rootScope, CustomerService, toastr, MsgBox) {

                    if (!$scope.customer) {
                        $scope.customer = {};
                    }
                    else
                    {
                        if($scope.customer.Gender)
                        {
                            $scope.customer.Gender = "True";
                        }
                        else
                        {
                            $scope.customer.Gender = "False";
                        }
                    }

                    $scope.saveCustomer = function () {
                        if (!$scope.customer.CustomerId) {
                            CustomerService.addCustomer($scope.customer)
                                .then(function () {

                                    toastr.success("Thêm khách hàng thành công");

                                    if (angular.isFunction($scope.onSave)) {
                                        $scope.onSave();
                                    }
                                });
                        }
                        else {
                            CustomerService.updateCustomer($scope.customer)
                                .then(function () {
                                    toastr.success("Cập nhật thông tin khách hàng thành công");

                                    if (angular.isFunction($scope.onSave)) {
                                        $scope.onSave();
                                    }
                                });
                        }
                    };

                    $scope.removeCustomer = function () {
                        MsgBox.confirm('Xóa khách hàng', 'Bạn có chắc chắn muốn xóa khách hàng <b>' + $scope.customer.CustomerName + '</b> này không?')
                            .then(function (resp) {
                                if (resp) {
                                    CustomerService.deleteCustomer($scope.customer.CustomerId)
                                        .then(function () {
                                            toastr.success("Xóa khách hàng thành công");

                                            if (angular.isFunction($scope.onRemove)) {
                                                $scope.onRemove();
                                            }
                                        },
                                        function (e) {
                                            toastr.error(e.Message, "Xóa khách hàng thất bại");
                                        });
                                }
                            })
                    }
                }]
            }

        });
})();