
// import SerializablePagination = Zanshin.Domain.Collections.SerializablePagination

// ReSharper disable once InconsistentNaming
module zanshin.admin {
    "use strict";

    export interface IUserService {
        getUsers(page: number): ng.IHttpPromise<Zanshin.Domain.Collections.SerializablePagination<User>>;
        getUser(id: number): ng.IHttpPromise<User>;
        addUser(u: User): ng.IPromise<User>;
        updateUser(u: User): ng.IPromise<boolean>;
        deleteUser(u: User): ng.IPromise<boolean>;
    }


    export class UserService implements IUserService {

        serviceUri = "/api/v1/users";
        static $inject = ["$rootScope", "$http", "$q", "$filter", "$window"];

        constructor(protected $rootScope: ng.IRootScopeService,
            protected $http: ng.IHttpService,
            protected $q: ng.IQService,
            protected $filter: ng.IFilterService,
            protected $window: ng.IWindowService) {
        }

        getUsers(page: number): ng.IHttpPromise<Zanshin.Domain.Collections.SerializablePagination<User>> {
            if (page === undefined || page === null) {
                page = 0;
            }
            return this.$http.get(this.serviceUri +"/paged/" + page);
        }

        getUser(userId: number): ng.IHttpPromise<User> {
            return this.$http.get(this.serviceUri + "/" + userId);
        }

        addUser(user: User): ng.IPromise<User> {
            var deferred = this.$q.defer();
            this.$http.post(this.serviceUri, user)
                .success(result => deferred.resolve(result))
                .error(deferred.reject);
            return deferred.promise;
        }

        updateUser(user: User): ng.IPromise<boolean> {
            var deferred = this.$q.defer();
            this.$http.put(this.serviceUri + "/" + user.id, user)
                .success(result => deferred.resolve(result))
                .error(deferred.reject);
            return deferred.promise;
        }

        deleteUser(user: User): ng.IPromise<boolean> {
            var deferred = this.$q.defer();
            this.$http.delete(this.serviceUri + "/" + user.id, user)
                .success(result => deferred.resolve(result))
                .error(deferred.reject);
            return deferred.promise;
        }

        createUser(): User {
            var user = <User>{}
            return user;
        }
    }
    angular.module("zanshin.admin").service("userService", UserService);
}; 