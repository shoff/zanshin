import Category = Zanshin.Domain.Entities.Forum.Category;
import Forum = Zanshin.Domain.Entities.Forum.Forum;
import Tag = Zanshin.Domain.Entities.Tag;

// ReSharper disable once InconsistentNaming

module zanshin.admin {
    "use strict";
    export interface ICategoryService {
        getCategories(): ng.IHttpPromise<Array<Category>>;
        addCategory(category: Category): ng.IPromise<Category>;
        createCategory(): Category;
    }


    class CategoryService implements ICategoryService {
        serviceUri = "/api/v1/categories";

        constructor(private $http: ng.IHttpService, private $q: ng.IQService) {
        }

        getCategories(): ng.IHttpPromise<Array<Category>> {
            return this.$http.get(this.serviceUri);
        }

        addCategory(category: Category): ng.IPromise<Category> {
            var deferred = this.$q.defer();
            this.$http.post(this.serviceUri, category)
                .success(result => deferred.resolve(result))
                .error(deferred.reject);
            return deferred.promise;
        }

        createCategory(): Category {
            return <Category>{};
        }
    }

    function factory($http: ng.IHttpService, $q: ng.IQService): CategoryService {
        return new CategoryService($http, $q);
    }

    factory.$inject = ["$http", "$q"];
    angular
        .module("zanshin.admin")
        .service("categoryService", CategoryService);
}; 