
using System.Web.Mvc;

namespace Zanshin.Areas.User.Controllers
{
    public class PrivateMessagesController : Controller
    {
        // GET: User/PrivateMessages
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}