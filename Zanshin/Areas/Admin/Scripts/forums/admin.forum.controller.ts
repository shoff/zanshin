
// ReSharper disable once InconsistentNaming

module zanshin.admin {
    "use strict";
    interface IMyScope extends ng.IScope {
        addForum: Function;
        validationMessage: any;
        forums: Array<Forum>;
        forum: Forum;
    }

    class ForumController {
        static $inject = ["$scope", "forumService"];

        constructor(private $scope: IMyScope,
            private forumService: IForumService) {

            $scope.forum = forumService.createForum();

            $scope.addForum = (forum: Forum) => {
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
    }

    angular.module("zanshin.admin")
        .controller("forumController", ForumController);
}
 