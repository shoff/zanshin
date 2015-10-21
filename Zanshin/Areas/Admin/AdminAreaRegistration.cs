using System.Web.Mvc;

namespace Zanshin.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Admin_forum_details",
                url: "Admin/forums/{id}",
                defaults: new { controller = "Forums", action = "Details" },
                namespaces: new[] { "Zanshin.Areas.Admin.Controllers" });
    
            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Zanshin.Areas.Admin.Controllers" });
        }
    }
}