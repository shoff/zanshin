
module zanshin.admin {
    "use strict";

    interface ICategoryScope extends ng.IScope {
        addCategory: Function;
        validationMessage: any;
        categories: Array<Category>;
        category: Category;
    }

    export class CategoryController {
        static $inject = ["$scope", "categoryService"];

        constructor(private $scope: ICategoryScope,
            private categoryService: ICategoryService) {

            $scope.category = categoryService.createCategory();

            $scope.addCategory = (category: Category) => {
                category.dateCreated = new Date(Date.now());
                category.lastUpdated = new Date(Date.now());

                // go to the forum index - temp hack
                window.location.replace("/Admin/Forums");
            };

            this.getCategories();
        }

        getCategories() {
            this.categoryService.getCategories().success(data => {
                this.$scope.categories = data;
            }).error(error => {
                this.$scope.validationMessage = error.exceptionMessage;
            });
        }

    }

    angular.module("zanshin.admin")
        .controller("categoryController", CategoryController);
} 