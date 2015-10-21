using System.Web.Mvc;

namespace Zanshin.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Index(int userId)
        {
            @ViewBag.UserId = userId;
            return View();
        }

    }
}
