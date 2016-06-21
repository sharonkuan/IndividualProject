namespace SupportApp {

    angular.module('SupportApp', ['ui.router', 'ngResource', 'ui.bootstrap', 'angular.filter']).config((
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
            //Home Events
            .state("historyEvents", {
                url: "/historyEvents",
                templateUrl: "/ngApp/views/historyEvents.html",
                controller: SupportApp.Controllers.HistoryEventsController,
                controllerAs: "controller"
            })
            .state("activeEventDetails", {
                url: "/event/activeEventDetails/:id",
                templateUrl: "/ngApp/views/activeEventDetails.html",
                controller: SupportApp.Controllers.ActiveEventDetailController,
                controllerAs: "controller"
            })
            .state("historyEventDetails", {
                url: "/event/historyEventDetails/:id",
                templateUrl: "/ngApp/views/historyEventDetails.html",
                controller: SupportApp.Controllers.HistoryEventDetailController,
                controllerAs: "controller"
            })
            //My Events
            .state("myEvents", {
                url: "/myEvents",
                templateUrl: "/ngApp/views/myEvents.html",
                controller: SupportApp.Controllers.MyEventsController,
                controllerAs: "controller"
            })
            .state("myHistoryEvents", {
                url: "/myHistoryEvents",
                templateUrl: "/ngApp/views/myHistoryEvents.html",
                controller: SupportApp.Controllers.MyHistoryEventsController,
                controllerAs: "controller"
            })
            .state("myEventDetails", {
                url: "/event/myEventDetails/:id",
                templateUrl: "/ngApp/views/myEventDetails.html",
                controller: SupportApp.Controllers.MyEventDetailController,
                controllerAs: "controller"
            })
            .state("myHistoryEventDetails", {
                url: "/event/myHistoryEventDetails/:id",
                templateUrl: "/ngApp/views/myHistoryEventDetails.html",
                controller: SupportApp.Controllers.MyHistoryEventDetailController,
                controllerAs: "controller"
            })
            //Admin Events
            .state("adminEvents", {
                url: "/adminEvents",
                templateUrl: "/ngApp/views/adminEvents.html",
                controller: SupportApp.Controllers.AdminEventsController,
                controllerAs: "controller"
            })
            .state("adminHistoryEvents", {
                url: "/adminHistoryEvents",
                templateUrl: "/ngApp/views/adminHistoryEvents.html",
                controller: SupportApp.Controllers.AdminHistoryEventsController,
                controllerAs: "controller"
            })
            .state("adminEventDetails", {
                url: "/event/adminEventDetails/:id",
                templateUrl: "/ngApp/views/adminEventDetails.html",
                controller: SupportApp.Controllers.AdminEventDetailController,
                controllerAs: "controller"
            })
            .state("adminHistoryEventDetails", {
                url: "/event/adminHistoryEventDetails/:id",
                templateUrl: "/ngApp/views/adminHistoryEventDetails.html",
                controller: SupportApp.Controllers.AdminHistoryEventDetailController,
                controllerAs: "controller"
            })
            //Event Create, Admin Add Additional Address, and Edit
            .state("userEventCreate", {
                url: "/userEvent/create",
                templateUrl: "/ngApp/views/userEventCreate.html",
                controller: SupportApp.Controllers.UserEventCreateController,
                controllerAs: "controller"
            })
            .state("eventAddressAdd", {
                url: "/event/address/add/:id",
                templateUrl: "/ngApp/views/eventAddressAdd.html",
                controller: SupportApp.Controllers.EventAddressAddController,
                controllerAs: "controller"
            })
            .state("eventAddressEdit", {
                url: "/event/address/edit/:id",
                templateUrl: "/ngApp/views/eventAddressEdit.html",
                controller: SupportApp.Controllers.EventAddressEditController,
                controllerAs: "controller"
            })
            .state("eventEdit", {
                url: "/event/edit/:id",
                templateUrl: "/ngApp/views/eventEdit.html",
                controller: SupportApp.Controllers.EventEditController,
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
