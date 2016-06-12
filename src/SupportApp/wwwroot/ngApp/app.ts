namespace SupportApp {

    angular.module('SupportApp', ['ui.router', 'ngResource', 'ui.bootstrap']).config((
        $stateProvider: ng.ui.IStateProvider,
        $urlRouterProvider: ng.ui.IUrlRouterProvider,
        $locationProvider: ng.ILocationProvider
    ) => {
        // Define routes
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/ngApp/views/home.html',
                controller: SupportApp.Controllers.HomeController,
                controllerAs: 'controller'
            })
            .state('secret', {
                url: '/secret',
                templateUrl: '/ngApp/views/secret.html',
                controller: SupportApp.Controllers.SecretController,
                controllerAs: 'controller'
            })
            .state('login', {
                url: '/login',
                templateUrl: '/ngApp/views/login.html',
                controller: SupportApp.Controllers.LoginController,
                controllerAs: 'controller'
            })
            .state('register', {
                url: '/register',
                templateUrl: '/ngApp/views/register.html',
                controller: SupportApp.Controllers.RegisterController,
                controllerAs: 'controller'
            })
            .state('externalRegister', {
                url: '/externalRegister',
                templateUrl: '/ngApp/views/externalRegister.html',
                controller: SupportApp.Controllers.ExternalRegisterController,
                controllerAs: 'controller'
            })
            .state("events", {
                url: "/events",
                templateUrl: "/ngApp/views/events.html",
                controller: SupportApp.Controllers.EventsController,
                controllerAs: "controller"
            })
            .state("eventDetails", {
                url: "/event/details/:id",
                templateUrl: "/ngApp/views/eventDetails.html",
                controller: SupportApp.Controllers.EventsDetailController,
                controllerAs: "controller"
            })
            .state('about', {
                url: '/about',
                templateUrl: '/ngApp/views/about.html',
                controller: SupportApp.Controllers.AboutController,
                controllerAs: 'controller'
            });            
        //    .state('notFound', {
        //        url: '/notFound',
        //        templateUrl: '/ngApp/views/notFound.html'
        //    });

        //// Handle request for non-existent route
        //$urlRouterProvider.otherwise('/notFound');

        // Enable HTML5 navigation
        $locationProvider.html5Mode(true);
    });

    
    angular.module('SupportApp').factory('authInterceptor', (
        $q: ng.IQService,
        $window: ng.IWindowService,
        $location: ng.ILocationService
    ) =>
        ({
            request: function (config) {
                config.headers = config.headers || {};
                config.headers['X-Requested-With'] = 'XMLHttpRequest';
                return config;
            },
            responseError: function (rejection) {
                if (rejection.status === 401 || rejection.status === 403) {
                    $location.path('/login');
                }
                return $q.reject(rejection);
            }
        })
    );

    angular.module('SupportApp').config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptor');
    });

    

}
