
module zanshin.forums {
    "use strict";

    export interface IForumService {
        getForums(): ng.IHttpPromise<Array<Category>>;
        getForum(forumId: number, pageNumber: number, pagesize: number): ng.IHttpPromise<Forum>;
    }

    class ForumService implements IForumService {
        private forumServiceUri: string = "/api/v1/forums";
        private categoryServiceUri: string = "api/v1/categories";


        constructor(
            private $http: ng.IHttpService,
            private $q: ng.IQService) {
        }

        getForums(): ng.IHttpPromise<Array<Category>> {
            var uri = this.categoryServiceUri;
            var deferred = this.$q.defer();
            return this.$http.get(uri).success(deferred.resolve)
                .error(deferred.reject);
        }

        getForum(forumId: number, pageNumber: number, pagesize: number): ng.IHttpPromise<Forum> {

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
        }
    }
    angular
        .module("zanshin")
        .service("forumService", ForumService);
} 