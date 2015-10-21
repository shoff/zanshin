namespace Zanshin.WebApi
{
    using System.Web.Http;
    using Zanshin.Domain.Repositories.Interfaces;

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TK">The type of the k.</typeparam>
    public abstract class BaseController<T, TK> : ApiController
        where T : class
    {
        private IEntityRepository<T, TK> entityRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{T, TK}"/> class.
        /// </summary>
        /// <param name="entityRepository">The entity repository.</param>
        protected BaseController(IEntityRepository<T, TK> entityRepository)
        {
            this.entityRepository = entityRepository;
        }



    }
}