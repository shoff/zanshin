
namespace Zanshin.WebApi
{
    using System;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Http.Description;
    using NLog;
    using Zanshin.Domain.Collections;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Repositories.Interfaces;
    using Zanshin.Domain.Services.Interfaces;
    using Zanshin.WebApi.Filters;

    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/topics")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TopicController : ApiController
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IEntityRepository<Topic, int> topicRepository;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicController" /> class.
        /// </summary>
        /// <param name="topicRepository">The topic repository.</param>
        /// <param name="cacheService">The cache service.</param>
        public TopicController(IEntityRepository<Topic, int> topicRepository, 
            ICacheService cacheService)
        {
            this.topicRepository = topicRepository;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Gets the specified topicid.
        /// </summary>
        /// <param name="topicid">The topicid.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pagesize">The page size.</param>
        /// <returns></returns>
        [Route("{topicid}/posts", Name = "Topics")]
        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> Get(int topicid, int pageNumber = 1, int pageSize = 10)
        {
            var topics =
                await this.topicRepository.GetAsync(x => x.TopicId == topicid, includeProperties: "Posts, Posts.Poster, Posts.Poster.UserIcon");

            var topic = this.cacheService.TryGet("TopicPosts_" + topicid.ToString(CultureInfo.InvariantCulture), topics.FirstOrDefault, null);

            if (topic != null)
            {
                topic.Views ++;
               
                await this.topicRepository.UpdateAsync(topic, topicid);

                // we don't need to send all the topic posts down, just the page we are on
                topic.PagedPosts = new SerializablePagination<Post>(topic.Posts, pageNumber, pageSize);
                topic.Posts = null;
                return this.Ok(topic);
            }

            return this.NotFound();
        }

        /// <summary>
        /// Posts the specified topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <returns></returns>
        [Route("")]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<IHttpActionResult> Post([FromBody]Topic topic)
        {

            try
            {
                var value = this.topicRepository.Insert(topic);
                if (value != null)
                {
                    return await Task.FromResult((IHttpActionResult)this.Created("location to be set", value));
                }

                return await Task.FromResult((IHttpActionResult)(this.InternalServerError()));

            }
            catch (Exception e)
            {
                logger.Error(e);
                return (IHttpActionResult)this.InternalServerError(e);
            }
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        [Route("")]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<IHttpActionResult> Put(int id, [FromBody]Topic value)
        {
            // TODO how do we know who is doing this?
            var topic = this.topicRepository.GetById(id);

            if (topic != null)
            {
                try
                {
                    await this.topicRepository.UpdateAsync(value, id);
                    this.topicRepository.Context.Commit();
                    return await Task.FromResult((IHttpActionResult)this.Ok());
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    return (IHttpActionResult)this.BadRequest(e.Message);
                }
            }
            try
            {
                topic = this.topicRepository.Insert(value);
                if (topic != null)
                {
                    return await Task.FromResult((IHttpActionResult)this.Created("location to be set", topic));
                }
                return await Task.FromResult((IHttpActionResult)(this.InternalServerError()));

            }
            catch (Exception e)
            {
                logger.Error(e);
                return (IHttpActionResult)this.InternalServerError(e);
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [Route("")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            // TODO try attaching rather than fetching.
            // TODO FIX THIS you were playing around with the entry .load shit.
            var topic = this.topicRepository.GetById(id);

            ((DbContext)this.topicRepository.Context)
                .Entry(topic).Collection(p => p.Posts).Load();

            if (topic == null)
            {
                return (IHttpActionResult)this.Ok();
            }
            try
            {
                await this.topicRepository.DeleteAsync(topic);
                return (IHttpActionResult)this.Ok();
            }
            catch (Exception e)
            {
                logger.Error(e);
                return (IHttpActionResult)this.Ok();
            }
        }
    }
}
