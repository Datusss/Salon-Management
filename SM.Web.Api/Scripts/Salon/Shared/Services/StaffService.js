(function () {
    'use strict';

    angular.module('SalonApp')
        .factory('StaffService', StaffService);

    /* @ngInject */
    function StaffService($q, $http) {
        var service = {};

        service.addStaff = addStaff;
        service.updateStaff = updateStaff;
        service.deleteStaff = deleteStaff;

        return service;

        function addStaff(service) {
            var defer = $q.defer();

            $http.post('api/staff', service)
                .success(function (resp) {
                    defer.resolve(resp);
                })
                .error(function (error) {
                    defer.reject(error);
                });

            return defer.promise;
        }

        function updateStaff(service) {
            var defer = $q.defer();

            $http.put('api/staff', service)
                .success(function (resp) {
                    defer.resolve(resp);
                })
                .error(function (error) {
                    defer.reject(error);
                });

            return defer.promise;
        }

        function deleteStaff(staffId) {
            var defer = $q.defer();

            $http.delete('api/staff/' + staffId)
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