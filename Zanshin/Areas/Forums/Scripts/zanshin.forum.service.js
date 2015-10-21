var zanshin;
(function (zanshin) {
    var forums;
    (function (forums) {
        "use strict";
        var ForumService = (function () {
            function ForumService($http, $q) {
                this.$http = $http;
                this.$q = $q;
                this.forumServiceUri = "/api/v1/forums";
                this.categoryServiceUri = "api/v1/categories";
            }
            ForumService.prototype.getForums = function () {
                var uri = this.categoryServiceUri;
                var deferred = this.$q.defer();
                return this.$http.get(uri).success(deferred.resolve)
                    .error(deferred.reject);
            };
            ForumService.prototype.getForum = function (forumId, pageNumber, pagesize) {
                var expand = "";
                if (pageNumber) {
                    expand += "?pageNumber=" + pageNumber;
                }
                if (pagesize) {
                    expand += "&pageSize=" + pagesize;
                }
                var uri = this.forumServiceUri + "/" + forumId + expand.trim();
                var deferred = this.$q.defer();
                return this.$http.get(uri).success(deferred.resolve)
                    .error(deferred.reject);
            };
            return ForumService;
        })();
        angular
            .module("zanshin")
            .service("forumService", ForumService);
    })(forums = zanshin.forums || (zanshin.forums = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.forum.service.js.map