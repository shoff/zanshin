#pragma warning disable 1591

namespace Zanshin.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Repositories.Interfaces;
    using Zanshin.Domain.Services.Interfaces;

    [RoutePrefix("api/v1/logs")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LogController : ApiController
    {
        
        private readonly IEntityRepository<Log, int> logRepository;
        private readonly ICacheService cacheService;

        public LogController(IEntityRepository<Log, int> logRepository, ICacheService cacheService)
        {
            this.logRepository = logRepository;
            this.cacheService = cacheService;
        }

        [HttpGet]
        [Route("", Name = "LogsV1")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return await Task.FromResult(
                    this.Ok(
                    this.cacheService.TryGet<Expression<Func<Log, bool>>,
                    Func<IQueryable<Log>,
                    IOrderedQueryable<Log>>, string,
                    IEnumerable<Log>>("Logs_", 
                        (x => x.LogLevel == "Info" || x.LogLevel == "Error" || x.LogLevel == "Fatal" || x.LogLevel=="Warn"),
                        (x=> x.OrderByDescending(y=>y.DateCreated)),
                        null, 
                        this.logRepository.Get, 
                        null)));
            }
            catch (Exception e)
            {
                return this.InternalServerError(e);
            }
        }
    }
}
#pragma warning restore 1591
