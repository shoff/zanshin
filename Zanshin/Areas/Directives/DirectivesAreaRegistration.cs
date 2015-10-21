using System.Web.Mvc;

namespace Zanshin.Areas.Directives
{
    public class DirectivesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Directives";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Directives_default",
                url: "Directives/{controller}/{action}/{id}",
                defaults: new
                {
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "Zanshin.Areas.Directives.Controllers" });
        }
    }
}