using System.Configuration;
using System.Text;
using NLog;
using Zanshin.Domain.Extensions;

namespace Zanshin.Domain.Factories
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Castle.MicroKernel;

    using Zanshin.Domain.Factories.Interfaces;
    using Zanshin.Domain.Services;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ControllerFactory : DefaultControllerFactory, IForumControllerFactory
    {
        private static readonly Logger logger = LogManager.GetLogger("ControllerFactory");

        /// <summary>
        ///   Releases the controller.
        /// </summary>
        /// <param name="controller"> The controller. </param>
        public override void ReleaseController(IController controller)
        {
            if (controller == null)
            {
                return;
            }
            Ioc.Instance.WindsorContainer.Release(controller);
        }

        /// <summary>
        /// tries to get the controller instance from the Windsor container. If the container does not have a type requested,
        ///  it will throw a <see cref="ComponentNotFoundException" /> . We catch these and add the controller
        /// type to the container. BUYER BEWARE: Any controller that needs to maintain state for a given
        /// request MUST define the controller in the Castle.config and set the lifestyle to transient.
        /// While you will likely see your controller work fine on your dev machine, if you don't define it in the config,
        /// Castle will create it as a singleton! So in Meridian speak this would be like having a singleton presenter.
        /// Why is that bad? Imagine a singleton <c>AccountController</c> being shared by 2 connections... smh
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// A reference to the controller.
        /// </returns>
        /// <exception cref="HttpException"><c>HttpException</c>
        /// .</exception>
        /// <exception cref="ArgumentNullException">The value of 'controllerType' cannot be null. </exception>
        /// <exception cref="ApplicationException">Controller type should be resolved by API . </exception>
        /// <exception cref="ComponentNotFoundException">key</exception>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException("controllerType");
            }

            if ((typeof(IHttpController)).IsAssignableFrom(controllerType))
            {
                if (requestContext != null)
                {
                    // log it
                    StringBuilder sb = new StringBuilder();
                    requestContext.RouteData.Values.Each(x => sb.AppendFormat("{0} - {1} ", x.Key, x.Value));
                    logger.Error(sb.ToString());
                }
                if (ConfigurationManager.AppSettings["throwOn404"] == "true")
                {
                    throw new ApplicationException(string.Format("{0} should not be resolving to normal MVC controller factory.",
                    controllerType));
                }
            }

            // only do this if the request context is not null, which I haven't been able to make null, but you never know.

            return (IController)Ioc.Instance.Resolve(controllerType.FullName);
        }
    }
}