
module zanshin.admin {
    "use strict";

    interface Log { }

    interface IHomeScope extends ng.IScope {
        //addCategory: Function;
        validationMessage: any;
        messages: Array<PrivateMessage>;
        logs: Array<Log>;
        //category: Category;
    }

    class AdminHomeController {
        static $inject = ["$scope", "homeService"];

        constructor(
            private $scope: IHomeScope,
            private homeService: IHomeService) {

            //$scope.category = categoryService.createCategory();

            //$scope.addCategory = (category: Category) => {
            //    category.dateCreated = new Date(Date.now());
            //    category.lastUpdated = new Date(Date.now());

            // go to the forum index - temp hack
            //window.location.replace("/Admin/Forums");
            //};

            this.getMessages();
            this.getLogs();
        }

        getMessages() {
            this.homeService.getMessages().success(data => {
                this.$scope.messages = data;
            }).error(error => {
                this.$scope.validationMessage = error.exceptionMessage;
            });
        }

        getLogs() {
            this.homeService.getLogs().success(data => {
                this.$scope.logs = data;
            }).error(error => {
                this.$scope.validationMessage = error.exceptionMessage;
            });
        }
    }

    angular.module("zanshin.admin")
        .controller("adminHomeController", AdminHomeController);
} 