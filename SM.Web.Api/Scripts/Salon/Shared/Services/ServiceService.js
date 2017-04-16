(function () {
    'use strict';

    angular.module('SalonApp')
        .factory('ServiceService', ServiceService);

    /* @ngInject */
    function ServiceService($q, $http) {
        var service = {};

        service.addService = addService;
        service.updateService = updateService;
        service.getServiceForCustomer = getServiceForCustomer;
        service.deleteService = deleteService;

        return service;

        function addService(service) {
            var defer = $q.defer();

            $http.post('api/service', service)
                .success(function (resp) {
                    defer.resolve(resp);
                })
                .error(function (error) {
                    defer.reject(error);
                });

            return defer.promise;
        }

        function updateService(service) {
            var defer = $q.defer();

            $http.put('api/service', service)
                .success(function (resp) {
                    defer.resolve(resp);
                })
                .error(function (error) {
                    defer.reject(error);
                });

            return defer.promise;
        }

        function getServiceForCustomer(customerId, serviceId) {
            var defer = $q.defer();

            $http.get('api/service/get-service-for-customer', {
                params: {
                    ServiceId: serviceId,
                    CustomerId: customerId
                }
            })
            .success(function (resp) {
                defer.resolve(resp);
            })
            .error(function (error) {
                defer.reject(error);
            });

            return defer.promise;
        }

        function deleteService(serviceId) {
            var defer = $q.defer();

            $http.delete('api/service/' + serviceId)
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