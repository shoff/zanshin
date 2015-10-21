// ReSharper disable once InconsistentNaming
var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var CategoryService = (function () {
            function CategoryService($http, $q) {
                this.$http = $http;
                this.$q = $q;
                this.serviceUri = "/api/v1/categories";
            }
            CategoryService.prototype.getCategories = function () {
                return this.$http.get(this.serviceUri);
            };
            CategoryService.prototype.addCategory = function (category) {
                var deferred = this.$q.defer();
                this.$http.post(this.serviceUri, category)
                    .success(function (result) { return deferred.resolve(result); })
                    .error(deferred.reject);
                return deferred.promise;
            };
            CategoryService.prototype.createCategory = function () {
                return {};
            };
            return CategoryService;
        })();
        function factory($http, $q) {
            return new CategoryService($http, $q);
        }
        factory.$inject = ["$http", "$q"];
        angular
            .module("zanshin.admin")
            .service("categoryService", CategoryService);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
;
//# sourceMappingURL=admin.category.service.js.map