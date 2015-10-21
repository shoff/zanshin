namespace Zanshin.Domain
{
    public static class Constants
    {
        public const string JsonApplicationContentType = "application/json";
        public const string JsonTextContentType = "text/json";
        public const string HtmlTextContentType = "text/html";
        public const string XmlContentType = "application/xml";
        public const string XmlTextContentType = "text/xml";
        public const string UrlEncodedContentType = "application/x-www-form-urlencoded";
        public const string Delete = "DELETE";
        public const string Put = "PUT";
        public const string Post = "POST";
        public const string Options = "OPTIONS";
        public const string Head = "HEAD";
        public const string Get = "GET";


        // for custom headers
        public const string UserIpAddress = "REMOTE_ADDR";
        public const string UserName = "Username";
        public const string Anon = "Anonymous";

        public const string GIF = "image/gif";
        public const string JPEG = "image/jpeg";
        public const string PNG = "image/png";


        // Environment keys available

        /// <summary>
        /// The rest URL key this is currently the only key that 
        /// must be present.
        /// </summary>
        public const string RestUrlKey = "RestBaseUri";

        /// <summary>
        /// The default sliding expiration
        /// </summary>
        public const string DefaultSlidingExpiration = "DefaultSlidingExpiration";

        /// <summary>
        /// The amount of time, in minutes from the moment the item is cached to expire it.
        /// </summary>
        public const string DefaultAbsoluteExpiration = "DefaultAbsoluteExpiration";

        // api 
        public const string ApiRouteSpecifier = "api/";


    }
}