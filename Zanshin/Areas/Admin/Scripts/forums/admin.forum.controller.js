// ReSharper disable once InconsistentNaming
var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var ForumController = (function () {
            function ForumController($scope, forumService) {
                this.$scope = $scope;
                this.forumService = forumService;
                $scope.forum = forumService.createForum();
                $scope.addForum = function (forum) {
                    $("#errorDiv").hide();
                    $("#spinner").toggle();
                    $scope.validationMessage = "";
                    forum.dateCreated = new Date(Date.now());
                    forum.lastUpdated = new Date(Date.now());
                    // hack
                    forum.categoryId = 1;
                    forum.moderatorId = 1;
                    forum.moderatorGroupId = 1;
                    forumService.addForum(forum).catch(function (response) {
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
            }
            ForumController.$inject = ["$scope", "forumService"];
            return ForumController;
        })();
        angular.module("zanshin.admin")
            .controller("forumController", ForumController);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=admin.forum.controller.js.map