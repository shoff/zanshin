namespace Zanshin.Domain.Services
{
    using System;
    using System.Web;
    using System.Web.Caching;
    using NLog;
    using Zanshin.Domain.Factories.Interfaces;
    using Zanshin.Domain.Filters.Interfaces;
    using Zanshin.Domain.Services.Interfaces;

    /// <summary>
    ///   Ioc ready caching service.
    /// </summary>
    /// <remarks>
    /// Applications that are not hosted in a web aware host should not use this service.
    /// </remarks>
    public sealed class CacheService : ICacheService
    {
        private Cache cache;
        private double cacheDuration;
        private bool cacheIsValid;
        private readonly ICacheItemPropertiesFactory cacheItemsPropertiesFactory;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService" /> class.
        /// </summary>
        /// <param name="cacheItemsPropertiesFactory">The cache items properties factory.</param>
        public CacheService(ICacheItemPropertiesFactory cacheItemsPropertiesFactory)
        {
            this.cacheItemsPropertiesFactory = cacheItemsPropertiesFactory;
            this.Initialize();
        }

        /// <summary>
        ///   Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            HttpContext context = HttpContext.Current;

            if (!ReferenceEquals(null, context))
            {
                this.cache = context.Cache;
                this.cacheIsValid = true;
                this.cacheDuration = 0.0d;

            }
            else
            {
                // should throw here since any other environment should have 
                // a valid HttpContext.
                throw new ApplicationException(Common.HttpContextNull);
            }

        }

        /// <summary>
        ///   Gets or sets the <see cref="System.Object" /> with the specified name.
        /// </summary>
        /// <value> </value>
        /// <exception cref="ApplicationException">Cannot create cache object
        ///   from
        ///   <see langword="null" />
        ///   context.</exception>
        public object this[string name]
        {
            get
            {
                if (this.cache != null)
                {
                    return this.cache[name];
                }

                HttpContext context = HttpContext.Current;

                if (context != null)
                {
                    this.cache = context.Cache;
                    return this.cache[name];
                }
                throw new ApplicationException(Common.HttpContextNull);
            }
            set
            {
                if (this.cache != null)
                {
                    this.cache[name] = value;
                }
                else
                {
                    HttpContext context = HttpContext.Current;

                    if (context != null)
                    {
                        this.cache = context.Cache;
                        this.cache[name] = value;
                    }
                    else
                    {
                        throw new ApplicationException(Common.HttpContextNull);
                    }
                }
            }
        }

        /// <summary>
        /// Tries to get the item from cache, if it does not exist, tries to invoke
        ///  the passed Func which should place the item in cache, and return the '
        /// now cached value. If the call fails, this method will return default(T).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="cacheItemProperties">The cache item properties.</param>
        /// <returns></returns>
        /// <remarks>The string key is not checked in this method as the contains method
        /// does that check and this always calls that method.</remarks>
        /// <exception cref="ArgumentNullException">expression</exception>
        public T TryGet<T>(string key, Func<T> serviceFunc, ICacheItemProperties cacheItemProperties)
        {
            if (serviceFunc == null)
            {
                throw new ArgumentNullException("serviceFunc");
            }

            try
            {
                if (this.Contains(key))
                {
                    return (T)this.cache[key];
                }

                // not in cache so get the data with the service func,
                // cache it then return it.
                T t = serviceFunc.Invoke();
                this.AddTToCache(key, cacheItemProperties, t);
                return t;
            }
            catch (Exception e)
            {
                logger.ErrorException(e.Message, e);
                return default(T);
            }
        }

        private void AddTToCache<T>(string key, ICacheItemProperties cacheItemProperties, T t)
        {
            if (cacheItemProperties == null)
            {
                // just a set of vanilla cacheItemProperties that are safe for any object.
                cacheItemProperties = this.cacheItemsPropertiesFactory.Default;
            }
            if (!typeof(T).IsValueType)
            {
                if (ReferenceEquals(null, t))
                {
                    return;
                }
            }
            this.Add(key, t, cacheItemProperties.Dependency, cacheItemProperties.AbsoluteExpiration,
                cacheItemProperties.SlidingExpiration, cacheItemProperties.CachePriority, cacheItemProperties.Callback);
        }

        /// <summary>
        ///   Adds the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        public void Add(string key, object value)
        {
            this.Add(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
        }

        /// <summary>
        ///   Adds the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <param name="absoluteExpiration"> The absolute expiration. </param>
        public void Add(string key, object value, DateTime absoluteExpiration)
        {
            this.Add(key, value, null, absoluteExpiration, Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Adds the item with the specified key to the cache with the
        /// designated sliding expiration.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <param name="slidingExpiration"> The sliding expiration. </param>
        public void Add(string key, object value, TimeSpan slidingExpiration)
        {
            this.Add(key, value, null, Cache.NoAbsoluteExpiration, slidingExpiration);
        }

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
        public void Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
            TimeSpan slidingExpiration, CacheItemPriority priority = CacheItemPriority.Normal,
            Delegate onRemoveCallback = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (!this.cacheIsValid)
            {
                return;
            }

            this.cache.Add(key, value, dependencies, absoluteExpiration,
                slidingExpiration, priority, (CacheItemRemovedCallback)onRemoveCallback);
        }

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
        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
            TimeSpan slidingExpiration, CacheItemPriority priority = CacheItemPriority.Normal,
            Delegate onRemoveCallback = null)
        {

            if (!this.cacheIsValid)
            {
                return;
            }

            this.cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration,
                priority, (CacheItemRemovedCallback)onRemoveCallback);
        }

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return default(T).
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public T TryGet<T1, T>(string key, T1 t1, Func<T1, T> serviceFunc, ICacheItemProperties properties)
        {
            try
            {
                if (this.Contains(key))
                {
                    return (T)this.cache[key];
                }

                T t = serviceFunc.Invoke(t1);
                this.AddTToCache(key, properties, t);
                return t;
            }
            catch (Exception e)
            {
                logger.ErrorException(e.Message, e);
                return default(T);
            }
        }

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return null.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public T TryGet<T1, T2, T>(string key, T1 t1, T2 t2, Func<T1, T2, T> serviceFunc, ICacheItemProperties properties)
        {
            try
            {
                if (this.Contains(key))
                {
                    return (T)this.cache[key];
                }

                T t = serviceFunc.Invoke(t1, t2);
                this.AddTToCache(key, properties, t);
                return t;
            }
            catch (Exception e)
            {
                logger.ErrorException(e.Message, e);
                return default(T);
            }
        }

        /// <summary>
        /// Tries to get the item from cache, if it does not
        /// exist, tries to invoke the passed Func which should place the
        /// item in cache, and return the now cached value. If the call fails,
        /// this method will return null.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public T TryGet<T1, T2, T3, T>(string key, T1 t1, T2 t2, T3 t3,
            Func<T1, T2, T3, T> serviceFunc, ICacheItemProperties properties)
        {
            try
            {
                if (this.Contains(key))
                {
                    return (T)this.cache[key];
                }

                T t = serviceFunc.Invoke(t1, t2, t3);
                this.AddTToCache(key, properties, t);
                return t;
            }
            catch (Exception e)
            {
                logger.ErrorException(e.Message, e);
                return default(T);
            }
        }

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
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public T TryGet<T1, T2, T3, T4, T>(string key, T1 t1, T2 t2, T3 t3, T4 t4, Func<T1, T2, T3, T4, T> serviceFunc,
            ICacheItemProperties properties)
        {
            try
            {
                if (this.Contains(key))
                {
                    return (T)this.cache[key];
                }

                T t = serviceFunc.Invoke(t1, t2, t3, t4);
                this.AddTToCache(key, properties, t);
                return t;
            }
            catch (Exception e)
            {
                logger.ErrorException(e.Message, e);
                return default(T);
            }
        }

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
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <param name="t5">The t5.</param>
        /// <param name="serviceFunc">The service func.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public T TryGet<T1, T2, T3, T4, T5, T>(string key, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, Func<T1, T2, T3, T4, T5, T> serviceFunc,
            ICacheItemProperties properties)
        {
            try
            {
                if (this.Contains(key))
                {
                    return (T)this.cache[key];
                }

                T t = serviceFunc.Invoke(t1, t2, t3, t4, t5);
                this.AddTToCache(key, properties, t);
                return t;
            }
            catch (Exception e)
            {
                logger.ErrorException(e.Message, e);
                return default(T);
            }
        }

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
        /// <typeparam name="T"></typeparam>
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
        public T TryGet<T1, T2, T3, T4, T5, T6, T>(string key, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6,
            Func<T1, T2, T3, T4, T5, T6, T> serviceFunc, ICacheItemProperties properties)
        {
            try
            {
                if (this.Contains(key))
                {
                    return (T)this.cache[key];
                }

                T t = serviceFunc.Invoke(t1, t2, t3, t4, t5, t6);
                this.AddTToCache(key, properties, t);
                return t;
            }
            catch (Exception e)
            {
                logger.ErrorException(e.Message, e);
                return default(T);
            }
        }

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
        /// <typeparam name="T"></typeparam>
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
        public T TryGet<T1, T2, T3, T4, T5, T6, T7, T>(string key, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7,
            Func<T1, T2, T3, T4, T5, T6, T7, T> serviceFunc, ICacheItemProperties properties)
        {
            try
            {
                if (this.Contains(key))
                {
                    return (T)this.cache[key];
                }

                T t = serviceFunc.Invoke(t1, t2, t3, t4, t5, t6, t7);
                this.AddTToCache(key, properties, t);
                return t;
            }
            catch (Exception e)
            {
                logger.ErrorException(e.Message, e);
                return default(T);
            }
        }

        /// <summary>
        /// Removes the item assigned the supplied key
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> </returns>
        public bool Remove(string key)
        {
            bool result = false;

            try
            {
                this.Invalidate(key);
                result = true;
            }
            catch (ArgumentNullException ane)
            {
                logger.ErrorException(ane.Message, ane);
            }
            catch (InvalidOperationException ioe)
            {
                logger.ErrorException(ioe.Message, ioe);
            }

            return result;
        }

        /// <summary>
        ///   Determines whether [contains] [the specified key].
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns> <c>true</c> if [contains] [the specified key]; otherwise, <c>false</c> . </returns>
        /// <remarks>
        ///   Should NEVER throw!
        /// </remarks>
        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            if (!this.cacheIsValid)
            {
                return false;
            }

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (this[key] != null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                logger.ErrorException(e.Message, e);
                return false;
            }

            return false;
        }

        /// <summary>
        ///   Invalidates the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <exception cref="ArgumentNullException">Argument is
        ///   <see langword="null" />
        ///   .</exception>
        public void Invalidate(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            if (this.cacheIsValid)
            {
                this.cache.Remove(key);
            }
        }

        /// <summary>
        ///   Gets the count.
        /// </summary>
        /// <value> The count. </value>
        public int Count
        {
            get { return this.cacheIsValid ? this.cache.Count : 0; }
        }

        /// <summary>
        ///   Gets the no absolute expiration.
        /// </summary>
        /// <value> The no absolute expiration. </value>
        public DateTime NoAbsoluteExpiration
        {
            get { return DateTime.MaxValue; }
        }

        /// <summary>
        ///   Gets the no sliding expiration.
        /// </summary>
        /// <value> The no sliding expiration. </value>
        public TimeSpan NoSlidingExpiration
        {
            get { return TimeSpan.Zero; }
        }

        /// <summary>
        ///   Gets or sets the cache expiration.
        /// </summary>
        /// <value> The cache expiration. </value>
        public double CacheExpiration
        {
            get
            {
                if (this.cacheDuration > 1)
                {
                    return 20d;
                }

                return this.cacheDuration;
            }
            set { this.cacheDuration = value; }
        }
    }
}