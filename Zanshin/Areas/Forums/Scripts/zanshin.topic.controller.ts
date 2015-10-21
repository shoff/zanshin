module zanshin.forums {
    "use strict";

    export interface ITopicScope extends ng.IScope {
        postsLoaded: boolean;
        postReply: Function;
        createTopic: Function;
        getTopics: Function;
        validationMessage: any;
        topics: Array<Topic>;
        topic: Topic;
        currentPage: string;
        
        // for editor
        postSubject: string;
        editorText: string;
        errorText: string[];
        addSmile: Function;
    }

    export class TopicController {

        queryString: string;
        topicId: number;
        pageNumber: number;
        pageSize: number;

        static $inject = ["$scope", "topicService", "coreService", "$window"];

        constructor(
            protected $scope: ITopicScope,
            protected topicService: ITopicService,
            protected coreService: zanshin.core.CoreService,
            protected $window: ng.IWindowService) {

            this.$scope.postsLoaded = false;
            this.$scope.postReply = () => {
                this.postReply();
            };

            this.$scope.getTopics = (forumId: string) => {
                this.getTopics(forumId);
            };

            this.$scope.addSmile = (smile: string, editorText: string) => {
                this.addSmile(smile, editorText);
            }

            this.init();
            this.getTopic(this.topicId, this.pageNumber, this.pageSize);
        }

        init(): void {
            this.$scope.errorText = [];
            this.$scope.editorText = "";
            this.$scope.currentPage = "1";
            if (this.$window.location.pathname) {
                var pathName = this.$window.location.pathname.split('/');
                var last = +pathName[pathName.length - 1];
                if (last !== NaN) {
                    this.topicId = last;
                } else {
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
        }


        addSmile(smile: string, editorText: string): void {
            this.$scope.editorText += smile;
        }

        postReply(): void {
            
            var post: Post = {
                postId: 0,
                subject: "blah blah blah",
                userId: 47, // temp hack
                poster: <User>{},
                postKarma: 0,
                topicId: 1,
                postTopic: <Topic>{},
                content: this.$scope.editorText,
                replyToPostId: 0,
                replyToPost: <Post>{},
                tags: {}[0],
                forumId: 1,
                dateCreated: new Date(Date.now()),
                lastUpdated: new Date(Date.now()),
            }
            this.topicService.replyToPost(post).success(data => {
                window.location.replace("/Forums/Topics/" + data.topicId);
            }).error(error => {
                for (var x in error.modelState) {
                    //this.$scope.validationMessage += " " + error.modelState[x];
                    this.$scope.errorText.push(error.modelState[x][0]);
                }


                //this.errors = <Array<Object>>error.modelState;
                //var arrayLength = this.errors.length;
                //for (var i = 0; i < arrayLength; i++) {
                //    this.$scope.validationMessage += " " + this.errors[i].valueOf;
                //}

            });
        }

        getTopic(topicId: number, pageNumber: number, pageSize:number) {
            this.topicService.getTopic(topicId,
                pageNumber > 0 ? pageNumber : this.pageNumber,
                pageSize > 0 ? pageSize : this.pageSize).then((result) => {
                    this.$scope.topic = result.data;
                    this.$scope.postSubject = 'RE: ' + this.$scope.topic.subject;
                    this.$scope.postsLoaded = true;
                }, (error) => {
                    this.$scope.validationMessage = error.data.exceptionMessage;
                    this.$scope.postsLoaded = true;
            });
        }

        getTopics(forumId: string) {
            this.topicService.getTopics(forumId).success(data => {
                this.$scope.topics = data.topics;
                this.$scope.postsLoaded = true;
            }).error(error => {
                this.$scope.validationMessage = error.exceptionMessage;
            });
        }
    }

    angular.module("zanshin")
        .controller("topicController", TopicController);
}
  