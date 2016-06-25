'use strict';
commentApp.directive('comment', function () {
    return {
        restrict: "E",
        controller: 'commentCtrl',
        templateUrl: 'scripts/comments/Comments.html?1'
    };
});

'use strict';
commentApp.directive('includeReplace', function () {
    return {
        require: 'ngInclude',
        restrict: 'A', /* optional */
        link: function (scope, el, attrs) {
            //debugger;
            el.replaceWith(el.children());
        }
    };
});
