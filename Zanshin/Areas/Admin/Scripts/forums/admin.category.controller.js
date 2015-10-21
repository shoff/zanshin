var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var CategoryController = (function () {
            function CategoryController($scope, categoryService) {
                this.$scope = $scope;
                this.categoryService = categoryService;
                $scope.category = categoryService.createCategory();
                $scope.addCategory = function (category) {
                    category.dateCreated = new Date(Date.now());
                    category.lastUpdated = new Date(Date.now());
                    // go to the forum index - temp hack
                    window.location.replace("/Admin/Forums");
                };
                this.getCategories();
            }
            CategoryController.prototype.getCategories = function () {
                var _this = this;
                this.categoryService.getCategories().success(function (data) {
                    _this.$scope.categories = data;
                }).error(function (error) {
                    _this.$scope.validationMessage = error.exceptionMessage;
                });
            };
            CategoryController.$inject = ["$scope", "categoryService"];
            return CategoryController;
        })();
        admin.CategoryController = CategoryController;
        angular.module("zanshin.admin")
            .controller("categoryController", CategoryController);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=admin.category.controller.js.map