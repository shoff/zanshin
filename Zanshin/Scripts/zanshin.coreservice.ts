module zanshin.core {
    "use strict";

    export interface ICoreService {
        parseParameters(parameters: {}): string;
        getObjectByPropertyValue<T>(values: T[], expr: {}): T;
        queryString(a: string[]): { [key: string]: string; };
        getQueryStringParameter(key: string, ignoreCase?: boolean);
        getQueryStringCollection(): { [key: string]: string; } ;
        refreshUiBindings(): void;
    }

    export class CoreService implements ICoreService {

        static $inject = ["$rootScope", "$http", "$q", "$filter", "$window"];

        constructor(
            protected $rootScope: ng.IRootScopeService,
            protected $http: ng.IHttpService,
            protected $q: ng.IQService,
            protected $filter: ng.IFilterService,
            protected $window: ng.IWindowService) {
        }

        // turns an object into a "&" separated list of URL key values
        parseParameters(parameters: { [key: string]: any }): string {
            var query = "";
            for (var property in parameters) {
                if (parameters.hasOwnProperty(property)) {
                    if (parameters[property] && parameters[property].constructor === Array) {
                        angular.forEach(parameters[property], function (value) {
                            query += property + "=" + value + "&";
                        });
                    } else {
                        query += property + "=" + parameters[property] + "&";
                    }
                }
            }
            return query;
        }
        
        //example: coreService.getObjectByPropertyValue(section.options, { selected: "true" })        
        getObjectByPropertyValue<T>(values: T[], expr: {}): T {
            var filteredFields = this.$filter("filter")(values, expr);
            return filteredFields ? filteredFields[0] : null;
        }

        queryString(a: string[]): { [key:string]:string; } {
            if (!a) {
                 return {};
            }
            var b: { [key: string]: string; } = {};
            for (var i = 0; i < a.length; ++i) {
                var p = a[i].split("=");
                if (p.length != 2) {
                    continue;
                }
                b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
            }
            return b;
        }

        getQueryStringParameter(key: string, ignoreCase?: boolean) {
            key = key.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + key + "=([^&#]*)", ignoreCase ? "i" : undefined)
                , results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        getQueryStringCollection(): { [key: string]: string; }  {
            return this.queryString(this.$window.location.search.substr(1).split("&"));
        }

        refreshUiBindings(): void {
            (<any>$(document)).foundation({ bindings: "events" });
        }

    }
    angular
        .module("zanshin")
        .service("coreService", CoreService);
}  