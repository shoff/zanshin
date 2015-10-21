
namespace Zanshin.Domain.Factories
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using Castle.MicroKernel;

    using Zanshin.Domain.Services;

    /// <summary>
    /// Used to resolve the ApiController for WebApi
    /// </summary>
    public sealed class ApiControllerFactory : IHttpControllerActivator
    {
        private readonly Ioc container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerFactory"/> class.
        /// </summary>
        public ApiControllerFactory()
        {
            this.container = this.container ?? Ioc.Instance;
        }

        /// <summary>
        /// Creates an <see cref="T:System.Web.Http.Controllers.IHttpController" /> object.
        /// </summary>
        /// <param name="request">The message request.</param>
        /// <param name="controllerDescriptor">The HTTP controller descriptor.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// An <see cref="T:System.Web.Http.Controllers.IHttpController" /> object.
        /// </returns>
        /// <remarks>Not checking the controllerDescriptor as we don't care about its state.</remarks>
        /// <exception cref="ArgumentNullException">The value of 'request' cannot be null. </exception>
        /// <exception cref="ComponentNotFoundException">key</exception>
        public IHttpController Create(HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (controllerType == null)
            {
                throw new ArgumentNullException("controllerType");
            }

            var controller = this.container.Resolve(controllerType.FullName);

            // Adds the given resource to a list of resources that will be disposed
            // by a host once the request is disposed.
            request.RegisterForDispose(new Release(() => this.container.Release(controller)));
            return controller as IHttpController;
        }

        private sealed class Release : IDisposable
        {
            private readonly Action release;

            /// <summary>
            /// Initializes a new instance of the <see cref="Release"/> class.
            /// </summary>
            /// <param name="release">The release.</param>
            public Release(Action release)
            {
                this.release = release;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <exception cref="Exception">A delegate callback throws an exception.</exception>
            public void Dispose()
            {
                this.release();
            }
        }
    }
}