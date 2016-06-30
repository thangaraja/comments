'use strict';
commentApp.directive('comment', function () {
    return {
        restrict: "E",
        controller: 'commentCtrl',
        templateUrl: 'scripts/comments/Comments.html'
    };
});

'use strict';
commentApp.directive('includeReplace', function () {
    return {
        require: 'ngInclude',
        restrict: 'A', /* optional */
        link: function (scope, el, attrs) {
            el.replaceWith(el.children());
        }
    };
});