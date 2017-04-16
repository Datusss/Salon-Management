(function () {
    'use strict';

    angular.module('SalonApp')
        .factory('PriceBookService', PriceBookService);

    /* @ngInject */
    function PriceBookService($q, $http) {
        var service = {};

        service.find = find;

        return service;

        function find(data) {
            var defer = $q.defer();

            $http.get('/Api/PriceBook/Find?Includes=ServiceName&Includes=ServiceType', { params: { ServiceId: data.ServiceId, ServiceTypeId: data.ServiceTypeId } })
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