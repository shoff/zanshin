

using System;
using System.Linq;
using NLog;

namespace Zanshin.Domain.Data
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.Validation;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Zanshin.Domain.Data.Configurations;
    using Zanshin.Domain.Data.Interfaces;
    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Interfaces;

    /// <summary>
    /// </summary>
    public sealed partial class DataContext : DbContext, IDataContext, ITransientLifestyle
    {
        private static readonly Logger logger = LogManager.GetLogger("DataContext");


        /// <summary>
        /// Initializes the <see cref="DataContext"/> class.
        /// </summary>
        static DataContext()
        {
            Database.SetInitializer(new EntityContextInitializer());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        public DataContext()
            : base("DefaultConnection")

            //: base(@"Data Source=SQL5013.myASP.NET;Initial Catalog=DB_9BB5A3_angularforum;User Id=DB_9BB5A3_angularforum_admin;Password=xo2Die5l;")
        {
        }

        /// <summary>
        /// Tracks the changes.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void TrackChanges(bool value)
        {
            this.Configuration.AutoDetectChangesEnabled = value;
        }

        /// <summary>
        /// Returns a IDbSet instance for access to entities of the given type in the context,
        /// the ObjectStateManager, and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IDbSet<TEntity> SetEntity<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }

        /// <summary>
        /// Attaches the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (this.Entry(entity).State == EntityState.Detached)
            {
                this.Set<TEntity>().Attach(entity);
            }
        }

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        /// <remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,
        /// then an exception is thrown
        /// </remarks>
        /// <exception cref="DbEntityValidationException">Condition. </exception>
        /// <exception cref="Exception">Condition. </exception>
        public int Commit()
        {
            try
            {
                //DateTime saveTime = DateTime.Now;
                //foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
                //{
                //    if (entry.Property("DateCreated").CurrentValue == null)
                //    {
                //        entry.Property("DateCreated").CurrentValue = saveTime;
                //    }
                //}
 
               return this.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages =
                    (from eve in ex.EntityValidationErrors let entity = eve.Entry.Entity.GetType().Name from ev in eve.ValidationErrors
                     select new { Entity = entity, ev.PropertyName, ev.ErrorMessage });

                var fullErrorMessage = string.Join(
                    "; ",
                    errorMessages.Select(
                        e => string.Format("[Entity: {0}, Property: {1}] {2}", e.Entity, e.PropertyName, e.ErrorMessage)));

                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                logger.Error(exceptionMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors, ex);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                logger.Error(e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Commits the asynchronous.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DbEntityValidationException">
        ///             The save was aborted because validation of entity property values failed.
        ///             </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///             A database command did not affect the expected number of rows. This usually indicates an optimistic 
        ///             concurrency violation; that is, a row has been changed in the database since it was queried.
        ///             </exception>
        /// <exception cref="DbUpdateException">An error occurred sending updates to the database.</exception>
        public async Task<int> CommitAsync()
        {
            return await this.SaveChangesAsync();
        }

        /// <summary>
        /// Sets the modified.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public void SetModified<TEntity>(TEntity entity) 
            where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Execute specific query with underlying persistence store
        /// </summary>
        /// <typeparam name="TEntity">Entity type to map query results</typeparam>
        /// <param name="sqlQuery">Dialect Query
        /// <example>
        /// SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer &gt; {0}
        /// </example></param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>
        /// Enumerable results
        /// </returns>
        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return this.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        /// <summary>
        /// Execute arbitrary command into underlaying persistence store
        /// </summary>
        /// <param name="sqlCommand">Command to execute
        /// <example>
        /// SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer &gt; {0}
        /// </example></param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>
        /// The number of affected records
        /// </returns>
        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return this.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new TagConfiguration());

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserLoginConfiguration());
            modelBuilder.Configurations.Add(new UserClaimConfiguration());
            modelBuilder.Configurations.Add(new UserGroupConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());

            // Forums
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new ForumConfiguration());
            modelBuilder.Configurations.Add(new TopicConfiguration());
            modelBuilder.Configurations.Add(new PostConfiguration());
            modelBuilder.Configurations.Add(new AvatarConfiguration());
            modelBuilder.Configurations.Add(new WebsiteConfiguration());

            modelBuilder.Entity<MessageReadByUser>().HasKey(l => new
            {
                l.GroupMessageId,
                l.UserId
            }).ToTable("MessagesReadByUsers");


        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static DataContext Create()
        {
            return new DataContext();
        }

    }
}