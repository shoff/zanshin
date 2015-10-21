var zanshin;
(function (zanshin) {
    var forums;
    (function (forums) {
        "use strict";
        var TopicService = (function () {
            function TopicService($http, $q) {
                this.$http = $http;
                this.$q = $q;
                this.postServiceUri = "/api/v1/posts";
                this.topicServiceUri = "/api/v1/topics";
                this.forumServiceUri = "/api/v1/forums";
            }
            TopicService.prototype.getTopics = function (forumId) {
                var uri = this.forumServiceUri + "/ " + forumId;
                var deferred = this.$q.defer();
                return this.$http.get(uri).success(deferred.resolve)
                    .error(deferred.reject);
            };
            // get the posts for a specific topicid
            TopicService.prototype.getTopic = function (topicId, pageNumber, pageSize) {
                var expand = "";
                if (pageNumber) {
                    expand += "?pageNumber=" + pageNumber;
                }
                if (pageSize) {
                    expand += "&pageSize=" + pageSize;
                }
                var uri = this.topicServiceUri + "/" + topicId + "/posts" + expand.trim();
                var deferred = this.$q.defer();
                return this.$http.get(uri).success(deferred.resolve)
                    .error(deferred.reject);
            };
            // Reply for posts.
            TopicService.prototype.replyToPost = function (post) {
                var deferred = this.$q.defer();
                return this.$http.post(this.postServiceUri, post).success(deferred.resolve)
                    .error(deferred.reject);
            };
            return TopicService;
        })();
        forums.TopicService = TopicService;
        angular
            .module("zanshin")
            .service("topicService", TopicService);
    })(forums = zanshin.forums || (zanshin.forums = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.topic.service.js.map