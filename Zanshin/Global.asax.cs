using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Zanshin
{
    using System.Web.Http;
    using System.Web.Http.Dispatcher;

    using Zanshin.Domain.Factories.Interfaces;
    using Zanshin.Domain.Services;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {

            ContainerConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            IHttpControllerActivator httpControllerActivator = Ioc.Instance.Resolve<IHttpControllerActivator>();
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), httpControllerActivator);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // set the MVC controller builder to our custom builder
            var controllerFactory = Ioc.Instance.Resolve<IForumControllerFactory>();
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}
