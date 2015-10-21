
// ReSharper disable once InconsistentNaming
module zanshin.admin {
    "use strict";
    export interface IForumService {
        getForums(): ng.IHttpPromise<Array<Forum>>;
        addForum(forum: Forum): ng.IPromise<Forum>;
        createForum(): Forum;
    }


    class ForumService implements IForumService {
        serviceUri = "/api/v1/forums";

        constructor(private $http: ng.IHttpService, private $q: ng.IQService) {
        }

        getForums(): ng.IHttpPromise<Array<Forum>> {
            return this.$http.get(this.serviceUri);
        }

        addForum(forum: Forum): ng.IPromise<Forum> {
            var deferred = this.$q.defer();
            this.$http.post(this.serviceUri, forum)
                .success(result => deferred.resolve(result))
                .error(deferred.reject);
            return deferred.promise;
        }

        createForum(): Forum {
            var forum = <Forum>{
                postsPerPage: 20,
                topicsPerPage: 20,
                hotTopicThreashold: 5
            }
            return forum;
        }
    }

    function factory($http: ng.IHttpService, $q: ng.IQService): ForumService {
        return new ForumService($http, $q);
    }

    factory.$inject = ["$http", "$q"];
    angular
        .module("zanshin.admin")
        .service("forumService", ForumService);
};