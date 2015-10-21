namespace Zanshin.Domain.Filters
{
    using System;
    using System.Web.Caching;

    using Zanshin.Domain.Filters.Interfaces;

    public sealed class CacheItemProperties : ICacheItemProperties
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="CacheItemProperties" /> class.
        /// </summary>
        public CacheItemProperties()
        {
            this.Dependency = null;
            this.CachePriority = CacheItemPriority.Normal;
            this.Callback = null;
        }

        /// <summary>
        ///   Gets or sets the dependency.
        /// </summary>
        /// <value> The dependency. </value>
        public CacheDependency Dependency { get; set; }

        /// <summary>
        ///   Gets or sets the absolute expiration.
        /// </summary>
        /// <value> The absolute expiration. </value>
        public DateTime AbsoluteExpiration { get; set; }

        /// <summary>
        ///   Gets or sets the sliding expiration.
        /// </summary>
        /// <value> The sliding expiration. </value>
        public TimeSpan SlidingExpiration { get; set; }

        /// <summary>
        ///   Gets or sets the cache priority.
        /// </summary>
        /// <value> The cache priority. </value>
        public CacheItemPriority CachePriority { get; set; }

        /// <summary>
        ///   Gets or sets the callback.
        /// </summary>
        /// <value> The callback. </value>
        public Delegate Callback { get; set; }
    }
}