namespace Zanshin.Domain.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    using Zanshin.Domain.Exceptions;

    /// <summary>
    /// Resource filter can be used api repositories to send key value pairs to 
    /// a WebApi, or any html provider.
    /// </summary>
    public sealed class ResourceFilter : List<KeyValuePair<string, string>>
    {
        private readonly object syncRoot = new object();

        /// <summary>
        /// Creates the filter with an assumed key value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ResourceFilter Create(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            return new ResourceFilter { new KeyValuePair<string, string>("?id", value) };
        }

        /// <summary>
        /// Creates the specified key value pairs.
        /// </summary>
        /// <param name="keyValuePairs">The key value pairs.</param>
        /// <returns></returns>
        public static ResourceFilter Create(IList<KeyValuePair<string, string>> keyValuePairs)
        {
            if (keyValuePairs == null)
            {
                throw new ArgumentNullException("keyValuePairs");
            }

            if (keyValuePairs[0].Key.ToUpperInvariant() != "ID")
            {
                throw new IdMissingException();
            }
            ResourceFilter filter = new ResourceFilter();

            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Key.ToUpperInvariant() == "ID")
                {
                    filter.Add(new KeyValuePair<string, string>("?id", kvp.Value));
                }
                else
                {
                    filter.Add(kvp);
                }
            }
            return filter;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {


            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}={1}", this[0].Key, HttpUtility.UrlEncode(this[0].Value));

            for (int i = 1; i < this.Count; i++)
            {
                sb.AppendFormat("&{0}={1}", this[i].Key, HttpUtility.UrlEncode(this[i].Value));
            }
            return sb.ToString();
        }
    }
}