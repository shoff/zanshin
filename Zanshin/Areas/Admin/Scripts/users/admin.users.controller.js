// ReSharper disable once InconsistentNaming
var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var AdminUsersController = (function () {
            function AdminUsersController($scope, userService) {
                this.$scope = $scope;
                this.userService = userService;
                this.getUsers();
            }
            AdminUsersController.prototype.getUsers = function () {
                var _this = this;
                this.userService.getUsers(0).success(function (data) {
                    _this.$scope.users = data.currentPage;
                }).error(function (error) {
                    _this.$scope.validationMessage = error.exceptionMessage;
                });
            };
            AdminUsersController.$inject = ["$scope", "userService"];
            return AdminUsersController;
        })();
        admin.AdminUsersController = AdminUsersController;
        angular.module("zanshin.admin")
            .controller("adminUsersController", AdminUsersController);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=admin.users.controller.js.map