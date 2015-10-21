// import SerializablePagination = Zanshin.Domain.Collections.SerializablePagination
// ReSharper disable once InconsistentNaming
var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var UserService = (function () {
            function UserService($rootScope, $http, $q, $filter, $window) {
                this.$rootScope = $rootScope;
                this.$http = $http;
                this.$q = $q;
                this.$filter = $filter;
                this.$window = $window;
                this.serviceUri = "/api/v1/users";
            }
            UserService.prototype.getUsers = function (page) {
                if (page === undefined || page === null) {
                    page = 0;
                }
                return this.$http.get(this.serviceUri + "/paged/" + page);
            };
            UserService.prototype.getUser = function (userId) {
                return this.$http.get(this.serviceUri + "/" + userId);
            };
            UserService.prototype.addUser = function (user) {
                var deferred = this.$q.defer();
                this.$http.post(this.serviceUri, user)
                    .success(function (result) { return deferred.resolve(result); })
                    .error(deferred.reject);
                return deferred.promise;
            };
            UserService.prototype.updateUser = function (user) {
                var deferred = this.$q.defer();
                this.$http.put(this.serviceUri + "/" + user.id, user)
                    .success(function (result) { return deferred.resolve(result); })
                    .error(deferred.reject);
                return deferred.promise;
            };
            UserService.prototype.deleteUser = function (user) {
                var deferred = this.$q.defer();
                this.$http.delete(this.serviceUri + "/" + user.id, user)
                    .success(function (result) { return deferred.resolve(result); })
                    .error(deferred.reject);
                return deferred.promise;
            };
            UserService.prototype.createUser = function () {
                var user = {};
                return user;
            };
            UserService.$inject = ["$rootScope", "$http", "$q", "$filter", "$window"];
            return UserService;
        })();
        admin.UserService = UserService;
        angular.module("zanshin.admin").service("userService", UserService);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
;
//# sourceMappingURL=admin.users.service.js.map