namespace Zanshin.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http.Dispatcher;
    using Castle.Core;
    using Castle.Core.Internal;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Zanshin.Domain.Factories;
    using Zanshin.Domain.Factories.Interfaces;
    using Zanshin.Domain.Helpers;
    using Zanshin.Domain.Helpers.Interfaces;
    using Zanshin.Domain.Services.Interfaces;

    /// <summary>
    /// An implementation of the IContainer interface using Castle Windsor as the 
    /// container.
    /// </summary>
    public sealed class Ioc
    {
        // TODO: Ioc controller that displays information about all of the items in the container for an administration site.

        private static readonly object syncRoot = new object();
        private static readonly RegisteredComponents registeredComponents = new RegisteredComponents();
        private static readonly Ioc instance = new Ioc();
        private bool initialized;

        private Ioc()
        {
            // this is a static lock, do not change it to an
            // instance lock!
            lock (syncRoot)
            {
                this.Initialize();
            }
        }

        /// <summary>
        ///   Gets the instance.
        /// </summary>
        public static Ioc Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        ///   Gets the Windsor container.
        /// </summary>
        public WindsorContainer WindsorContainer
        {
            get;
            private set;
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="classType">Type of the class.</param>
        /// <exception cref="System.ArgumentNullException">
        /// key
        /// or
        /// classType
        /// </exception>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public void AddComponent(string key, Type classType)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (classType == null)
            {
                throw new ArgumentNullException("classType");
            }

#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For(classType).Named(key));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,

                // ServiceType = typeof(T),
                ClassType = classType,
                LifeStyle = LifestyleType.Singleton
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="classType">Type of the class.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// classType
        /// or
        /// key
        /// </exception>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public void AddComponent(string key, Type serviceType, Type classType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            if (classType == null)
            {
                throw new ArgumentNullException("classType");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For(serviceType).ImplementedBy(classType).Named(key));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,
                ServiceType = serviceType,
                ClassType = classType,
                LifeStyle = LifestyleType.Singleton
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Adds the component with lifestyle.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="classType">Type of the class.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        /// <exception cref="System.ArgumentNullException">
        /// classType
        /// or
        /// key
        /// </exception>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public void AddComponentWithLifestyle(string key, Type classType, LifestyleType lifestyle)
        {
            if (classType == null)
            {
                throw new ArgumentNullException("classType");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For(classType).Named(key).LifeStyle.Is(lifestyle));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,
                ClassType = classType,
                LifeStyle = lifestyle
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Adds the component with lifestyle.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="classType">Type of the class.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// classType
        /// or
        /// key
        /// </exception>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public void AddComponentWithLifestyle(string key, Type serviceType, Type classType, LifestyleType lifestyle)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            if (classType == null)
            {
                throw new ArgumentNullException("classType");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For(serviceType).ImplementedBy(classType).Named(key).LifeStyle.Is(lifestyle));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,
                ServiceType = serviceType,
                ClassType = classType,
                LifeStyle = lifestyle
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        ///   Adds the component.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <remarks>
        ///   This method is thread-safe.
        /// </remarks>
        public void AddComponent<T>()
            where T : class
        {
#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For<T>());
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = typeof(T).ToString(),

                // ServiceType = typeof(T),
                ClassType = typeof(T),
                LifeStyle = LifestyleType.Singleton
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public void AddComponent<T>(string key)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For<T>().Named(key));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,
                ClassType = typeof(T),
                LifeStyle = LifestyleType.Singleton
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        ///   Adds the component with lifestyle.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="lifestyle"> The lifestyle. </param>
        public void AddComponentWithLifestyle<T>(LifestyleType lifestyle)
            where T : class
        {
            this.WindsorContainer.Register(Component.For<T>().LifeStyle.Is(lifestyle));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = typeof(T).ToString(),

                //ServiceType = typeof(T),
                ClassType = typeof(T),
                LifeStyle = lifestyle
            });
        }

        /// <summary>
        /// Adds the component with lifestyle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public void AddComponentWithLifestyle<T>(string key, LifestyleType lifestyle)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For<T>().Named(key).LifeStyle.Is(lifestyle));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,

                //ServiceType = typeof(T),
                ClassType = typeof(T),
                LifeStyle = lifestyle
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        ///   Adds the component.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <typeparam name="TU"> The type of the U. </typeparam>
        /// <remarks>
        ///   This method is thread-safe.
        /// </remarks>
        public void AddComponent<T, TU>()
            where TU : class, T
            where T : class
        {
#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For<T>().ImplementedBy<TU>());
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = typeof(T).ToString(),
                ServiceType = typeof(T),
                ClassType = typeof(TU),
                LifeStyle = LifestyleType.Singleton
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU">The type of the U.</typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public void AddComponent<T, TU>(string key)
            where TU : class, T
            where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For<T>().ImplementedBy<TU>().Named(key));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,
                ServiceType = typeof(T),
                ClassType = typeof(TU),
                LifeStyle = LifestyleType.Singleton
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        ///   Adds the component with lifestyle.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <typeparam name="TU"> The type of the U. </typeparam>
        /// <param name="lifestyle"> The lifestyle. </param>
        /// <remarks>
        ///   This method is thread-safe.
        /// </remarks>
        public void AddComponentWithLifestyle<T, TU>(LifestyleType lifestyle)
            where TU : class, T
            where T : class
        {
#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For<T>().ImplementedBy<TU>().LifeStyle.Is(lifestyle));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = typeof(T).ToString(),
                ServiceType = typeof(T),
                ClassType = typeof(TU),
                LifeStyle = lifestyle
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Adds the component with lifestyle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU">The type of the U.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public void AddComponentWithLifestyle<T, TU>(string key, LifestyleType lifestyle)
            where T : class
            where TU : T
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For<T>().ImplementedBy<TU>().Named(key).LifeStyle.Is(lifestyle));
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,
                ServiceType = typeof(T),
                ClassType = typeof(TU),
                LifeStyle = lifestyle
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Adds the component with dependency (dependency can be a constructor parameter or public property).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU">The type of the U.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        /// <param name="dependencyPropertyName">Name of the dependency property.</param>
        /// <param name="dependencyPropertyValue">The dependency property value.</param>
        /// <exception cref="System.ArgumentNullException">
        /// dependencyPropertyValue
        /// or key or key
        /// </exception>
        public void AddComponentWithDependency<T, TU>(
            string key, LifestyleType lifestyle, string dependencyPropertyName,
            object dependencyPropertyValue)
            where T : class
            where TU : T
        {
            if (dependencyPropertyValue == null)
            {
                throw new ArgumentNullException("dependencyPropertyValue");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (string.IsNullOrWhiteSpace(dependencyPropertyName))
            {
                throw new ArgumentNullException("key");
            }

#if !DEBUG
            using (TimedLock.Lock(syncRoot))
            {
#endif
            this.WindsorContainer.Register(Component.For<T>().ImplementedBy<TU>().DependsOn(
                Dependency.OnValue(dependencyPropertyName, dependencyPropertyValue)).LifeStyle.Is(lifestyle));

            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,
                ServiceType = typeof(T),
                ClassType = typeof(TU),
                LifeStyle = lifestyle
            });
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Resolves an object with the specified key and specified service
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// service
        /// or
        /// key
        /// </exception>
        public object Resolve(string key, Type service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            return this.WindsorContainer.Resolve(key, service);
        }


        /// <summary>
        /// Resolves an object with the specified key and specified service asynchronously.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// service
        /// or
        /// key
        /// </exception>
        public async Task<object> ResolveAsync(string key, Type service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            return await Task.Run(() => this.WindsorContainer.Resolve(key, service));
        }

        /// <summary>
        /// Resolves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">key</exception>
        /// <exception cref="Castle.MicroKernel.ComponentNotFoundException">
        /// key
        /// or
        /// key
        /// </exception>
        /// <exception cref="ArgumentNullException">The value of 'key' cannot be null.</exception>
        /// <exception cref="ComponentNotFoundException">key</exception>
        /// <remarks>
        /// This method is dangerous! Be sure you know what you are doing if you use it!
        /// </remarks>
        public object Resolve(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            // TODO validate this
            var componentModel = (ComponentModel)this.WindsorContainer.Kernel.GraphNodes
                .Find(x => ((ComponentModel)x).ComponentName.Name == key);

            if (!ReferenceEquals(null, componentModel))
            {
                var serviceType = componentModel.Services.ToArray()[0];
                var item = this.WindsorContainer.Resolve(key, serviceType);

                if (ReferenceEquals(null, item))
                {
                    throw new ComponentNotFoundException("key",
                        string.Format("Could not resolve component named {0}", key));
                }

                return item;
            }
            throw new ComponentNotFoundException("key",
                string.Format("Could not resolve component named {0}", key));
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">service</exception>
        public object Resolve(Type service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            return this.WindsorContainer.Resolve(service);
        }

        /// <summary>
        ///   Releases the specified instance.
        /// </summary>
        /// <param name="instanceParam"> The instance. </param>
        public void Release(object instanceParam)
        {
            if (instanceParam == null)
            {
                // if it's already null, just bail
                return;
            }
            this.WindsorContainer.Release(instanceParam);
        }

        /// <summary>
        /// Releases the specified instance asynchronously.
        /// </summary>
        /// <param name="instanceParam">The instance parameter.</param>
        /// <returns></returns>
        public async Task ReleaseAsync(object instanceParam)
        {
            await Task.Run(() => this.Release(instanceParam));
        }


        /// <summary>
        /// Resolves an object specified type T.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        public T Resolve<T>()
        {
            return this.WindsorContainer.Resolve<T>();
        }

        /// <summary>
        /// Resolves an object specified type T asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> ResolveAsync<T>()
        {
            return await Task.Run(() => this.WindsorContainer.Resolve<T>());
        }

        /// <summary>
        /// Resolves an object with the specified key and type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public T Resolve<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            return this.WindsorContainer.Resolve<T>(key);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="classType">Type of the class.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        /// <exception cref="System.ArgumentNullException">
        /// serviceType
        /// or
        /// classType
        /// or
        /// key
        /// </exception>
        /// <exception cref="ArgumentNullException">The value of 'serviceType' cannot be null.</exception>
        public void Replace(string key, Type serviceType, Type classType, LifestyleType lifestyle = LifestyleType.Singleton)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            if (classType == null)
            {
                throw new ArgumentNullException("classType");
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            // add the new component to the internal component list,
            // or updates an existing component if one is found with the 
            // same key.
            registeredComponents.AddComponent(new RegisteredComponent
            {
                Key = key,
                ServiceType = serviceType,
                ClassType = classType,
                LifeStyle = lifestyle
            });

            this.initialized = false;
            this.Initialize();
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public bool ContainsKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            bool result = false;

            foreach (var component in this.WindsorContainer.Kernel.GraphNodes)
            {
                ComponentModel cm = (ComponentModel)component;
                if (cm.Name.Equals(key))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ioc"/> class.
        /// </summary>
        internal void Initialize()
        {
            if (!this.initialized)
            {
#if !DEBUG
                using (TimedLock.Lock(syncRoot))
                {
#endif
                this.WindsorContainer = new WindsorContainer();

                //windsorContainer.Register(
                //    Component.For(typeof(IContainer)).Activator<ContainerActivator>()
                //    .ImplementedBy(typeof(Ioc)).Named("Ioc").LifeStyle.Is(LifestyleType.Singleton));

                foreach (var component in registeredComponents)
                {
                    if (component.ServiceType != null)
                    {
                        this.WindsorContainer.Register(
                            Component.For(component.ServiceType).ImplementedBy(component.ClassType).Named(component.Key).LifeStyle.
                                Is(component.LifeStyle));
                    }
                    else
                    {
                        this.WindsorContainer.Register(
                            Component.For(component.ClassType).Named(component.Key).LifeStyle.Is(component.LifeStyle));
                    }
                }
                this.initialized = true;
#if !DEBUG  
                }
#endif
            }
        }

        internal sealed class RegisteredComponent
        {
            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            /// <value>
            /// The key.
            /// </value>
            public string Key
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the type of the service.
            /// </summary>
            /// <value>
            /// The type of the service.
            /// </value>
            public Type ServiceType
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the type of the class.
            /// </summary>
            /// <value>
            /// The type of the class.
            /// </value>
            public Type ClassType
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the life style.
            /// </summary>
            /// <value>
            /// The life style.
            /// </value>
            public LifestyleType LifeStyle
            {
                get;
                set;
            }
        }

        internal sealed class RegisteredComponents : List<RegisteredComponent>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RegisteredComponents"/> class.
            /// </summary>
            internal RegisteredComponents()
            {
                this.Add(new RegisteredComponent
                {
                    Key = "ConfigurationWrapper",
                    ServiceType = typeof(IConfigurationWrapper),
                    ClassType = typeof(ConfigurationWrapper),
                    LifeStyle = LifestyleType.Transient
                });

                this.Add(new RegisteredComponent
                {
                    Key = "CacheItemPropertiesFactory",
                    ServiceType = typeof(ICacheItemPropertiesFactory),
                    ClassType = typeof(CacheItemPropertiesFactory),
                    LifeStyle = LifestyleType.Singleton
                });

                this.Add(new RegisteredComponent
                {
                    Key = "InternalControllerFactory",
                    ServiceType = typeof(IForumControllerFactory),
                    ClassType = typeof(ControllerFactory),
                    LifeStyle = LifestyleType.Singleton
                });

                this.Add(new RegisteredComponent
                {
                    Key = "ControllerRegistrationService",
                    ServiceType = typeof(IControllerRegistrationService),
                    ClassType = typeof(ControllerRegistrationService),
                    LifeStyle = LifestyleType.Singleton
                });

                this.Add(new RegisteredComponent
                {
                    Key = "ApiControllerFactory",
                    ServiceType = typeof(IHttpControllerActivator),
                    ClassType = typeof(ApiControllerFactory),
                    LifeStyle = LifestyleType.Singleton
                });

                this.Add(new RegisteredComponent
                {
                    Key = "CacheService",
                    ServiceType = typeof(ICacheService),
                    ClassType = typeof(CacheService),
                    LifeStyle = LifestyleType.Singleton
                });

                this.Add(new RegisteredComponent
                {
                    Key = "DependencyDiscoveryService",
                    ServiceType = typeof(IAssemblyDiscoveryService),
                    ClassType = typeof(AssemblyDiscoveryService),
                    LifeStyle = LifestyleType.Singleton
                });
            }

            /// <summary>
            /// Adds the component.
            /// </summary>
            /// <param name="registeredComponent">The registered component.</param>
            /// <exception cref="ArgumentNullException">The value of 'registeredComponent' cannot be null. </exception>
            public void AddComponent(RegisteredComponent registeredComponent)
            {
                if (registeredComponent == null)
                {
                    throw new ArgumentNullException("registeredComponent");
                }

                // could we just set c's values here???
                var c = (from p in this
                         where p.Key.ToUpperInvariant() == registeredComponent.Key.ToUpperInvariant()
                         select p).FirstOrDefault();
                if (c != null)
                {
                    var x = this.FindIndex(y => y.Key.ToUpperInvariant() == registeredComponent.Key.ToUpperInvariant());
                    this[x].Key = registeredComponent.Key;
                    this[x].ServiceType = registeredComponent.ServiceType;
                    this[x].ClassType = registeredComponent.ClassType;
                    this[x].LifeStyle = registeredComponent.LifeStyle;
                }
                else
                {
                    this.Add(registeredComponent);
                }
            }
        }
    }
}