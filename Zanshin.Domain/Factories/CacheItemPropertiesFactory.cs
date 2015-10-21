namespace Zanshin.Domain.Factories
{
    using System;
    using System.Web.Caching;

    using Zanshin.Domain.Factories.Interfaces;
    using Zanshin.Domain.Filters;
    using Zanshin.Domain.Filters.Interfaces;
    using Zanshin.Domain.Helpers.Interfaces;

    public sealed class CacheItemPropertiesFactory : ICacheItemPropertiesFactory
    {
        private readonly int slidingExpiration;
        private readonly int absoluteExpiration;
        private readonly ICacheItemProperties defaultItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItemPropertiesFactory" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public CacheItemPropertiesFactory(IConfigurationWrapper config)
        {
            string key;
            if ((key = config.AppSettings[Constants.DefaultSlidingExpiration]) != null)
            {
                if (!int.TryParse(key, out this.slidingExpiration))
                {
                    this.slidingExpiration = 1;
                }
            }

            if ((key = config.AppSettings[Constants.DefaultAbsoluteExpiration]) != null)
            {
                if (!int.TryParse(key, out this.absoluteExpiration))
                {
                    this.absoluteExpiration = 10;
                }
            }
            this.defaultItem = this.Build();
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns></returns>
        public ICacheItemProperties Build()
        {
            CacheItemProperties cip = new CacheItemProperties
            {
                SlidingExpiration = TimeSpan.FromMinutes(this.slidingExpiration),
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(this.absoluteExpiration),
                CachePriority = CacheItemPriority.Normal
            };

            return cip;
        }

        /// <summary>
        /// Builds the specified sliding expiration.
        /// </summary>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="cacheItemPriority">The cache item priority.</param>
        /// <param name="dependency">The dependency.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public ICacheItemProperties Build(int slidingExpiration, int absoluteExpiration, CacheItemPriority cacheItemPriority,
            CacheDependency dependency, Delegate callback)
        {
            if (slidingExpiration < 0)
            {
                slidingExpiration = 0;
            }

            if (absoluteExpiration < 0)
            {
                absoluteExpiration = 0;
            }

            CacheItemProperties cip = new CacheItemProperties
            {
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration),
                AbsoluteExpiration = absoluteExpiration > 0 ? DateTime.Now + TimeSpan.FromMinutes(absoluteExpiration) : DateTime.MaxValue,
                CachePriority = CacheItemPriority.Normal
            };

            return cip;
        }

        /// <summary>
        /// Gets the default cacheItemProperties object.
        /// This will be created with the values passed in via the
        /// environment service or with a sliding of 1 and absolute of 10.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public ICacheItemProperties Default
        {
            get { return this.defaultItem; }
        }

    }
}