

module zanshin.ckeditor {
    "use strict";


    export interface ICKEditorScope extends ng.IScope {
        editorText: string;
        editorOptions: any;
        addSmile: Function;
    }
     
    declare var CKEDITOR;

    export class CKEditorController {

        static $inject = ["$scope"];
        constructor(
            protected $scope: ICKEditorScope) {
            this.init();
        }

        init(): void {
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

            this.$scope.addSmile = (smile: string, editorText: string)=>{
                editorText += smile;
            }
            console.log(this.$scope);
        }
    }
    angular.module("zanshin").controller("CKEditorController", CKEditorController);
}