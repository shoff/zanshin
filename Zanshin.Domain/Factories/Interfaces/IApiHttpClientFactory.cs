namespace Zanshin.Domain.Factories.Interfaces
{
    using System.Net.Http;

    public interface IApiHttpClientFactory
    {
        /// <summary>
        /// Creates the specified URL.
        /// </summary>
        /// <param name="restBaseUrl">The URL.</param>
        /// <returns></returns>
        HttpClient Create(string restBaseUrl);

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        HttpClient Create();
    }
}