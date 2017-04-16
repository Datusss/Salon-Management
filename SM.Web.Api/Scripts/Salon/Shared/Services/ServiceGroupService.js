(function () {
    'use strict';

    angular.module('SalonApp')
        .factory('ServiceGroupService', ServiceGroupService);

    /* @ngInject */
    function ServiceGroupService($q, $http) {
        var service = {};

        service.addServiceGroup = addServiceGroup;
        service.updateServiceGroup = updateServiceGroup;

        return service;

        function addServiceGroup(serviceGroup) {
            var defer = $q.defer();

            $http.post('api/serviceGroup', serviceGroup)
                .success(function (resp) {
                    defer.resolve(resp);
                })
                .error(function (error) {
                    defer.reject(error);
                });

            return defer.promise;
        }

        function updateServiceGroup(serviceGroup) {
            var defer = $q.defer();

            $http.put('api/serviceGroup', serviceGroup)
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