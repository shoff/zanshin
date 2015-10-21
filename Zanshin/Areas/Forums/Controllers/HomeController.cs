#pragma warning disable 1591

namespace Zanshin.Areas.Forums.Controllers
{
    using System;
    using System.Web.Mvc;
    using NLog;

    public class HomeController : Controller
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger(typeof(HomeController));
        
        [HttpGet]
        public ActionResult Index()
        {
            // this retuns categories

            // Hack to handle the weird shit that occurs when the request has a trailing slash.
            try
            {
                if ((ControllerContext.HttpContext.Request != null) && (ControllerContext.HttpContext.Request.Url != null))
                {
                    string referer = ControllerContext.HttpContext.Request.Url.OriginalString;

                    if (referer.EndsWith("/"))
                    {
                        string newRequest = referer.Substring(0, referer.Length - 1);
                        return Redirect(newRequest);
                    }
                }
            }
            catch (NotImplementedException notImplementedException)
            {
                logger.Error(notImplementedException);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Topics(int? forumId)
        {
            if (!forumId.HasValue)
            {
                return Index();
            }
            try
            {
                if ((ControllerContext.HttpContext.Request != null) && (ControllerContext.HttpContext.Request.Url != null))
                {
                    string referer = ControllerContext.HttpContext.Request.Url.OriginalString;

                    if (referer.EndsWith("/"))
                    {
                        string newRequest = referer.Substring(0, referer.Length - 1);
                        return Redirect(newRequest);
                    }
                }
            }
            catch (NotImplementedException notImplementedException)
            {
                logger.Error(notImplementedException);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Posts(int topicId)
        {
            return View();
        }

    }
}
#pragma warning restore 1591
