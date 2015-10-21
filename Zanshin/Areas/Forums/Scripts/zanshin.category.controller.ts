module zanshin.forums {
    "use strict";

    export interface ICategoryScope extends ng.IScope {
        validation: string;
        forums: Array<Forum>;
        categories: Array<Category>;
        forumCount: number;
    }

    // TODO split this into categories controller and forum controller
    export class CategoryController {
        
        static $inject = [
            "$scope",
            "forumService",
            "coreService",
            "$window"];

        constructor(
            protected $scope: ICategoryScope,
            protected forumService: forums.IForumService,
            protected coreService: core.ICoreService,
            protected $window: ng.IWindowService) {
            this.$scope.forumCount = 0;
            this.getForums();
        }

        getForums() {
            this.forumService.getForums()
                .then((result) => {
                    this.$scope.categories = result.data;
                    for (var i = 0; i < this.$scope.categories.length; i++) {
                        this.$scope.forumCount += this.$scope.categories[i].forums.length;
                    }
                }, (error) => {
                    this.$scope.validation = error.statusText;
                });
        }
    }
    angular
        .module("zanshin")
        .controller("categoryController", CategoryController);
}