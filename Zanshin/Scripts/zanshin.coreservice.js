var zanshin;
(function (zanshin) {
    var core;
    (function (core) {
        "use strict";
        var CoreService = (function () {
            function CoreService($rootScope, $http, $q, $filter, $window) {
                this.$rootScope = $rootScope;
                this.$http = $http;
                this.$q = $q;
                this.$filter = $filter;
                this.$window = $window;
            }
            // turns an object into a "&" separated list of URL key values
            CoreService.prototype.parseParameters = function (parameters) {
                var query = "";
                for (var property in parameters) {
                    if (parameters.hasOwnProperty(property)) {
                        if (parameters[property] && parameters[property].constructor === Array) {
                            angular.forEach(parameters[property], function (value) {
                                query += property + "=" + value + "&";
                            });
                        }
                        else {
                            query += property + "=" + parameters[property] + "&";
                        }
                    }
                }
                return query;
            };
            //example: coreService.getObjectByPropertyValue(section.options, { selected: "true" })        
            CoreService.prototype.getObjectByPropertyValue = function (values, expr) {
                var filteredFields = this.$filter("filter")(values, expr);
                return filteredFields ? filteredFields[0] : null;
            };
            CoreService.prototype.queryString = function (a) {
                if (!a) {
                    return {};
                }
                var b = {};
                for (var i = 0; i < a.length; ++i) {
                    var p = a[i].split("=");
                    if (p.length != 2) {
                        continue;
                    }
                    b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
                }
                return b;
            };
            CoreService.prototype.getQueryStringParameter = function (key, ignoreCase) {
                key = key.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + key + "=([^&#]*)", ignoreCase ? "i" : undefined), results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            };
            CoreService.prototype.getQueryStringCollection = function () {
                return this.queryString(this.$window.location.search.substr(1).split("&"));
            };
            CoreService.prototype.refreshUiBindings = function () {
                $(document).foundation({ bindings: "events" });
            };
            CoreService.$inject = ["$rootScope", "$http", "$q", "$filter", "$window"];
            return CoreService;
        })();
        core.CoreService = CoreService;
        angular
            .module("zanshin")
            .service("coreService", CoreService);
    })(core = zanshin.core || (zanshin.core = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.coreservice.js.map