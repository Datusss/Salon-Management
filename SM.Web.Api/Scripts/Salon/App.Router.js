(function () {
	'use strict';

	angular.module('SalonApp').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
		$urlRouterProvider.otherwise('/Login');

		$stateProvider
			//for login layout
			.state('Login', {
			    abstract: true,
			    templateUrl: 'Scripts/Salon/Shared/Layout/LoginLayout.html'
			})
			.state('Login.Login', {
			    url: '/Login',
			    templateUrl: 'Scripts/Salon/Login/Login.view.html',
			    controller: 'LoginController',
			    controllerAs: 'vm'
			})
			.state('Login.Logout', {
			    url: '/Logout',
			    controller: function ($state, AuthenticationService) {
			        AuthenticationService.ClearCredentials();
			        $state.go('Login.LoginForm');
			    }
			})
			//for admin layout
			.state('App', {
			    abstract: true,
			    templateUrl: 'Scripts/Salon/Shared/Layout/AppLayout.html'
			})
			.state('App.Dashboard', {
			    url: '/',
			    templateUrl: 'Scripts/Salon/Dashboard/Dashboard.view.html',
			    controller: 'DashboardController',
			    controllerAs: 'vm'
			})
            .state('App.Bill', {
                url: '/Bill',
                templateUrl: 'Scripts/Salon/Bill/Bill.view.html',
                controller: 'BillController',
                controllerAs: 'vm'
            })
            .state('App.Customer', {
                url: '/Customer',
                templateUrl: 'Scripts/Salon/Customer/Customer.view.html',
                controller: 'CustomerController',
                controllerAs: 'vm'
            })
            .state('App.Service', {
                url: '/Service',
                templateUrl: 'Scripts/Salon/Service/Service.view.html',
                controller: 'ServiceController',
                controllerAs: 'vm'
            })
	        .state('App.Staff', {
	            url: '/Staff',
	            templateUrl: 'Scripts/Salon/Staff/Staff.view.html',
	            controller: 'StaffController',
	            controllerAs: 'vm'
	        });
	}])
    .run(['$rootScope', '$location', '$cookieStore', '$http',
        function ($rootScope, $location, $cookieStore, $http) {
            $rootScope.globals = $cookieStore.get('globals') || {};

            if ($rootScope.globals.currentUser) {
            	$http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata;
            }

            $rootScope.$on('$locationChangeStart', function (event, next, current) {
                if ($location.path() !== '/Login' && !$rootScope.globals.currentUser) {
                    $location.path('/Login');
                }
            });

            $rootScope.$on("kendoRendered", function (e) {
                $("#filter").data("kendoPanelBar").expand($("#filter .k-item"), true);
            });
        }
    ])
})();