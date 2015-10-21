//import Group = Zanshin.Domain.Entities.Group;
// ReSharper disable once InconsistentNaming
var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var GroupService = (function () {
            function GroupService($http, $q) {
                this.$http = $http;
                this.$q = $q;
                this.serviceUri = "/api/v1/groups";
            }
            GroupService.prototype.getGroups = function () {
                return this.$http.get(this.serviceUri);
            };
            GroupService.prototype.addGroup = function (group) {
                var deferred = this.$q.defer();
                this.$http.post(this.serviceUri, group)
                    .success(function (result) { return deferred.resolve(result); })
                    .error(deferred.reject);
                return deferred.promise;
            };
            GroupService.prototype.addUserToGroup = function (groupId, userId, userType) {
                var deferred = this.$q.defer();
                if (userType === undefined) {
                    userType = "members";
                }
                var url = this.serviceUri + "/" + groupId + "/" + userType;
                this.$http.post(url, userId).success(function (result) {
                    //    this.$rootScope.$broadcast("showAddToCartPopup");
                    //    cartLine.availability = result.availability;
                    //    this.getCart();
                    deferred.resolve(result);
                }).error(function (error) {
                    //    this.getCart();
                    deferred.resolve(error);
                });
                return deferred.promise;
            };
            GroupService.prototype.createGroup = function () {
                var group = {
                    founderId: 0,
                    groupDescription: "",
                    displayGroupInLegend: true,
                    groupRecievePrivateMessages: false,
                    groupColor: "#000"
                };
                return group;
            };
            GroupService.$inject = ["$http", "$q"];
            return GroupService;
        })();
        //function factory($http: ng.IHttpService, $q: ng.IQService): GroupService {
        //    return new GroupService($http, $q);
        //}
        angular
            .module("zanshin.admin")
            .service("groupService", GroupService);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
;
//# sourceMappingURL=admin.group.service.js.map