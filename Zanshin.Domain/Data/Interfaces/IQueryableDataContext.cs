namespace Zanshin.Domain.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;

    /// <summary>
    /// </summary>
    public interface IQueryableDataContext
    {
        /// <summary>
        /// Returns a IDbSet instance for access to entities of the given type in the context,
        /// the ObjectStateManager, and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IDbSet<TEntity> SetEntity<TEntity>() where TEntity : class;

        /// <summary>
        /// Attaches the specified item.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="item">The item.</param>
        void Attach<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Set object as modified
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="item">The entity item to set as modifed</param>
        void SetModified<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown
        ///</remarks>
        int Commit();

        /// <summary>
        /// Commits the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Execute specific query with underliying persistence store
        /// </summary>
        /// <typeparam name="TEntity">Entity type to map query results</typeparam>
        /// <param name="sqlQuery">
        /// Dialect Query 
        /// <example>
        /// SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer > {0}
        /// </example>
        /// </param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>
        /// Enumerable results 
        /// </returns>
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Execute arbitrary command into underliying persistence store
        /// </summary>
        /// <param name="sqlCommand">Command to execute
        /// <example>
        /// SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer &gt; {0}
        /// </example></param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>
        /// The number of affected records
        /// </returns>
        int ExecuteCommand(string sqlCommand, params object[] parameters);
    }
}