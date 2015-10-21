using System.Web.Mvc;

namespace Zanshin.Controllers
{
    using Zanshin.Domain.Helpers.Interfaces;

    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly IConfigurationWrapper configurationWrapper;

        public HomeController(IConfigurationWrapper configurationWrapper)
        {
            this.configurationWrapper = configurationWrapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}