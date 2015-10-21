namespace Zanshin.Domain.Factories
{
    using System;
    using System.Configuration;
    using System.Net.Http;

    using Zanshin.Domain.Factories.Interfaces;
    using Zanshin.Domain.Interfaces;

    public sealed class ApiHttpClientFactory : IApiHttpClientFactory, IPerWebRequestLifestyle
    {
        public const string RestUrlKey = "RestUrl";

        /// <summary>
        /// Creates the specified URL.
        /// </summary>
        /// <param name="restBaseUrl">The URL.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">restBaseUrl</exception>
        public HttpClient Create(string restBaseUrl)
        {
            if (string.IsNullOrEmpty(restBaseUrl))
            {
                throw new ArgumentNullException("restBaseUrl");
            }

            // TODO URI factory
            Uri uri = new Uri(restBaseUrl);

            HttpClient client = new HttpClient
            {
                BaseAddress = uri,
            };

            return client;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public HttpClient Create()
        {
            string url = ConfigurationManager.AppSettings[RestUrlKey];
            return this.Create(url);
        }
    }
}