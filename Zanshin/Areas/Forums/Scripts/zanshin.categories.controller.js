var zanshin;
(function (zanshin) {
    var forums;
    (function (forums) {
        "use strict";
        var CategoriesController = (function () {
            function CategoriesController($scope, forumService) {
                this.$scope = $scope;
                this.forumService = forumService;
                console.log(this.$scope);
                this.getCategories();
            }
            CategoriesController.prototype.getCategories = function () {
                return null;
            };
            CategoriesController.$inject = ["$scope", "forumService"];
            return CategoriesController;
        })();
        forums.CategoriesController = CategoriesController;
        angular.module("zanshin")
            .controller("categoriesController", CategoriesController);
    })(forums = zanshin.forums || (zanshin.forums = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.categories.controller.js.map