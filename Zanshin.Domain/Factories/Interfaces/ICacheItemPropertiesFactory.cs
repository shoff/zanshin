namespace Zanshin.Domain.Factories.Interfaces
{
    using System;
    using System.Web.Caching;

    using Zanshin.Domain.Filters.Interfaces;

    public interface ICacheItemPropertiesFactory
    {
        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns></returns>
        ICacheItemProperties Build();

        /// <summary>
        /// Builds the specified sliding expiration.
        /// </summary>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="cacheItemPriority">The cache item priority.</param>
        /// <param name="dependency">The dependency.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        ICacheItemProperties Build(int slidingExpiration, int absoluteExpiration, CacheItemPriority cacheItemPriority,
            CacheDependency dependency, Delegate callback);

        /// <summary>
        /// Gets the default cacheItemProperties object.
        /// This will be created with the values passed in via the
        /// environment service or with a sliding of 1 and absolute of 10.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        ICacheItemProperties Default { get; }
    }
}