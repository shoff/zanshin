// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityRepository.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Zanshin.Domain.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Zanshin.Domain.Data.Interfaces;

    /// <summary>
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TK">The type of the k.</typeparam>
    public interface IEntityRepository<TEntity, in TK>
        where TEntity : class
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        IDataContext Context { get; }

        /// <summary>
        /// Generic method to get a collection of Entities
        /// </summary>
        /// <param name="filter">Filter expression for the return Entities</param>
        /// <param name="orderBy">Represents the order of the return Entities</param>
        /// <param name="includeProperties">Include Properties for the navigation properties</param>
        /// <returns>A Enumerable of Entities</returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");


        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        /// <summary>
        /// Finds the specified match.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "");

        /// <summary>
        /// Generic Method to get an Entity by Identity
        /// </summary>
        /// <param name="id">The Identity of the Entity</param>
        /// <returns>
        /// The Entity
        /// </returns>
        TEntity GetById(TK id);
        
        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(TK id, string includeProperties = null);
        
        /// <summary>
        /// Generic method for add an Entity to the context
        /// </summary>
        /// <param name="entity">The Entity to Add</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Generic method for deleting a method in the context by identity
        /// </summary>
        /// <param name="id">The Identity of the Entity</param>
        void Delete(TK id);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entityToDelete);

        /// <summary>
        /// Generic method for deleting a method in the context passing the Entity
        /// </summary>
        /// <param name="entityToDelete">Entity to Delete</param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Generic method for updating an Entity in the context
        /// </summary>
        /// <param name="entityToUpdate">The entity to Update</param>
        /// <param name="key">The key.</param>
        TEntity Update(TEntity entityToUpdate, TK key);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entityToUpdate, TK key);
        
        /// <summary>
        /// Generic implementation for get Paged Entities
        /// </summary>
        /// <typeparam name="TKey">Key for order Expression</typeparam>
        /// <param name="pageIndex">Index of the Page</param>
        /// <param name="pageCount">Number of Entities to get</param>
        /// <param name="orderByExpression">Order expression</param>
        /// <param name="ascending">If the order is ascending or descending</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// Enumerable of Entities matching the conditions
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        IEnumerable<TEntity> GetPagedElements<TKey>(int pageIndex, int pageCount,
            Expression<Func<TEntity, TKey>> orderByExpression, bool ascending = true, string includeProperties = "");


        //IEnumerable<TEntity> GetPagedElements<TKey>(int pageIndex, int pageCount,
        //    Expression<Func<TEntity, TKey>> orderByExpression, bool ascending = true, string includeProperties = "");

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="sqlQuery">The Query to be executed</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>
        /// List of Entity
        /// </returns>
        IEnumerable<TEntity> GetFromDatabaseWithQuery(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Execute a command in database
        /// </summary>
        /// <param name="sqlCommand">The sql query</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>integer representing the sql code</returns>
        int ExecuteInDatabaseByQuery(string sqlCommand, params object[] parameters);

        /// <summary>
        /// Get count of Entities
        /// </summary>
        /// <returns></returns>
        int GetCount();
    }
}