// ReSharper disable once InconsistentNaming
var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var GroupController = (function () {
            function GroupController($scope, groupService) {
                // $scope.group = groupService.createGroup();
                this.$scope = $scope;
                this.groupService = groupService;
                $scope.addGroup = function (group) {
                    $("#errorDiv").hide();
                    $("#spinner").toggle();
                    $scope.validationMessage = "";
                    // group.dateCreated = new Date(Date.now());
                    // group.lastUpdated = new Date(Date.now());
                    // hack
                    group.founderId = 1;
                    groupService.addGroup(group).catch(function (response) {
                        $scope.validationMessage = response;
                        $("#spinner").hide();
                        $("#errorDiv").toggle();
                    }).then(function () {
                        $("#spinner").hide();
                        if ($scope.validationMessage === "") {
                            window.location.replace("/Admin/Forums");
                        }
                    });
                };
                this.getCategories();
            }
            GroupController.prototype.getCategories = function () {
                var _this = this;
                this.groupService.getGroups().success(function (data) {
                    _this.$scope.groups = data;
                }).error(function (error) {
                    _this.$scope.validationMessage = error.exceptionMessage;
                });
            };
            GroupController.$inject = ["$scope", "groupService"];
            return GroupController;
        })();
        angular.module("zanshin.admin")
            .controller("GroupController", GroupController);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=admin.group.controller.js.map