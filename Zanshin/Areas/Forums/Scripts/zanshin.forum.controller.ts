module zanshin.forums {
    "use strict";

    export interface IForumScope extends ng.IScope {
        postsLoaded: boolean;
        validationMessage: any;
        forums: Array<Forum>;
        categories: Array<Category>;
        forum: Forum;
        topicCount: number;
        currentPage: string;
    }

    // TODO split this into categories controller and forum controller
    export class ForumController {

        forumId: number;
        pageNumber: number;
        pageSize: number;

        static $inject = [
            "$scope",
            "forumService",
            "coreService",
            "$window"];

        constructor(
            protected $scope: IForumScope,
            protected forumService: forums.IForumService,
            protected coreService: zanshin.core.ICoreService,
            protected $window: ng.IWindowService) {
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
            } else {
                this.getForums();
            }
        }

        init():void {
            if (this.$window.location.pathname) {
                var pathName = this.$window.location.pathname.split('/');
                var last = +pathName[pathName.length - 1];
                if (last !== NaN) {
                    this.forumId = last;
                } else {
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
                            } else if (q[0].toLowerCase() === "pagesize") {
                                this.pageSize = +q[1];
                            }
                        } 
                    }
                }
            }
        }

        getForums() {
            this.forumService.getForums()
                .then((result) => {
                    this.$scope.categories = result.data;
                    this.$scope.postsLoaded = true;
                }, (error) => {
                    this.$scope.validationMessage = error.statusText;
                });
        }

        getForum(forumId: number, pageNumber: number, pageSize: number) {

            this.forumService.getForum(forumId,
                pageNumber > 0 ? pageNumber : this.pageNumber,
                pageSize > 0 ? pageSize : this.pageSize).then((result) => {
                    this.$scope.forum = result.data;
                    this.$scope.postsLoaded = true;
                }, (error) => {
                    this.$scope.validationMessage = error.data.exceptionMessage;
            });
        }

    }
    angular
        .module("zanshin")
        .controller("forumController", ForumController);
}