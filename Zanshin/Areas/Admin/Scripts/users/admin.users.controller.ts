
// ReSharper disable once InconsistentNaming

module zanshin.admin {
    "use strict";

    export interface IUserScope extends ng.IScope {
        users: Array<User>;
        user: User;
        addUser: Function;
        deleteUser: Function;
        updateUser: Function;
        validationMessage: any;
    }

    export class AdminUsersController {

        users: Array<User>;
        user: User;
        addUser: Function;
        deleteUser: Function;
        updateUser: Function;
        validationMessage: any;


        static $inject = ["$scope", "userService"];

        constructor(
            protected $scope: IUserScope,
            protected userService: IUserService) {

            this.getUsers();
        }

        getUsers(): void {
            this.userService.getUsers(0).success(data => {
                this.$scope.users = data.currentPage;
            }).error(error => {
                this.$scope.validationMessage = error.exceptionMessage;
            });
        }
        
    }

    angular.module("zanshin.admin")
        .controller("adminUsersController", AdminUsersController);
} 