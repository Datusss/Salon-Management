(function () {
    'use strict';

    angular.module('SalonApp')
        .factory('BillService', BillService);

    /* @ngInject */
    function BillService($q, $http) {
        var service = {};

        service.addBill = addBill;
        service.updateBill = updateBill;
        service.getBillById = getBillById;
        service.deleteBill = deleteBill;

        return service;

        function addBill(bill) {
            var defer = $q.defer();

            $http.post('api/bill', bill)
                .success(function (resp) {
                    defer.resolve(resp);
                })
                .error(function (error) {
                    defer.reject(error);
                });

            return defer.promise;
        }

        function updateBill(bill) {
            var defer = $q.defer();

            $http.put('api/bill', bill)
                .success(function (resp) {
                    defer.resolve(resp);
                })
                .error(function (error) {
                    defer.reject(error);
                });

            return defer.promise;
        }

        function getBillById(billId, includes) {
            var defer = $q.defer();
            var query = '';

            if (includes && includes.length > 0) {

                var first = true;

                angular.forEach(includes, function (include) {

                    if(first)
                        query += '?Includes=' + include;
                    else
                        query += '&Includes=' + include;
                })
            }

            $http.get('api/bill/' + billId + query)
            .success(function (resp) {
                defer.resolve(resp);
            })
            .error(function (error) {
                defer.reject(error);
            });

            return defer.promise;
        }

        function deleteBill(billId) {
            var defer = $q.defer();

            $http.delete('api/bill/' + billId)
            .success(function (resp) {
                defer.resolve(resp);
            })
            .error(function (error) {
                defer.reject(error);
            });

            return defer.promise;
        }
    }

})();