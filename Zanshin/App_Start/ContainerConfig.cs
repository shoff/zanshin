
namespace Zanshin
{
    using Castle.Core;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Zanshin.Domain.Data;
    using Zanshin.Domain.Data.Interfaces;
    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Providers;
    using Zanshin.Domain.Providers.Identity;
    using Zanshin.Domain.Providers.Interfaces;
    using Zanshin.Domain.Repositories;
    using Zanshin.Domain.Repositories.Interfaces;
    using Zanshin.Domain.Services;
    using Zanshin.Domain.Services.Interfaces;
    using Zanshin.WebApi.Services;

    public class ContainerConfig
    {
        private static IAssemblyDiscoveryService assemblyDiscoveryService;

        public static void RegisterComponents()
        {
            assemblyDiscoveryService = Ioc.Instance.Resolve<IAssemblyDiscoveryService>();
            assemblyDiscoveryService.GenerateDependencyList();

            IControllerRegistrationService controllerRegistrationService = Ioc.Instance.Resolve<IControllerRegistrationService>();
            controllerRegistrationService.RegisterControllers();
            // Data
            Ioc.Instance.AddComponentWithLifestyle("IDataContext", typeof(IDataContext), typeof(DataContext), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("IUserService", typeof(IUserService), typeof(UserService), LifestyleType.PerWebRequest);


            // Identity
            //Ioc.Instance.AddComponentWithLifestyle("GroupManager", typeof(IGroupManager), typeof(ApplicationRoleManager), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("ApplicationSignInManager", typeof(IApplicationSignInManager), typeof(ApplicationSignInManager), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("ApplicationUserManager", typeof(IApplicationUserManager), typeof(ApplicationUserManager), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("AuthenticationManager", typeof(IAuthenticationManager),typeof(ApplicationAuthenticationManager), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("IUserStore", typeof(IUserStore<User, int>), typeof(UserStoreProvider<User, int>), LifestyleType.Transient);
            Ioc.Instance.AddComponentWithLifestyle("ClaimsIdentityFactory", typeof(IClaimsIdentityFactory<User, int>), typeof(ApplicationClaimsIdentityFactory), LifestyleType.PerWebRequest);

            
            // Forums
            Ioc.Instance.AddComponentWithLifestyle("IForumRepository", typeof(IEntityRepository<Forum, int>),  typeof(EntityRepository<Forum, int>), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("ITopicRepository", typeof(IEntityRepository<Topic, int>),  typeof(EntityRepository<Topic, int>), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("IUserRepository",  typeof(IEntityRepository<User,   int>), typeof(EntityRepository<User,  int>), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("IUserClaimRepository", typeof(IEntityRepository<UserClaim, int>), typeof(EntityRepository<UserClaim, int>), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("IUserLoginRepository", typeof(IEntityRepository<UserLogin, int>), typeof(EntityRepository<UserLogin, int>), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("ILogRepository", typeof(IEntityRepository<Log, int>), typeof(EntityRepository<Log, int>), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("IWebsiteRepository", typeof(IEntityRepository<Website, int>), typeof(EntityRepository<Website, int>), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("IPrivateMessageRepository", typeof(IEntityRepository<PrivateMessage, int>), typeof(EntityRepository<PrivateMessage, int>), LifestyleType.PerWebRequest);



            Ioc.Instance.AddComponentWithLifestyle("IPostRepository", typeof(IEntityRepository<Post, int>), typeof(EntityRepository<Post, int>), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("ICategoryRepository", typeof(IEntityRepository<Category, int>), typeof(EntityRepository<Category, int>), LifestyleType.PerWebRequest);
            Ioc.Instance.AddComponentWithLifestyle("IGroupRepository", typeof(IEntityRepository<Group, int>), typeof(EntityRepository<Group, int>), LifestyleType.PerWebRequest);
            // container.Register(AllTypes.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient());

            // api registrations
            Ioc.Instance.AddComponentWithLifestyle("IForumApi", typeof(IApiRepository<Forum>), typeof(ApiRepository<Forum>), LifestyleType.PerWebRequest);
        }
    }
}