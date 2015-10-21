namespace Zanshin.Domain.Providers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Zanshin.Domain.Data.Interfaces;

    public interface IEntityStore<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Context for the store
        /// </summary>
        IDataContext Context { get; }

        /// <summary>
        /// Used to query the entities
        /// </summary>
        IQueryable<TEntity> EntitySet { get; }

        /// <summary>
        /// EntitySet for this store
        /// </summary>
        DbSet<TEntity> DbEntitySet { get; }

        /// <summary>
        /// FindAsync an entity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// Insert an entity
        /// </summary>
        /// <param name="entity"></param>
        void Create(TEntity entity);

        /// <summary>
        /// Mark an entity for deletion
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
    }
}