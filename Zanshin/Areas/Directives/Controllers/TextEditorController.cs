
using System.Web.Mvc;
#pragma warning disable 1591

namespace Zanshin.Areas.Directives.Controllers
{
    // ReSharper disable once InconsistentNaming
    public class TextEditorController : Controller
    {
        // GET: Directives/CKEditor
        public ActionResult BasicEditor()
        {
            return View();
        }
    }
}