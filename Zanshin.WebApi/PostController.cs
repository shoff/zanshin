
namespace Zanshin.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Http.Description;
    using NLog;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Repositories.Interfaces;
    using Zanshin.WebApi.Filters;

    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/posts")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PostController : ApiController
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The post repository
        /// </summary>
        private readonly IEntityRepository<Post, int> postRepository;
        private readonly IEntityRepository<Topic, int> topicRepository;
        private readonly IEntityRepository<User, int> userRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="PostController" /> class.
        /// </summary>
        /// <param name="postRepository">The post repository.</param>
        /// <param name="topicRepository">The topic repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public PostController(IEntityRepository<Post, int> postRepository, 
            IEntityRepository<Topic, int> topicRepository, IEntityRepository<User, int> userRepository)
        {
            this.postRepository = postRepository;
            this.topicRepository = topicRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Gets posts belonging to the specified TopicId
        /// </summary>
        /// <param name="topicId">The topic identifier.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <returns></returns>
        [Route("{topicId}", Name = "Post")]
        [ResponseType(typeof(IEnumerable<Post>))]
        public async Task<IHttpActionResult> Get(int topicId, int replyTo)
        {
            //Contract.Ensures(Contract.Result<Task<IHttpActionResult>>() != null);
            var post =
                this.postRepository.Get(x => x.TopicId == topicId && x.ReplyToPostId == replyTo, includeProperties: "ReplyToPost,Tags")
                .OrderBy(x=>x.DateCreated);

            if (post.Any())
            {
                return await Task.FromResult((IHttpActionResult)this.Ok(post));
            }

            return await Task.FromResult((IHttpActionResult)NotFound());
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> Get(int id)
        {
           // Contract.Ensures(Contract.Result<Task<IHttpActionResult>>() != null);
            var post =
                this.postRepository.Get(x => x.PostId == id, includeProperties: "Poster,Poster.UserIcon,ReplyToPost,PostTopic,Tags")
                .FirstOrDefault();
            if (post != null)
            {
                return await Task.FromResult((IHttpActionResult)this.Ok(post));
            }
            return await Task.FromResult((IHttpActionResult)NotFound());
        }
        
        /// <summary>
        /// Posts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        [Route("")]
        //[CheckModelForNull]
        //[ValidateModelState]
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> Post([FromBody]Post value)
        {
            try
            {
                value.Poster = this.userRepository.GetById(value.UserId);
                value.PostTopic = this.topicRepository.Get(x => x.TopicId == value.TopicId, includeProperties: "Posts, Posts.Poster").First();

                var post = await this.postRepository.InsertAsync(value);

                if (post != null)
                {
                    return await Task.FromResult((IHttpActionResult)this.Created("location to be set", post));
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
        [Route("{id}")]
        [CheckModelForNull]
        [ValidateModelState]        
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> Put(int id, [FromBody]Post value)
        {
            // TODO how do we know who is doing this?
            var post = this.postRepository.GetById(id);

            if (post != null)
            {
                try
                {
                    this.postRepository.Update(value, id);
                    this.postRepository.Context.Commit();
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
                post = this.postRepository.Insert(value);
                if (post != null)
                {
                    return await Task.FromResult((IHttpActionResult)this.Created("location to be set", post));
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
        /// <returns></returns>
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var post = this.postRepository.GetById(id);
            if (post == null)
            {
                return (IHttpActionResult)this.Ok();
            }
            try
            {
                await this.postRepository.DeleteAsync(post);
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
