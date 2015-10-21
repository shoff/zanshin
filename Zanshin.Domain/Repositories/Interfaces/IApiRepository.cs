namespace Zanshin.Domain.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Zanshin.Domain.Filters;

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApiRepository<T>
        where T : new()
    {
        /// <summary>
        /// Deletes the specified resource.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        HttpResponseMessage Delete(string id);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> DeleteAsync(string id);

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        T Get(ResourceFilter filter);

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<T> GetAsync(ResourceFilter filter);

        /// <summary>
        /// Gets a resource. If id is specified then gets the resource
        /// with the specified Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T Get(string id);

        /// <summary>
        /// Gets a resource asynchronously. If id is specified then gets the resource
        /// with the specified Id asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Instance of T if successful, otherwise 
        /// default(T) which can be null.</returns>
        Task<T> GetAsync(string id);

        /// <summary>
        /// Gets a resource.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Get();

        /// <summary>
        /// Gets a resource asynchronously. If id is specified then gets the resource
        /// with the specified Id asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAsync();

        /// <summary>
        /// Gets a resource with the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">filter</exception>
        T Get(IList<KeyValuePair<string, string>> filter);

        /// <summary>
        /// Gets a resource with the applied filter asynchronously. If id is specified then gets the resource
        /// with the specified Id asynchronously.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">filter</exception>
        Task<T> GetAsync(IList<KeyValuePair<string, string>> filter);

        /// <summary>
        ///  Creates the resource.
        /// </summary>
        /// <param name="resource"> The resource. </param>
        HttpResponseMessage Post(T resource);

        /// <summary>
        /// Asynchronously creates the resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsync(T resource);

        /// <summary>
        ///   Updates the specified resource.
        /// </summary>
        /// <param name="resource"> The resource. </param>
        /// <param name="id"> </param>
        /// <remarks>The id field can be null.</remarks>
        HttpResponseMessage Put(string id, T resource);

        /// <summary>
        /// Updates the resource asynchronously.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> PutAsync(string id, T resource);

        /// <summary>
        /// Gets or sets the timeout value.
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <value>
        /// The HTTP client.
        /// </value>
        /// <remarks>This property is not added onto the interface
        /// so as not to encourage direct use of the client.</remarks>
        HttpClient HttpClient { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void Dispose();
    }


    [ExcludeFromCodeCoverage]
    [ContractClassFor(typeof(IApiRepository<>))]
    internal abstract class ApiRepositoryContract<T> : IApiRepository<T>
        where T : new()
    {
        /// <summary>
        /// Deletes the specified resource.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">id</exception>
        public HttpResponseMessage Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }
            return default(HttpResponseMessage);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">id</exception>
        public Task<HttpResponseMessage> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }
            return default(Task<HttpResponseMessage>);
        }

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">filter</exception>
        public T Get(ResourceFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            return default(T);
        }

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">filter</exception>
        public Task<T> GetAsync(ResourceFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            return default(Task<T>);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">id</exception>
        public T Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }
            return default(T);
        }

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> GetAsync(string id)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(id), "id");
            return default(Task<T>);
        }

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<T> Get();

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <returns></returns>
        public abstract Task<IEnumerable<T>> GetAsync();

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">filter</exception>
        public T Get(IList<KeyValuePair<string, string>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            return default(T);
        }

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">filter</exception>
        public Task<T> GetAsync(IList<KeyValuePair<string, string>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            return default(Task<T>);
        }

        /// <summary>
        /// Creates the resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">resource</exception>
        public HttpResponseMessage Post(T resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            return default(HttpResponseMessage);
        }

        /// <summary>
        /// Asynchronously creates the resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">resource</exception>
        public Task<HttpResponseMessage> PostAsync(T resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            return default(Task<HttpResponseMessage>);
        }

        /// <summary>
        /// Updates the specified resource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// resource
        /// or
        /// id
        /// </exception>
        /// <remarks>
        /// The id field can be null.
        /// </remarks>
        public HttpResponseMessage Put(string id, T resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }
            return default(HttpResponseMessage);
        }

        /// <summary>
        /// Updates the resource asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// resource
        /// or
        /// id
        /// </exception>
        public Task<HttpResponseMessage> PutAsync(string id, T resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }
            return default(Task<HttpResponseMessage>);
        }

        /// <summary>
        /// Gets or sets the timeout value.
        /// </summary>
        public abstract TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <value>
        /// The HTTP client.
        /// </value>
        /// <remarks>
        /// This property is not added onto the interface
        /// so as not to encourage direct use of the client.
        /// </remarks>
        public abstract HttpClient HttpClient { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();
    }
}
