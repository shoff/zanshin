var zanshin;
(function (zanshin) {
    var user;
    (function (user) {
        "use strict";
        var UserController = (function () {
            function UserController($scope, userService) {
                this.$scope = $scope;
                this.userService = userService;
            }
            UserController.prototype.getUser = function (userId) {
                var _this = this;
                this.userService.getUser(userId).then(function (result) {
                    _this.$scope.user = result.data;
                }, function (error) {
                    _this.$scope.validation = error.statusText;
                });
            };
            UserController.$inject = ["$scope", "userService"];
            return UserController;
        })();
        angular
            .module("zanshin")
            .controller("UserController", UserController);
    })(user = zanshin.user || (zanshin.user = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.user.controller.js.map