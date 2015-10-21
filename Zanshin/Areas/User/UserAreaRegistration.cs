using System.Web.Mvc;

namespace Zanshin.Areas.User
{
    public class UserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get  { return "User"; }
        }
        
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "User_default",
                url: "User/{username}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    username = UrlParameter.Optional
                },
                namespaces: new[] { "Zanshin.Areas.User.Controllers" });
        }
  
    }
}