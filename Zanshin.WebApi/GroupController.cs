
namespace Zanshin.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Http.Description;
    using NLog;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Repositories.Interfaces;
    using Zanshin.Domain.Services.Interfaces;
    using Zanshin.WebApi.Filters;

    /// <summary>
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1/groups")]
    public class GroupController : ApiController
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IEntityRepository<Group, int> groupsRepository;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupController" /> class.
        /// </summary>
        /// <param name="groupsRepository">The groups repository.</param>
        /// <param name="cacheService">The cache service.</param>
        public GroupController(IEntityRepository<Group, int> groupsRepository, ICacheService cacheService)
        {
            this.groupsRepository = groupsRepository;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(IEnumerable<Group>))]

        public IEnumerable<Group> Get()
        {
            // Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = ""
            return this.cacheService.TryGet<Expression<Func<Group, bool>>, Func<IQueryable<Group>, IOrderedQueryable<Group>>, string,
                IEnumerable<Group>>("Groups", (x => x != null), null, null, this.groupsRepository.Get, null);
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        [Route("")]
        [CheckModelForNull]
        [ValidateModelState]
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Group group)
        {
            try
            {
                await this.groupsRepository.UpdateAsync(group, id);
                return this.StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return this.InternalServerError(e);
            }
        }

    }
}