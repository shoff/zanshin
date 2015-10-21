namespace Zanshin.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Zanshin.Domain.Factories;
    using Zanshin.Domain.Factories.Interfaces;
    using Zanshin.Domain.Filters;
    using Zanshin.Domain.Repositories.Interfaces;

    public sealed class ApiRepository<T> : IApiRepository<T>
        where T : new()
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IApiHttpClientFactory apiHttpClientFactory;
        private readonly string apiRoute;

        private TimeSpan timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRepository{T}"/> class.
        /// </summary>
        /// <param name="apiHttpClientFactory">The API HTTP client factory.</param>
        /// <remarks>
        /// This sets the request timeout to 10 seconds by default. For long
        /// Timeout duration set the property.
        /// </remarks>
        /// <exception cref="OverflowException">
        ///   is less than <see cref="F:System.TimeSpan.MinValue" />
        ///  or greater than <see cref="F:System.TimeSpan.MaxValue" />.-or-
        ///   <name>value</name>
        ///   is <see cref="F:System.Double.PositiveInfinity" />
        /// .-or- is <see cref="F:System.Double.NegativeInfinity" />. </exception>
        public ApiRepository(IApiHttpClientFactory apiHttpClientFactory)
        {
            var resourceName = typeof(T).Name;
            this.timeout = TimeSpan.FromSeconds(10);
            this.apiRoute = Constants.ApiRouteSpecifier + resourceName;

            // could be null.
            this.apiHttpClientFactory = apiHttpClientFactory;

            // adding plural of the resource name for getting the collection of this type
            // TODO this breaks convention with API. Fix

            // Added this to allow for backward compatibility.
            if (this.apiHttpClientFactory == null)
            {
                this.HttpClient = new HttpClient
                {
                    BaseAddress = new Uri(ConfigurationManager.AppSettings[ApiHttpClientFactory.RestUrlKey]),
                    Timeout = this.timeout
                };
            }
            else
            {
                this.HttpClient = this.apiHttpClientFactory.Create(ConfigurationManager.AppSettings[ApiHttpClientFactory.RestUrlKey]);
            }

            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                (Constants.JsonApplicationContentType));
        }

        /// <summary>
        /// Deletes the specified resource.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The value of 'id' cannot be null. </exception>
        public HttpResponseMessage Delete(string id)
        {
            // this should be changed as the .Result puts a wait on the httpClient and can result in deadlocks.
            return this.HttpClient.DeleteAsync(string.Format("{0}/{1}", this.apiRoute, id)).Result;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The value of 'id' cannot be null. </exception>
        public async Task<HttpResponseMessage> DeleteAsync(string id)
        {
            return await this.HttpClient.DeleteAsync(string.Format("{0}/{1}", this.apiRoute, id));
        }

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public T Get(ResourceFilter filter)
        {
            var path = string.Format("{0}?{1}", this.apiRoute, filter);

            // this should be changed as the .Result puts a wait on the httpClient and can result in deadlocks.
            var response = this.HttpClient.GetAsync(path).Result;

            if (response.IsSuccessStatusCode)
            {
                // this should be changed as the .Result puts a wait on the httpClient and can result in deadlocks.
                return response.Content.ReadAsAsync<T>().Result;
            }
            return new T();
        }

        /// <summary>
        /// Gets the resource specified by the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The value of 'filter' cannot be null. </exception>
        public async Task<T> GetAsync(ResourceFilter filter)
        {
            var path = string.Format("{0}{1}", this.apiRoute, filter);
            var response = await this.HttpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }

            return new T();
        }

        /// <summary>
        /// Gets a resource. If id is specified then gets the resource
        /// with the specified Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T Get(string id)
        {
            // this should be changed as the .Result puts a wait on the httpClient and can result in deadlocks.
            var response = this.HttpClient.GetAsync(string.Format("{0}/{1}", this.apiRoute, id)).Result;
            return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<T>().Result : default(T);
        }

        /// <summary>
        /// Gets a resource asynchronously. If id is specified then gets the resource
        /// with the specified Id asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Instance of T if successful, otherwise 
        /// default(T) which can be null.</returns>
        public async Task<T> GetAsync(string id)
        {
            var response = await this.HttpClient.GetAsync(string.Format("{0}/{1}", this.apiRoute, id));
            T resource;
            if (response.IsSuccessStatusCode)
            {
                resource = await response.Content.ReadAsAsync<T>();
            }
            else
            {
                resource = default(T);
            }

            return resource;
        }

        /// <summary>
        /// Gets a resource.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Get()
        {
            List<T> resourceCollection = new List<T>();

            // this should be changed as the .Result puts a wait on the httpClient and can result in deadlocks.
            HttpResponseMessage response = this.HttpClient.GetAsync(this.apiRoute).Result;

            if (response.IsSuccessStatusCode)
            {
                // this should be changed as the .Result puts a wait on the httpClient and can result in deadlocks.
                var resultList = response.Content.ReadAsAsync<List<T>>().Result;

                if (resultList != null)
                {
                    foreach (var resultValue in resultList)
                    {
                        resourceCollection.Add(resultValue);
                    }
                }
            }
            else
            {
                resourceCollection.Add(new T());
            }

            return resourceCollection;
        }

        /// <summary>
        /// Gets a resource asynchronously. If id is specified then gets the resource
        /// with the specified Id asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAsync()
        {
            HttpResponseMessage response = await this.HttpClient.GetAsync(this.apiRoute);

            if (response.IsSuccessStatusCode)
            {
                var resultList = await response.Content.ReadAsAsync<IEnumerable<T>>();
                return resultList;
            }

            return Task.FromResult<IEnumerable<T>>(new List<T>()).Result;
        }

        /// <summary>
        /// Gets a resource with the applied filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public T Get(IList<KeyValuePair<string, string>> filter)
        {
            return this.Get(ResourceFilter.Create(filter));
        }

        /// <summary>
        /// Gets a resource with the applied filter asynchronously. If id is specified then gets the resource
        /// with the specified Id asynchronously.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">filter</exception>
        public async Task<T> GetAsync(IList<KeyValuePair<string, string>> filter)
        {
            return await this.GetAsync(ResourceFilter.Create(filter));
        }

        /// <summary>
        ///  Creates the resource.
        /// </summary>
        /// <param name="resource"> The resource. </param>
        public HttpResponseMessage Post(T resource)
        {
            // this should be changed as the .Result puts a wait on the httpClient and can result in deadlocks.
            return this.HttpClient.PostAsJsonAsync(this.apiRoute, resource).Result;
        }

        /// <summary>
        /// Asynchronously creates the resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(T resource)
        {
            return await this.HttpClient.PostAsJsonAsync(this.apiRoute, resource);
        }

        /// <summary>
        ///   Updates the specified resource.
        /// </summary>
        /// <param name="resource"> The resource. </param>
        /// <param name="id"> </param>
        /// <remarks>The id field can be null.</remarks>
        public HttpResponseMessage Put(string id, T resource)
        {
            HttpResponseMessage response;

            if (!string.IsNullOrEmpty(id))
            {
                string query = string.Format("{0}/{1}", this.apiRoute, id);
                response = this.HttpClient.PutAsJsonAsync(query, resource).Result;
            }
            else
            {
                response = this.HttpClient.PutAsJsonAsync(this.apiRoute, resource).Result;
            }
            return response;
        }

        /// <summary>
        /// Updates the resource asynchronously.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsync(string id, T resource)
        {
            HttpResponseMessage response;
            if (!string.IsNullOrEmpty(id))
            {
                string query = string.Format("{0}/{1}", this.apiRoute, id);
                response = await this.HttpClient.PutAsJsonAsync(query, resource);
            }
            else
            {
                response = await this.HttpClient.PutAsJsonAsync(this.apiRoute, resource);
            }
            return response;
        }

        /// <summary>
        /// Gets or sets the timeout value.
        /// </summary>
        public TimeSpan Timeout
        {
            get { return this.timeout; }
            set
            {
                this.timeout = value;
                this.HttpClient.Timeout = this.timeout;
            }
        }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <value>
        /// The HTTP client.
        /// </value>
        /// <remarks>This property is not added onto the interface
        /// so as not to encourage direct use of the client.</remarks>
        public HttpClient HttpClient { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (this.HttpClient != null)
                {
                    this.HttpClient.Dispose();
                    this.HttpClient = null;
                }
            }
        }
    }
}