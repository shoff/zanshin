

namespace Zanshin.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
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
    [RoutePrefix("api/v1/categories")]
    public class CategoryController : ApiController
    {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IEntityRepository<Category, int> categoryRepository;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController" /> class.
        /// </summary>
        /// <param name="categoryRepository">The category repository.</param>
        /// <param name="cacheService">The cache service.</param>
        public CategoryController(IEntityRepository<Category, int> categoryRepository, ICacheService cacheService)
        {
            this.categoryRepository = categoryRepository;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(IEnumerable<Category>))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return await Task.FromResult(
                    this.Ok(
                    this.cacheService.TryGet<Expression<Func<Category, bool>>,
                    Func<IQueryable<Category>,
                    IOrderedQueryable<Category>>, string,
                    IEnumerable<Category>>("Categories", (x => x != null), null, "Forums", 
                    this.categoryRepository.Get, null)));
            }
            catch (Exception e)
            {
                logger.Error(e);
                return this.InternalServerError(e);
            }
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        [Route("{categoryId}", Name = "GetCategoryById")]
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetById(int categoryId)
        {
            var category = this.categoryRepository.Get(x => x.CategoryId == categoryId, includeProperties: "Forums");
            if (category != null)
            {
                return await Task.FromResult(this.Ok(category));
            }

            return await Task.FromResult(this.NotFound());
        }

        /// <summary>
        /// Posts the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<IHttpActionResult> Post([FromBody] Category category)
        {
            try
            {
                this.categoryRepository.Insert(category);
                this.categoryRepository.Context.Commit();
            }
            catch (Exception e)
            {
                logger.Error(e);
                return this.InternalServerError(e);
            }
            
            var link = this.Url.Link("GetCategoryById", new { categoryId = category.CategoryId });

            return link != null ?
                await Task.FromResult(this.Created(new Uri(link), category)) :
                await Task.FromResult(this.Created("unknown", category));
        }

        [HttpPut]
        [Route("{id}")]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<IHttpActionResult> Put(int id, [FromBody] Category value)
        {
            // TODO how do we know who is doing this?
            var category = this.categoryRepository.GetById(id);

            if (category != null)
            {
                try
                {
                    await this.categoryRepository.UpdateAsync(value, id);
                    this.categoryRepository.Context.Commit();
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
                category = this.categoryRepository.Insert(value);
                if (category != null)
                {
                    return await Task.FromResult((IHttpActionResult)this.Created("location to be set", category));
                }
                return await Task.FromResult((IHttpActionResult)(this.InternalServerError()));

            }
            catch (Exception e)
            {
                logger.Error(e);
                return (IHttpActionResult)this.InternalServerError(e);
            }
        }
    }
}