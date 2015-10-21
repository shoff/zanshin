
namespace Zanshin.Domain.Services
{
    using System.Web.Http.Controllers;
    using System.Web.Mvc;

    using Castle.MicroKernel.Registration;

    using Zanshin.Domain.Services.Interfaces;

    public sealed class ControllerRegistrationService : IControllerRegistrationService
    {
        private readonly IAssemblyDiscoveryService assemblyDiscoveryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRegistrationService"/> class.
        /// </summary>
        /// <param name="assemblyDiscoveryService">The dependency discovery service.</param>
        public ControllerRegistrationService(IAssemblyDiscoveryService assemblyDiscoveryService)
        {
            this.assemblyDiscoveryService = assemblyDiscoveryService;
        }

        /// <summary>
        /// Registers the controllers.
        /// </summary>
        public void RegisterControllers()
        {
            foreach (var assembly in this.assemblyDiscoveryService.AssemblyList)
            {
                Ioc.Instance.WindsorContainer.Register(Types.FromAssembly(assembly).BasedOn<IHttpController>().LifestyleTransient());
                Ioc.Instance.WindsorContainer.Register(Types.FromAssembly(assembly).BasedOn<IController>().LifestyleTransient());
            }
        }
    }
}