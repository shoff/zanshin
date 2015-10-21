namespace Zanshin.Domain.Services.Interfaces
{
    using System;
    using System.Web.Caching;

    using Zanshin.Domain.Filters.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        ///   Gets or sets the <see cref="System.Object" /> with the specified name.
        /// </summary>
        /// <value> </value>
        object this[string name] { get; set; }

        /// <summary>
        ///   Gets the count.
        /// </summary>
        /// <value> The count. </value>
        int Count { get; }

        /// <summary>
        ///   Gets the no absolute expiration.
        /// </summary>
        /// <value> The no absolute expiration. </value>
        DateTime NoAbsoluteExpiration { get; }

        /// <summary>
        ///   Gets the no sliding expiration.
        /// </summary>
        /// <value> The no sliding expiration. </value>
        TimeSpan NoSlidingExpiration { get; }

        /// <summary>
        ///   Gets or sets the cache expiration.
        /// </summary>
        /// <value> The cache expiration. </value>
        double CacheExpiration { get; set; }

        /// <summary>
        /// Tries to get the item from cache, if it does not exist, tries to invoke
        ///  the passed Func which should place the item in cache, and return the '
        /// now cached value. If the call fails, this method will return default(T).
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="cacheItemProperties">The cache item properties.</param>
        /// <returns></returns>
        /// <remarks>The string key is not checked in this method as the contains method
        /// does that check and this always calls that method.</remarks>
        /// <exception cref="ArgumentNullException">expression</exception>
        TResult TryGet<TResult>(string key, Func<TResult> serviceFunc, ICacheItemProperties cacheItemProperties);

        /// <summary>
        ///   Adds the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        void Add(string key, object value);

        /// <summary>
        ///   Adds the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <param name="absoluteExpiration"> The absolute expiration. </param>
        void Add(string key, object value, DateTime absoluteExpiration);

        /// <summary>
        /// Adds the item with the specified key to the cache with the
        /// designated sliding expiration.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <param name="slidingExpiration"> The sliding expiration. </param>
        void Add(string key, object value, TimeSpan slidingExpiration);

        /// <summary>
        ///  Adds the item with the specified key to the cache with full control
        /// over all aspects of the cache insertion.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <param name="dependencies"> The dependencies. </param>
        /// <param name="absoluteExpiration"> The absolute expiration. </param>
        /// <param name="slidingExpiration"> The sliding expiration. </param>
        /// <param name="priority"> The priority. </param>
        /// <param name="onRemoveCallback"> The on remove callback. </param>
        /// <exception cref="ArgumentNullException">Argument is
        ///   <see langword="null" />
        ///   .</exception>
        void Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
            TimeSpan slidingExpiration, CacheItemPriority priority = CacheItemPriority.Normal,
            Delegate onRemoveCallback = null);

        /// <summary>
        ///   Inserts the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <param name="dependencies"> The dependencies. </param>
        /// <param name="absoluteExpiration"> The absolute expiration. </param>
        /// <param name="slidingExpiration"> The sliding expiration. </param>
        /// <param name="priority"> The priority. </param>
        /// <param name="onRemoveCallback"> The on remove callback. </param>
        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
            TimeSpan slidingExpiration, CacheItemPriority priority = CacheItemPriority.Normal,
            Delegate onRemoveCallback = null);

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return default(T).
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        TResult TryGet<T1, TResult>(string key, T1 t1, Func<T1, TResult> serviceFunc, ICacheItemProperties properties);

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return null.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        TResult TryGet<T1, T2, TResult>(string key, T1 t1, T2 t2, Func<T1, T2, TResult> serviceFunc, ICacheItemProperties properties);

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return null.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">key</exception>
        TResult TryGet<T1, T2, T3, TResult>(string key, T1 t1, T2 t2, T3 t3,
            Func<T1, T2, T3, TResult> serviceFunc, ICacheItemProperties properties);

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return null.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        TResult TryGet<T1, T2, T3, T4, TResult>(string key, T1 t1, T2 t2, T3 t3, T4 t4, Func<T1, T2, T3, T4, TResult> serviceFunc,
            ICacheItemProperties properties);

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return null.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        TResult TryGet<T1, T2, T3, T4, T5, TResult>(string key, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, Func<T1, T2, T3, T4, T5, TResult> serviceFunc,
            ICacheItemProperties properties);

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return null.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="t6">The t6.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        TResult TryGet<T1, T2, T3, T4, T5, T6, TResult>(string key, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6,
            Func<T1, T2, T3, T4, T5, T6, TResult> serviceFunc, ICacheItemProperties properties);

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return null.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="t6">The t6.</param>
        /// <param name="t7">The t7.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        TResult TryGet<T1, T2, T3, T4, T5, T6, T7, TResult>(string key, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> serviceFunc, ICacheItemProperties properties);

        /// <summary>
        /// Removes the item assigned the supplied key
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> </returns>
        bool Remove(string key);

        /// <summary>
        ///   Determines whether [contains] [the specified key].
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> <c>true</c> if [contains] [the specified key]; otherwise, <c>false</c> . </returns>
        /// <remarks>
        ///   Should NEVER throw!
        /// </remarks>
        bool Contains(string key);

        /// <summary>
        ///   Invalidates the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <exception cref="ArgumentNullException">Argument is
        ///   <see langword="null" />
        ///   .</exception>
        /// <exception cref="InvalidOperationException">Cannot delete a value from a disabled cache.</exception>
        void Invalidate(string key);
    }
}