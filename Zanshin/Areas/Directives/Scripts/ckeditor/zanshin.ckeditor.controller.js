var zanshin;
(function (zanshin) {
    var ckeditor;
    (function (ckeditor) {
        "use strict";
        var CKEditorController = (function () {
            function CKEditorController($scope) {
                this.$scope = $scope;
                this.init();
            }
            CKEditorController.prototype.init = function () {
                this.$scope.editorText = "";
                this.$scope.editorOptions = {
                    language: 'en',
                    enterMode: CKEDITOR.ENTER_BR,
                    //skin: 'kama',
                    toolbarLocation: 'top',
                    toolbar: 'full',
                    toolbar_full: [
                        { name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', 'Underline'] },
                        { name: 'paragraph', items: ['BulletedList', 'NumberedList', 'Blockquote'] },
                        { name: 'editing', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                        { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
                        { name: 'tools', items: ['SpellChecker', 'Maximize'] },
                        { name: 'clipboard', items: ['Undo', 'Redo'] },
                        { name: 'styles', items: ['Format', 'FontSize', 'TextColor', 'PasteText', 'PasteFromWord', 'RemoveFormat'] },
                        { name: 'insert', items: ['Image', 'Table', 'SpecialChar', 'MediaEmbed'] }, '/',
                    ],
                };
                this.$scope.addSmile = function (smile, editorText) {
                    editorText += smile;
                };
                console.log(this.$scope);
            };
            CKEditorController.$inject = ["$scope"];
            return CKEditorController;
        })();
        ckeditor.CKEditorController = CKEditorController;
        angular.module("zanshin").controller("CKEditorController", CKEditorController);
    })(ckeditor = zanshin.ckeditor || (zanshin.ckeditor = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.ckeditor.controller.js.map