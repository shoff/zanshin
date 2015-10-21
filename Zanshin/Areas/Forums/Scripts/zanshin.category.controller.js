var zanshin;
(function (zanshin) {
    var forums;
    (function (forums) {
        "use strict";
        // TODO split this into categories controller and forum controller
        var CategoryController = (function () {
            function CategoryController($scope, forumService, coreService, $window) {
                this.$scope = $scope;
                this.forumService = forumService;
                this.coreService = coreService;
                this.$window = $window;
                this.$scope.forumCount = 0;
                this.getForums();
            }
            CategoryController.prototype.getForums = function () {
                var _this = this;
                this.forumService.getForums()
                    .then(function (result) {
                    _this.$scope.categories = result.data;
                    for (var i = 0; i < _this.$scope.categories.length; i++) {
                        _this.$scope.forumCount += _this.$scope.categories[i].forums.length;
                    }
                }, function (error) {
                    _this.$scope.validation = error.statusText;
                });
            };
            CategoryController.$inject = [
                "$scope",
                "forumService",
                "coreService",
                "$window"];
            return CategoryController;
        })();
        forums.CategoryController = CategoryController;
        angular
            .module("zanshin")
            .controller("categoryController", CategoryController);
    })(forums = zanshin.forums || (zanshin.forums = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.category.controller.js.map