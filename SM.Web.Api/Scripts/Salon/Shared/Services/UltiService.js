(function () {
    'use strict';

    angular.module('SalonApp')
        .factory('UltiService', UltiService);

    /* @ngInject */
    function UltiService($q, $http) {
        var service = {};

        service.getDistrict = getDistrict;

        return service;

        function getDistrict(city) {
            if (city == "Hà Nội") {
                var hanoi = [
                    "Quận Ba Đình",
                    "Quận Hoàn Kiếm",
                    "Quận Hai Bà Trưng",
                    "Quận Đống Đa",
                    "Quận Tây Hồ",
                    "Quận Cầu Giấy",
                    "Quận Thanh Xuân",
                    "Quận Hoàng Mai",
                    "Quận Long Biên",
                    "Quận Hà Đông",
                ]

                return hanoi;
            }
            else if(city == "Hồ Chí Minh"){
                var hcm = [
                    "Quận 1",
                    "Quận 12",
                    "Quận Thủ Đức",
                    "Quận 9",
                    "Quận Gò Vấp",
                    "Quận Bình Thạnh",
                    "Quận Tân Bình",
                    "Quận Tân Phú",
                    "Quận Phú Nhuận",
                    "Quận 2",
                    "Quận 3",
                    "Quận 10",
                    "Quận 11",
                    "Quận 4",
                    "Quận 5",
                    "Quận 6",
                    "Quận 8",
                    "Quận Bình Tân",
                    "Quận 7",
                ]

                return hcm;
            }
        }
    }
})();