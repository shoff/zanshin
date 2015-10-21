namespace Zanshin.Domain.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Zanshin.Domain.Data.Interfaces;
    using Zanshin.Domain.Entities;

    public interface IGeoLocationRepository
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        IDataContext Context { get; set; }

        /// <summary>
        /// Generic method to get a collection of Entities
        /// </summary>
        /// <param name="filter">Filter expression for the return Entities</param>
        /// <param name="orderBy">Represents the order of the return Entities</param>
        /// <param name="includeProperties">Include Properties for the navigation properties</param>
        /// <returns>A Enumerable of Entities</returns>
        IEnumerable<GeoLocation> Get(Expression<Func<GeoLocation, bool>> filter = null,
            Func<IQueryable<GeoLocation>, IOrderedQueryable<GeoLocation>> orderBy = null, string includeProperties = "");

        /// <summary>
        /// Generic Method to get an Entity by Identity
        /// </summary>
        /// <param name="id">The Identity of the Entity</param>
        /// <returns>
        /// The Entity
        /// </returns>
        GeoLocation GetById(Int32 id);

        /// <summary>
        /// Generic method for add an Entity to the context
        /// </summary>
        /// <param name="entity">The Entity to Add</param>
        GeoLocation Insert(GeoLocation entity);

        /// <summary>
        /// Generic method for deleting a method in the context by identity
        /// </summary>
        /// <param name="id">The Identity of the Entity</param>
        void Delete(Int32 id);

        /// <summary>
        /// Generic method for deleting a method in the context pasing the Entity
        /// </summary>
        /// <param name="entityToDelete">Entity to Delete</param>
        void Delete(GeoLocation entityToDelete);

        /// <summary>
        /// Generic method for updating an Entity in the context
        /// </summary>
        /// <param name="entityToUpdate">The story to Update</param>
        void Update(GeoLocation entityToUpdate);

        /// <summary>
        /// Generic implementation for get Paged Entities
        /// </summary>
        /// <typeparam name="TKey">Key for order Expression</typeparam>
        /// <param name="pageIndex">Index of the Page</param>
        /// <param name="pageCount">Number of Entities to get</param>
        /// <param name="orderByExpression">Order expression</param>
        /// <param name="orderby">if set to <c>true</c> [orderby].</param>
        /// <returns>
        /// Enumerable of Entities matching the conditions
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        IEnumerable<GeoLocation> GetPagedElements<TKey>(int pageIndex, int pageCount,
            Expression<Func<GeoLocation, TKey>> orderByExpression, bool orderby = true);

        /// <summary>
        /// Generic implementation for get Paged Entities
        /// </summary>
        /// <typeparam name="TKey">Key for order Expression</typeparam>
        /// <param name="pageIndex">Index of the Page</param>
        /// <param name="pageCount">Number of Entities to get</param>
        /// <param name="orderByExpression">Order expression</param>
        /// <param name="ascending">If the order is ascending or descending</param>
        /// <param name="includeProperties">Includes</param>
        /// <returns>
        /// Enumerable of Entities matching the conditions
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        IEnumerable<GeoLocation> GetPagedElements<TKey>(int pageIndex, int pageCount,
            Expression<Func<GeoLocation, TKey>> orderByExpression, bool ascending = true, string includeProperties = "");

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="sqlQuery">The Query to be executed</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>
        /// List of Entity
        /// </returns>
        IEnumerable<GeoLocation> GetFromDatabaseWithQuery(string sqlQuery, params object[] parameters);

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

        /// <summary>
        /// Adds if not exists.
        /// </summary>
        /// <param name="geoLocation">The geo location.</param>
        /// <returns></returns>
        GeoLocation AddIfNotExists(GeoLocation geoLocation);
    }
}