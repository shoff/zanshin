import Topic = Zanshin.Domain.Entities.Forum.Topic;
import Post = Zanshin.Domain.Entities.Forum.Post;

module zanshin.forums {
    "use strict";

    export interface ITopicService {
        getTopics(forumId: string): ng.IHttpPromise<Forum>;
        getTopic(topicId: number, pageNumber:number, pageSize:number): ng.IHttpPromise<Topic>;
        replyToPost(post: Post): ng.IHttpPromise<Topic>;
    }

    export class TopicService implements ITopicService {

        protected postServiceUri: string = "/api/v1/posts";
        protected topicServiceUri: string = "/api/v1/topics";
        protected forumServiceUri: string = "/api/v1/forums";

        constructor(
            protected $http: ng.IHttpService,
            protected $q: ng.IQService) {
        }

        getTopics(forumId: string): ng.IHttpPromise<Forum> {
            var uri = this.forumServiceUri + "/ " + forumId;
            var deferred = this.$q.defer();
            return this.$http.get(uri).success(deferred.resolve)
                .error(deferred.reject);
        }

        // get the posts for a specific topicid
        getTopic(topicId: number, pageNumber: number, pageSize: number): ng.IHttpPromise<Topic> {

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
        }
        
        // Reply for posts.
        replyToPost(post: Post): ng.IHttpPromise<Topic> {
            var deferred = this.$q.defer();
            return this.$http.post(this.postServiceUri, post).success(deferred.resolve)
                .error(deferred.reject);
        }

    }
    angular
        .module("zanshin")
        .service("topicService", TopicService);
}  