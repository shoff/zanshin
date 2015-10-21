var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var AdminHomeController = (function () {
            function AdminHomeController($scope, homeService) {
                //$scope.category = categoryService.createCategory();
                this.$scope = $scope;
                this.homeService = homeService;
                //$scope.addCategory = (category: Category) => {
                //    category.dateCreated = new Date(Date.now());
                //    category.lastUpdated = new Date(Date.now());
                // go to the forum index - temp hack
                //window.location.replace("/Admin/Forums");
                //};
                this.getMessages();
                this.getLogs();
            }
            AdminHomeController.prototype.getMessages = function () {
                var _this = this;
                this.homeService.getMessages().success(function (data) {
                    _this.$scope.messages = data;
                }).error(function (error) {
                    _this.$scope.validationMessage = error.exceptionMessage;
                });
            };
            AdminHomeController.prototype.getLogs = function () {
                var _this = this;
                this.homeService.getLogs().success(function (data) {
                    _this.$scope.logs = data;
                }).error(function (error) {
                    _this.$scope.validationMessage = error.exceptionMessage;
                });
            };
            AdminHomeController.$inject = ["$scope", "homeService"];
            return AdminHomeController;
        })();
        angular.module("zanshin.admin")
            .controller("adminHomeController", AdminHomeController);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=admin.home.controller.js.map