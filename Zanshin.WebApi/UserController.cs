

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
    using Zanshin.Domain.Collections;
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Repositories.Interfaces;
    using Zanshin.Domain.Services.Interfaces;
    using Zanshin.WebApi.Filters;

    /// <summary>
    /// </summary>
    [RoutePrefix("api/v1/users")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IEntityRepository<User, int> userRepository;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="cacheService">The cache service.</param>
        public UserController(IEntityRepository<User, int> userRepository, ICacheService cacheService)
        {
            this.userRepository = userRepository;
            this.cacheService = cacheService;
        }

        // GET: api/Users
        // TODO: authorization
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("paged/{pagenumber}", Name = "UsersV1")]
        [ResponseType(typeof(SerializablePagination<User>))]
        public async Task<IHttpActionResult> Get(int? pagenumber)
        {
            try
            {
                var users = this.cacheService.TryGet<Expression<Func<User, bool>>,
                     Func<IQueryable<User>, IOrderedQueryable<User>>, string,
                     IEnumerable<User>>("Users", (x => x != null), null, "Groups,Profile,UserIcon,Rank", this.userRepository.Get, null);

                var userArray = users as User[] ?? users.ToArray();

                if (!userArray.Any())
                {
                    // no users? Ok just return
                    return await Task.FromResult(this.Ok());
                }

                pagenumber = pagenumber.HasValue ? pagenumber : 0;

                return await Task.FromResult((IHttpActionResult)this.Ok(
                    new SerializablePagination<User>(userArray.ToList(), (int)pagenumber)));
            }
            catch (Exception e)
            {
                logger.Error(e);
                return this.InternalServerError(e);
            }
        }

        /// <summary>
        /// Gets the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [Route("{userId}", Name = "GetUserById")]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> Get(int userId)
        {
            try
            {
                var user = await this.userRepository.GetAsync(x => x.Id == userId, includeProperties: "Groups,Profile,UserIcon,Rank");
                if (user != null)
                {
                    return this.Ok(user);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                return this.InternalServerError(e);
            }
            return NotFound();
        }

        /// <summary>
        /// Posts the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("")]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<IHttpActionResult> Post([FromBody] User user)
        {
            try
            {
                await this.userRepository.InsertAsync(user);
                return this.CreatedAtRoute("GetUserById", new
                {
                    userId = user.Id
                }, user);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return this.InternalServerError(e);
            }
        }

        // PUT: api/Users/5
        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("{id}")]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<IHttpActionResult> Put(int id, [FromBody] User user)
        {
            try
            {
                await this.userRepository.UpdateAsync(user, user.Id);
                return this.StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return this.InternalServerError(e);
            }
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [Route("{id}")]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<IHttpActionResult> Delete(int id, [FromBody] User user)
        {
            try
            {
                await this.userRepository.DeleteAsync(user);
                return this.StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                logger.Error(e);

                // don't let them know that an error has occured on delete.
                return this.Ok();
            }
        }
    }
}
