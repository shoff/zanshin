namespace Zanshin
{
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.EnableCors();

            var json = httpConfiguration.Formatters.JsonFormatter;
            httpConfiguration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
            {
                IgnoreSerializableInterface = true,
                IgnoreSerializableAttribute = true,
                SerializeCompilerGeneratedMembers = true
            };
            //httpConfiguration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling =
            //    Newtonsoft.Json.PreserveReferencesHandling.None;
            //httpConfiguration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = 
            //    Newtonsoft.Json.PreserveReferencesHandling.All;

            // Web API routes
            httpConfiguration.MapHttpAttributeRoutes();

            //httpConfiguration.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

        }
    }
}
