
// ReSharper disable once InconsistentNaming

module zanshin.admin {
    "use strict";
    interface IMyScope extends ng.IScope {
        addGroup: Function;
        validationMessage: any;
        groups: Array<Group>;
        group: Group;
    }

    class GroupController {

        static $inject = ["$scope", "groupService"];

        constructor(private $scope: IMyScope,
            private groupService: IGroupService) {
            
            // $scope.group = groupService.createGroup();

            $scope.addGroup = (group: Group) => {
                $("#errorDiv").hide();
                $("#spinner").toggle();
                $scope.validationMessage = "";
                // group.dateCreated = new Date(Date.now());
                // group.lastUpdated = new Date(Date.now());
                // hack

                group.founderId = 1;

                groupService.addGroup(group).catch((response) => {
                    $scope.validationMessage = response;
                    $("#spinner").hide();
                    $("#errorDiv").toggle();
                }).then(() => {
                    $("#spinner").hide();
                    if ($scope.validationMessage === "") {
                        window.location.replace("/Admin/Forums");
                    }
                });
            };

            this.getCategories();
        }

        getCategories() {
            this.groupService.getGroups().success(data => {
                this.$scope.groups = data;
            }).error(error => {
                this.$scope.validationMessage = error.exceptionMessage;
            });
        }
    }

    angular.module("zanshin.admin")
        .controller("GroupController", GroupController);
}
 