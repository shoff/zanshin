

namespace Zanshin.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Repositories.Interfaces;
    using Zanshin.Domain.Services.Interfaces;

    public class HomeController : Controller
    {
        private readonly IEntityRepository<Website, int> websiteRepository;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="websiteRepository">The website repository.</param>
        /// <param name="cacheService">The cache service.</param>
        public HomeController(IEntityRepository<Website, int> websiteRepository, ICacheService cacheService)
        {
            this.websiteRepository = websiteRepository;
            this.cacheService = cacheService;
        }

        // GET: Admin/Home
        public ActionResult Index()
        {
            var website = this.websiteRepository.Get(x => x.WebsiteId == 1);
            if (website.Any())
            {
                ViewBag.WebSiteName = website.ToList().First().Name + " Admin Site";
            }
            else
            {
                ViewBag.WebSiteName = "Admin website";
            }
            return View();
        }
    }
}