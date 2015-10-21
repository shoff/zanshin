
import User = Zanshin.Domain.Entities.Identity.User;

module zanshin.user {
    "use strict";

    export interface IUserService {
        getUser(userId: number): ng.IHttpPromise<User>;
    }

    class UserService implements IUserService {
        private serviceUri: string = "/api/v1/users";

        constructor(private $http: ng.IHttpService, private $q: ng.IQService) { }

        getUser(userId: number): ng.IHttpPromise<User> {
            var uri = this.serviceUri + "/" + userId;
            var deferred = this.$q.defer();
            return this.$http.get(uri).success(deferred.resolve)
                .error(deferred.reject);
        }
    }
    angular
        .module("zanshin")
        .service("userService", UserService);
}