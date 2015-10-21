//module zanshin.breadcrumb {
//    angular.module('zanshin.breadcrumb',[]);
//}
var zanshin;
(function (zanshin) {
    var breadcrumb;
    (function (breadcrumb) {
        "use strict";
        angular
            .module("zanshin")
            .directive("zanBreadCrumb", [function () {
                var directive = {
                    restrict: 'E',
                    replace: true,
                    templateUrl: "/Directives/Breadcrumb",
                    controller: "BreadCrumbController"
                };
                return directive;
            }]);
    })(breadcrumb = zanshin.breadcrumb || (zanshin.breadcrumb = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.breadcrumb.directives.js.map