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

    [RoutePrefix("api/v1/messages")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PrivateMessagesController : ApiController
    {
        private readonly IEntityRepository<PrivateMessage, int> messageRepository;
        private readonly ICacheService cacheService;

        public PrivateMessagesController(IEntityRepository<PrivateMessage, int> messageRepository, ICacheService cacheService)
        {
            this.messageRepository = messageRepository;
            this.cacheService = cacheService;
        }

        [HttpGet]
        [Route("", Name = "MessagesV1")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return await Task.FromResult(
                    this.Ok(
                    this.cacheService.TryGet<Expression<Func<PrivateMessage, bool>>,
                    Func<IQueryable<PrivateMessage>,
                    IOrderedQueryable<PrivateMessage>>, string,
                    IEnumerable<PrivateMessage>>("PrivateMessages_", (x => x != null), null, "FromUser,ToUser", this.messageRepository.Get, null)));

            }
            catch (Exception e)
            {
                return this.InternalServerError(e);
            }
        }
    }
}
#pragma warning restore 1591
