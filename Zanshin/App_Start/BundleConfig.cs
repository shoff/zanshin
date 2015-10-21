using System.Web.Optimization;

namespace Zanshin
{
    /// <summary>
    /// </summary>
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/libraries/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/libraries/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/libraries/bootstrap/bootstrap.min.js",
                      "~/Scripts/respond.js"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Scripts/libraries/font-awesome/css/font-awesome.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/flatly").Include(
                    "~/Content/bootstrap.min.css",
                    //"~/Content/bootstrap-theme.min.css",
                    "~/Content/bootstrap-flatly.min.css",
                    //"~/Scripts/libraries/font-awesome/css/font-awesome.min.css",
                    "~/Content/site.css"));

            // Angular Zanshin
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/libraries/angular/angular.js",
                "~/Scripts/libraries/angular/angular-route.min.js",
                "~/Scripts/libraries/angular-filter/0.5.4/angular-filter.min.js",
                "~/Scripts/zanshin.module.js",
                "~/Scripts/zanshin.coreservice.js"
                ));

            // Angular directives
            bundles.Add(new ScriptBundle("~/bundles/directives").Include(
                "~/Areas/Directives/Scripts/breadcrumb/zanshin.breadcrumb.controller.js",
                "~/Areas/Directives/Scripts/breadcrumb/zanshin.breadcrumb.directives.js"
                ));


            // Forums
            bundles.Add(new ScriptBundle("~/bundles/forums").Include(
                "~/Areas/Forums/Scripts/zanshin.forum.service.js",
                "~/Areas/Forums/Scripts/zanshin.category.controller.js",
                "~/Areas/Forums/Scripts/zanshin.forum.controller.js",
                "~/Areas/Forums/Scripts/zanshin.topic.service.js",
                "~/Areas/Forums/Scripts/zanshin.topic.controller.js"
                ));

            bundles.Add(new StyleBundle("~/Content/forums-css").Include(
                "~/Areas/Forums/Content/forums.css"));


            // Admin
            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Areas/Admin/Scripts/zanshin.admin.js",
                "~/Areas/Admin/Scripts/home/admin.home.service.js",
                "~/Areas/Admin/Scripts/home/admin.home.controller.js",
                "~/Areas/Admin/Scripts/forums/admin.category.service.js",
                "~/Areas/Admin/Scripts/forums/admin.category.controller.js",
                "~/Areas/Admin/Scripts/forums/admin.forum.service.js",
                "~/Areas/Admin/Scripts/forums/admin.forum.controller.js",
                "~/Areas/Admin/Scripts/groups/admin.group.service.js",
                "~/Areas/Admin/Scripts/groups/admin.group.controller.js",
                "~/Areas/Admin/Scripts/users/admin.users.service.js",
                "~/Areas/Admin/Scripts/users/admin.users.controller.js"
                ));

            bundles.Add(new StyleBundle("~/Content/admin-css").Include(
                "~/Areas/Admin/Content/bootstrap.min.css",
                "~/Scripts/libraries/font-awesome/css/font-awesome.min.css",
                "~/Areas/Admin/Content/styles.css"));

            // TextAngular
            bundles.Add(new ScriptBundle("~/bundles/text-angular").Include(
                "~/Scripts/libraries/text-angular/textAngular-rangy.min.js",
                "~/Scripts/libraries/text-angular/textAngular-sanitize.min.js",
                "~/Scripts/libraries/text-angular/textAngular.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/text-angular").Include(
                    "~/Scripts/libraries/text-angular/textAngular.css"
                ));

            // User
            bundles.Add(new ScriptBundle("~/bundles/user").Include(
                "~/Areas/User/Scripts/zanshin.user.js"
             ));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
