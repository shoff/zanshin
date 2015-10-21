var zanshin;
(function (zanshin) {
    var forums;
    (function (forums) {
        "use strict";
        var TopicController = (function () {
            function TopicController($scope, topicService, coreService, $window) {
                var _this = this;
                this.$scope = $scope;
                this.topicService = topicService;
                this.coreService = coreService;
                this.$window = $window;
                this.$scope.postsLoaded = false;
                this.$scope.postReply = function () {
                    _this.postReply();
                };
                this.$scope.getTopics = function (forumId) {
                    _this.getTopics(forumId);
                };
                this.$scope.addSmile = function (smile, editorText) {
                    _this.addSmile(smile, editorText);
                };
                this.init();
                this.getTopic(this.topicId, this.pageNumber, this.pageSize);
            }
            TopicController.prototype.init = function () {
                this.$scope.errorText = [];
                this.$scope.editorText = "";
                this.$scope.currentPage = "1";
                if (this.$window.location.pathname) {
                    var pathName = this.$window.location.pathname.split('/');
                    var last = +pathName[pathName.length - 1];
                    if (last !== NaN) {
                        this.topicId = last;
                    }
                    else {
                        // todo
                        this.$window.location.assign("/404");
                    }
                }
                // more hacking
                if (this.$window.location.search) {
                    //?pageNumber=3
                    var expand = this.$window.location.search.split('&');
                    for (var e = 0; e < expand.length; e++) {
                        var pn = expand[0].slice(1);
                        if (pn) {
                            var q = pn.split('=');
                            if (q.length === 2) {
                                if (q[0].toLowerCase() === "pagenumber") {
                                    this.pageNumber = +q[1];
                                    this.$scope.currentPage = this.pageNumber.toString();
                                }
                                else if (q[0].toLowerCase() === "pagesize") {
                                    this.pageSize = +q[1];
                                }
                            }
                        }
                    }
                }
            };
            TopicController.prototype.addSmile = function (smile, editorText) {
                this.$scope.editorText += smile;
            };
            TopicController.prototype.postReply = function () {
                var _this = this;
                var post = {
                    postId: 0,
                    subject: "blah blah blah",
                    userId: 47,
                    poster: {},
                    postKarma: 0,
                    topicId: 1,
                    postTopic: {},
                    content: this.$scope.editorText,
                    replyToPostId: 0,
                    replyToPost: {},
                    tags: {}[0],
                    forumId: 1,
                    dateCreated: new Date(Date.now()),
                    lastUpdated: new Date(Date.now()),
                };
                this.topicService.replyToPost(post).success(function (data) {
                    window.location.replace("/Forums/Topics/" + data.topicId);
                }).error(function (error) {
                    for (var x in error.modelState) {
                        //this.$scope.validationMessage += " " + error.modelState[x];
                        _this.$scope.errorText.push(error.modelState[x][0]);
                    }
                    //this.errors = <Array<Object>>error.modelState;
                    //var arrayLength = this.errors.length;
                    //for (var i = 0; i < arrayLength; i++) {
                    //    this.$scope.validationMessage += " " + this.errors[i].valueOf;
                    //}
                });
            };
            TopicController.prototype.getTopic = function (topicId, pageNumber, pageSize) {
                var _this = this;
                this.topicService.getTopic(topicId, pageNumber > 0 ? pageNumber : this.pageNumber, pageSize > 0 ? pageSize : this.pageSize).then(function (result) {
                    _this.$scope.topic = result.data;
                    _this.$scope.postSubject = 'RE: ' + _this.$scope.topic.subject;
                    _this.$scope.postsLoaded = true;
                }, function (error) {
                    _this.$scope.validationMessage = error.data.exceptionMessage;
                    _this.$scope.postsLoaded = true;
                });
            };
            TopicController.prototype.getTopics = function (forumId) {
                var _this = this;
                this.topicService.getTopics(forumId).success(function (data) {
                    _this.$scope.topics = data.topics;
                    _this.$scope.postsLoaded = true;
                }).error(function (error) {
                    _this.$scope.validationMessage = error.exceptionMessage;
                });
            };
            TopicController.$inject = ["$scope", "topicService", "coreService", "$window"];
            return TopicController;
        })();
        forums.TopicController = TopicController;
        angular.module("zanshin")
            .controller("topicController", TopicController);
    })(forums = zanshin.forums || (zanshin.forums = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.topic.controller.js.map