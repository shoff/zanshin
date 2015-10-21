var zanshin;
(function (zanshin) {
    var user;
    (function (user) {
        "use strict";
        var UserService = (function () {
            function UserService($http, $q) {
                this.$http = $http;
                this.$q = $q;
                this.serviceUri = "/api/v1/users";
            }
            UserService.prototype.getUser = function (userId) {
                var uri = this.serviceUri + "/" + userId;
                var deferred = this.$q.defer();
                return this.$http.get(uri).success(deferred.resolve)
                    .error(deferred.reject);
            };
            return UserService;
        })();
        angular
            .module("zanshin")
            .service("userService", UserService);
    })(user = zanshin.user || (zanshin.user = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.user.service.js.map