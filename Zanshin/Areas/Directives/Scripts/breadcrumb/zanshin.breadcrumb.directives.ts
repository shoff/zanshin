
//module zanshin.breadcrumb {
//    angular.module('zanshin.breadcrumb',[]);
//}

module zanshin.breadcrumb {
    "use strict";
    angular
        .module("zanshin")
        .directive("zanBreadCrumb", [function () {

        var directive: ng.IDirective = {
            restrict: 'E',
            replace: true,
            templateUrl: "/Directives/Breadcrumb",
            controller: "BreadCrumbController"
        }
        return directive;
    }]);
} 