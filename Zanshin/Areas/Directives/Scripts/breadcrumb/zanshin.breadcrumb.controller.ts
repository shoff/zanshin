
module zanshin.breadcrumb {
    "use strict";

    export interface IBreadCrumb extends ng.IScope {
    }

    export class BreadCrumbController {

        static $inject = ["$scope"];


        constructor(
            protected $scope: IBreadCrumb) {
            this.init();
        }

        init(): void {
            // TODO
        }
    }

    angular
        .module("zanshin")
        .controller("BreadCrumbController", BreadCrumbController);
} 