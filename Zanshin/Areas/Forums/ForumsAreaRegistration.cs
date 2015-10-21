using System.Web.Mvc;

namespace Zanshin.Areas.Forums
{
    /// <summary>
    /// </summary>
    public class ForumsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Forums";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Forums_default",
                url: "Forums",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                },
                namespaces: new[] { "Zanshin.Areas.Forums.Controllers" });

            context.MapRoute(name: "Topic_Default",
                url: "Forums/{forumId}",
                defaults: new
                {
                    controller = "Home",
                    action = "Topics",
                    forumId = UrlParameter.Optional
                }, namespaces: new[] { "Zanshin.Areas.Forums.Controllers" });

            context.MapRoute(name: "Posts_default",
                url: "Forums/Posts/{topicId}",
                defaults: new
                {
                    controller = "Home",
                    action = "Posts",
                    topicId = UrlParameter.Optional
                }, namespaces: new[] { "Zanshin.Areas.Forums.Controllers" });
        }
    }
}