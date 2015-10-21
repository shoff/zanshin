namespace Zanshin.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}