'use strict';
commentApp.directive('comment', function () {
    return {
        restrict: "E",
        controller: 'commentCtrl',
        templateUrl: 'scripts/comments/Comments.html?1'
    };
});

