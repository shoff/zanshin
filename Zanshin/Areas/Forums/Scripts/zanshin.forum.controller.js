var zanshin;
(function (zanshin) {
    var forums;
    (function (forums) {
        "use strict";
        // TODO split this into categories controller and forum controller
        var ForumController = (function () {
            function ForumController($scope, forumService, coreService, $window) {
                this.$scope = $scope;
                this.forumService = forumService;
                this.coreService = coreService;
                this.$window = $window;
                // set default values
                this.pageNumber = 1;
                this.pageSize = 20;
                this.$scope.postsLoaded = false;
                this.$scope.topicCount = 99999999;
                this.$scope.currentPage = "1";
                // this is kinda hackish, see above todo
                this.init();
                if ((this.forumId) && (this.forumId > 0)) {
                    this.getForum(this.forumId, 0, 0);
                }
                else {
                    this.getForums();
                }
            }
            ForumController.prototype.init = function () {
                if (this.$window.location.pathname) {
                    var pathName = this.$window.location.pathname.split('/');
                    var last = +pathName[pathName.length - 1];
                    if (last !== NaN) {
                        this.forumId = last;
                    }
                    else {
                        // todo
                        this.$window.location.assign("/404");
                    }
                }
                // more hacking
                if (this.$window.location.search) {
                    //?pageNumber=3
                    var expand = this.$window.location.search.split('&');
                    for (var e = 0; e < expand.length; e++) {
                        var pn = expand[0].slice(1);
                        if (pn) {
                            var q = pn.split('=');
                            if (q.length === 2) {
                                if (q[0].toLowerCase() === "pagenumber") {
                                    this.pageNumber = +q[1];
                                    this.$scope.currentPage = this.pageNumber.toString();
                                }
                                else if (q[0].toLowerCase() === "pagesize") {
                                    this.pageSize = +q[1];
                                }
                            }
                        }
                    }
                }
            };
            ForumController.prototype.getForums = function () {
                var _this = this;
                this.forumService.getForums()
                    .then(function (result) {
                    _this.$scope.categories = result.data;
                    _this.$scope.postsLoaded = true;
                }, function (error) {
                    _this.$scope.validationMessage = error.statusText;
                });
            };
            ForumController.prototype.getForum = function (forumId, pageNumber, pageSize) {
                var _this = this;
                this.forumService.getForum(forumId, pageNumber > 0 ? pageNumber : this.pageNumber, pageSize > 0 ? pageSize : this.pageSize).then(function (result) {
                    _this.$scope.forum = result.data;
                    _this.$scope.postsLoaded = true;
                }, function (error) {
                    _this.$scope.validationMessage = error.data.exceptionMessage;
                });
            };
            ForumController.$inject = [
                "$scope",
                "forumService",
                "coreService",
                "$window"];
            return ForumController;
        })();
        forums.ForumController = ForumController;
        angular
            .module("zanshin")
            .controller("forumController", ForumController);
    })(forums = zanshin.forums || (zanshin.forums = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.forum.controller.js.map