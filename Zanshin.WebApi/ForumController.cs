namespace Zanshin.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Http.Description;
    using NLog;
    using Zanshin.Domain.Collections;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Repositories.Interfaces;
    using Zanshin.WebApi.Filters;

    /// <summary>
    /// </summary>
    [RoutePrefix("api/v1/forums")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ForumController : ApiController
    {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IEntityRepository<Forum, int> forumRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumController"/> class.
        /// </summary>
        /// <param name="forumRepository">
        /// The forum repository.
        /// </param>
        public ForumController(IEntityRepository<Forum, int> forumRepository)
        {
            this.forumRepository = forumRepository;
        }

        /// <summary>
        /// Gets a collection of <seealso cref="Forum" />
        /// </summary>
        /// <returns>
        /// IEnumerable&lt;Forum&gt;
        /// </returns>
        [Route("")]
        [ResponseType(typeof(IEnumerable<Forum>))]
        public async Task<IEnumerable<Forum>> Get()
        {
            return await this.forumRepository.GetAsync();
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="forumId">The forum identifier.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pagesize">The page size.</param>
        /// <returns></returns>
        [Route("{forumId}", Name = "GetForumById")]
        [ResponseType(typeof(Forum))]
        public async Task<IHttpActionResult> GetById(int forumId, int pageNumber = 1, int pagesize = 10)
        {
            var forum = this.forumRepository.Get(x=>x.ForumId == forumId, includeProperties: "Topics,Topics.Createdby.UserIcon,ForumModerator").FirstOrDefault();

            if (forum != null)
            {
                forum.PagedTopics = new SerializablePagination<Topic>(forum.Topics, pageNumber, pagesize);
                return await Task.FromResult((IHttpActionResult)this.Ok(forum));
            }

            return await Task.FromResult(this.NotFound());
        }

        /// <summary>
        /// Posts the specified forum.
        /// </summary>
        /// <param name="forum">
        /// The forum.
        /// </param>
        /// <response code="201">Forum created</response>
        /// <response code="400">Forum name already in use</response>
        /// <returns>
        /// </returns>
        /// <exception cref="HttpResponseException">
        /// Condition. 
        /// </exception>
        [Route("")]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<IHttpActionResult> Post([FromBody] Forum forum)
        {
            if (string.IsNullOrWhiteSpace(forum.Name))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent(Messages.NameRequired)
                });
            }

            if (string.IsNullOrWhiteSpace(forum.ForumDescription))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent(Messages.DescriptionRequired)
                });
            }
            string submittedName = forum.Name.Trim().ToUpperInvariant();
            var forums = this.forumRepository.Get(x => x.CategoryId == forum.CategoryId);
            foreach (var f in forums)
            {
                if (f.Name.Trim().ToUpperInvariant() == submittedName)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent(string.Format(Messages.DuplicateForumName, forum.Name))
                    });
                }
            }

            try
            {
                // first see if this forum name has been used before
                this.forumRepository.Insert(forum);
                this.forumRepository.Context.Commit();
            }
            catch (Exception e)
            {
                // ReSharper disable once ThrowFromCatchWithNoInnerException
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent(e.Message), 

                });
            }

            var link = this.Url.Link("GetForumById", new { forumId = forum.ForumId });
            return link != null ?
                await Task.FromResult(this.Created(new Uri(link), forum)) :
                await Task.FromResult(this.Created("unknown", forum));
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="forumId">
        /// The forum identifier.
        /// </param>
        /// <param name="forum">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        [Route("{forumId}")]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<IHttpActionResult> Put(int forumId, [FromBody]Forum forum)
        {
            try
            {
                this.forumRepository.Update(forum, forum.ForumId);
                this.forumRepository.Context.Commit();

            }
            catch (Exception e)
            {
                logger.Error(e);
                return this.InternalServerError(e);
            }

            return await Task.FromResult(this.Created(new Uri(this.Url.Link("GetForumById", new { forumId })), forum));
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="forumId">
        /// The identifier.
        /// </param>
        /// <returns>
        /// </returns>
        [Route("{forumId}")]
        public async Task<IHttpActionResult> Delete(int forumId)
        {
            try
            {
                this.forumRepository.Delete(forumId);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch(Exception e)
            {
                logger.Error(e);
            }

            return await Task.FromResult(this.Ok());

        }
    }
}