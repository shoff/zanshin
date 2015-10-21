namespace Zanshin.Domain.Providers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Zanshin.Domain.Data.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityStore<TEntity> : IEntityStore<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Constructor that takes a Context
        /// </summary>
        /// <param name="context"></param>
        public EntityStore(IDataContext context)
        {
            this.Context = context;
            this.DbEntitySet = (DbSet<TEntity>)context.SetEntity<TEntity>();
        }

        /// <summary>
        /// Context for the store
        /// </summary>
        public IDataContext Context { get; private set; }

        /// <summary>
        /// Used to query the entities
        /// </summary>
        public IQueryable<TEntity> EntitySet
        {
            get { return this.DbEntitySet; }
        }

        /// <summary>
        /// EntitySet for this store
        /// </summary>
        public DbSet<TEntity> DbEntitySet { get; private set; }

        /// <summary>
        /// FindAsync an entity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            return this.DbEntitySet.FindAsync(id);
        }

        /// <summary>
        /// Insert an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Create(TEntity entity)
        {
            this.DbEntitySet.Add(entity);
        }

        /// <summary>
        /// Mark an entity for deletion
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            this.DbEntitySet.Remove(entity);
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            if (entity != null)
            {
                this.Context.SetModified(entity);
            }
        }
    }
}