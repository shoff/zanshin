// ReSharper disable once InconsistentNaming
var zanshin;
(function (zanshin) {
    var admin;
    (function (admin) {
        "use strict";
        var HomeService = (function () {
            function HomeService($http, $q) {
                this.$http = $http;
                this.$q = $q;
                this.messagesUrl = "/api/v1/messages";
                this.logsUrl = "/api/v1/logs";
            }
            HomeService.prototype.getMessages = function () {
                return this.$http.get(this.messagesUrl);
            };
            HomeService.prototype.getLogs = function () {
                return this.$http.get(this.logsUrl);
            };
            return HomeService;
        })();
        function factory($http, $q) {
            return new HomeService($http, $q);
        }
        factory.$inject = ["$http", "$q"];
        angular
            .module("zanshin.admin")
            .service("homeService", HomeService);
    })(admin = zanshin.admin || (zanshin.admin = {}));
})(zanshin || (zanshin = {}));
;
//# sourceMappingURL=admin.home.service.js.map