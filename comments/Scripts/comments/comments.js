
var commentApp = angular.module('CommentsApp', ['angular-storage', 'angularMoment']);

commentApp.service('UserService', function (store, $http, $rootScope) {

    var accounturlBase = '/Account/';

    var service = this;

    var setCurrentUser = function () {

        $http({
            method: "get",
            url: accounturlBase + "GetCurrentUser",
        }).success(function (currentUser) {
            store.set('user', currentUser);
            $rootScope.$broadcast('authorized');
        });
        
    };

    setCurrentUser();

    service.getCurrentUser = function () {
        return store.get('user');
    };

});

commentApp.service('APIInterceptor', function ($rootScope, store) {
    var service = this;
    service.request = function (config) {
        return config;
    };
    service.responseError = function (response) {
        if (response.status === 401) {
            $rootScope.$broadcast('unauthorized');
        }
        return response;
    };
});

commentApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('APIInterceptor');
});



