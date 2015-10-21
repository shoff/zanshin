module zanshin.ckeditor {
    "use strict";

    angular.module("zanshin")
        .directive("zanBasicEditor", [function () {

        var directive: ng.IDirective = {
            restrict: 'E',
            scope: {
                editorText: '='
            },
            templateUrl: "/Directives/TextEditor/BasicEditor",
            controller: "CKEditorController"
        }
        // now return the directive
        return directive;
    }]);
}