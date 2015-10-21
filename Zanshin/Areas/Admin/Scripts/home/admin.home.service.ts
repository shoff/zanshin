import PrivateMessage = Zanshin.Domain.Entities.PrivateMessage;
import Log = Zanshin.Domain.Entities.Log;

// ReSharper disable once InconsistentNaming
module zanshin.admin {
    "use strict";

    export interface IHomeService {
        getMessages(): ng.IHttpPromise<Array<PrivateMessage>>;
        getLogs(): ng.IHttpPromise<Array<Log>>;
    }


    class HomeService implements IHomeService {
        messagesUrl = "/api/v1/messages";
        logsUrl = "/api/v1/logs";

        constructor(
            private $http: ng.IHttpService,
            private $q: ng.IQService) {
        }

        getMessages(): ng.IHttpPromise<Array<PrivateMessage>> {
            return this.$http.get(this.messagesUrl);
        }

        getLogs(): ng.IHttpPromise<Array<Log>> {
            return this.$http.get(this.logsUrl);
        }
        //addCategory(category: Category): ng.IPromise<Category> {
        //    var deferred = this.$q.defer();
        //    this.$http.post(this.serviceUri, category)
        //        .success(result => deferred.resolve(result))
        //        .error(deferred.reject);
        //    return deferred.promise;
        //}

        //createCategory(): Category {
        //    return <Category>{};
        //}
    }

    function factory($http: ng.IHttpService, $q: ng.IQService): HomeService {
        return new HomeService($http, $q);
    }

    factory.$inject = ["$http", "$q"];
    angular
        .module("zanshin.admin")
        .service("homeService", HomeService);
};  