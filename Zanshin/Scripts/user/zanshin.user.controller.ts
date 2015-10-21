

module zanshin.user {
    "use strict";

    interface IUserScope extends ng.IScope {
        user: User;
        validation: string;
    }


    class UserController {

        static $inject = ["$scope", "userService"];
        constructor(
            private $scope: IUserScope,
            private userService: user.IUserService) {
        }

        getUser(userId: number) {
            this.userService.getUser(userId).then((result) => {
                this.$scope.user = result.data;
            },
                (error) => {
                    this.$scope.validation = error.statusText;
            })
        }

    }
    angular
        .module("zanshin")
        .controller("UserController", UserController);
}