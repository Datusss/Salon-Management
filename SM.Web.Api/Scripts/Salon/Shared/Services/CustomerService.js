(function () {
    'use strict';

    angular.module('SalonApp')
        .factory('CustomerService', CustomerService);

    /* @ngInject */
    function CustomerService($q, $http) {
        var service = {};

        service.addCustomer = addCustomer;
        service.updateCustomer = updateCustomer;
        service.deleteCustomer = deleteCustomer;

        return service;

        function addCustomer(customer) {
            var defer = $q.defer();

            $http.post('api/customer', customer)
                .success(function (resp) {
                    defer.resolve(resp);
                })
                .error(function (error) {
                    defer.reject(error);
                });

            return defer.promise;
        }

        function updateCustomer(customer) {
            var defer = $q.defer();

            $http.put('api/customer', customer)
                .success(function (resp) {
                    defer.resolve(resp);
                })
                .error(function (error) {
                    defer.reject(error);
                });

            return defer.promise;
        }

        function deleteCustomer(customerId) {
            var defer = $q.defer();

            $http.delete('api/customer/' + customerId)
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