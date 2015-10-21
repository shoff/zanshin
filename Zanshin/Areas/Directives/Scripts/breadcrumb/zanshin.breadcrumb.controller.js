var zanshin;
(function (zanshin) {
    var breadcrumb;
    (function (breadcrumb) {
        "use strict";
        var BreadCrumbController = (function () {
            function BreadCrumbController($scope) {
                this.$scope = $scope;
                this.init();
            }
            BreadCrumbController.prototype.init = function () {
                // TODO
            };
            BreadCrumbController.$inject = ["$scope"];
            return BreadCrumbController;
        })();
        breadcrumb.BreadCrumbController = BreadCrumbController;
        angular
            .module("zanshin")
            .controller("BreadCrumbController", BreadCrumbController);
    })(breadcrumb = zanshin.breadcrumb || (zanshin.breadcrumb = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.breadcrumb.controller.js.map