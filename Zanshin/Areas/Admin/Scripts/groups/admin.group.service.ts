
//import Group = Zanshin.Domain.Entities.Group;

interface Group {
    founderId: number;
}
// ReSharper disable once InconsistentNaming
module zanshin.admin {
    "use strict";
    export interface IGroupService {
        getGroups(): ng.IHttpPromise<Array<Group>>;
        addGroup(forum: Group): ng.IPromise<Group>;
        createGroup(): Group;
    }


    class GroupService implements GroupService {
        serviceUri = "/api/v1/groups";
        static $inject = ["$http", "$q"];


        constructor(private $http: ng.IHttpService, private $q: ng.IQService) {
        }

        getGroups(): ng.IHttpPromise<Array<Group>> {
            return this.$http.get(this.serviceUri);
        }

        addGroup(group: Group): ng.IPromise<Group> {
            var deferred = this.$q.defer();
            this.$http.post(this.serviceUri, group)
                .success(result => deferred.resolve(result))
                .error(deferred.reject);
            return deferred.promise;
        }

        addUserToGroup(groupId: number, userId: number, userType: string): ng.IPromise<Array<User>> {

            var deferred = this.$q.defer();
            if (userType === undefined) {

                userType = "members";
            }

            var url = this.serviceUri + "/" + groupId + "/" + userType;

            this.$http.post(url, userId).success((result: Array<User>) => {
                //    this.$rootScope.$broadcast("showAddToCartPopup");
                //    cartLine.availability = result.availability;
                //    this.getCart();
                deferred.resolve(result);
            }).error(error => {
                //    this.getCart();
                deferred.resolve(error);
            });
            return deferred.promise;
        }


        createGroup(): Group {

            var group = <Group>{
                founderId: 0, // todo
                groupDescription: "",
                displayGroupInLegend: true,
                groupRecievePrivateMessages: false,
                groupColor: "#000"
            };
            return group;
        }


    }

    //function factory($http: ng.IHttpService, $q: ng.IQService): GroupService {
    //    return new GroupService($http, $q);
    //}

    angular
        .module("zanshin.admin")
        .service("groupService", GroupService);
};