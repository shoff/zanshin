var zanshin;
(function (zanshin) {
    var ckeditor;
    (function (ckeditor) {
        "use strict";
        angular.module("zanshin")
            .directive("zanBasicEditor", [function () {
                var directive = {
                    restrict: 'E',
                    scope: {
                        editorText: '='
                    },
                    templateUrl: "/Directives/TextEditor/BasicEditor",
                    controller: "CKEditorController"
                };
                // now return the directive
                return directive;
            }]);
    })(ckeditor = zanshin.ckeditor || (zanshin.ckeditor = {}));
})(zanshin || (zanshin = {}));
//# sourceMappingURL=zanshin.ckeditor.directives.js.map