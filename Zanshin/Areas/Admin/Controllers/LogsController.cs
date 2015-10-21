namespace Zanshin.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Repositories.Interfaces;

    public class LogsController : Controller
    {
        private readonly IEntityRepository<Log, int> logRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsController"/> class.
        /// </summary>
        /// <param name="logRepository">The log repository.</param>
        public LogsController(IEntityRepository<Log, int> logRepository)
        {
            this.logRepository = logRepository;
        }

        public ActionResult Index()
        {
            var yesterday = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            var logs = this.logRepository.Get(); //(x => x.DateCreated > yesterday);
            return this.View(logs);
        }

    }
}