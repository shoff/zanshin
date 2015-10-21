using System.Web.Http;
using System.Web.Mvc;

namespace Zanshin.Areas.RouteDebugger
{
    public class RouteDebuggerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RouteDebugger";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "RouteDebugger_default",
               url: "rd/{action}",
               defaults: new { controller = "RouteDebugger", action = "Index" },
               namespaces: new[] { "Zanshin.Areas.RouteDebugger.Controllers" });


            // Replace some of the default routing implementations with our custom debug
            // implementations.
            RouteDebuggerConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}
