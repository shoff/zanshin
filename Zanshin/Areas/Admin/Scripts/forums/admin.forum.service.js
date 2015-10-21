// ReSharper disable once InconsistentNaming
var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var ForumService = (function () {
            function ForumService($http, $q) {
                this.$http = $http;
                this.$q = $q;
                this.serviceUri = "/api/v1/forums";
            }
            ForumService.prototype.getForums = function () {
                return this.$http.get(this.serviceUri);
            };
            ForumService.prototype.addForum = function (forum) {
                var deferred = this.$q.defer();
                this.$http.post(this.serviceUri, forum)
                    .success(function (result) { return deferred.resolve(result); })
                    .error(deferred.reject);
                return deferred.promise;
            };
            ForumService.prototype.createForum = function () {
                var forum = {
                    postsPerPage: 20,
                    topicsPerPage: 20,
                    hotTopicThreashold: 5
                };
                return forum;
            };
            return ForumService;
        })();
        function factory($http, $q) {
            return new ForumService($http, $q);
        }
        factory.$inject = ["$http", "$q"];
        angular
            .module("zanshin.admin")
            .service("forumService", ForumService);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
;
//# sourceMappingURL=admin.forum.service.js.map