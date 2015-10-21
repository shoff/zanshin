
// ReSharper disable ExceptionNotDocumented
namespace Zanshin.Domain.Providers.Identity
{
    using System;
    using System.Data.Entity.Utilities;
    using System.Globalization;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using NLog;
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Extensions;
    using Zanshin.Domain.Providers.Interfaces;
    using Zanshin.Domain.Services;

    /// <summary>
    /// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    /// </summary>
    public sealed class ApplicationUserManager : UserManager<User, int>, IApplicationUserManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserManager" /> class.
        /// </summary>
        /// <param name="userStore">The store.</param>
        /// <param name="claimsIdentityFactory">The claims identity factory.</param>
        public ApplicationUserManager(IUserStore<User, int> userStore, IClaimsIdentityFactory<User, int> claimsIdentityFactory)
            : base(userStore)
        {
            this.ClaimsIdentityFactory = claimsIdentityFactory;
        }

        /// <summary>
        /// Creates the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref>
        ///         <name>value</name>
        ///     </paramref>
        ///     is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.-or-<paramref>
        ///         <name>value</name>
        ///     </paramref>
        ///     is <see cref="F:System.Double.PositiveInfinity" />.-or-<paramref>
        ///         <name>value</name>
        ///     </paramref>
        ///     is <see cref="F:System.Double.NegativeInfinity" />.</exception>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // IUserStore is the UserStoreProvider
            // this is a sucky hack ... but
            var manager = new ApplicationUserManager(
                Ioc.Instance.Resolve<IUserStore<User, int>>(), 
                Ioc.Instance.Resolve<IClaimsIdentityFactory<User, int>>());
            
            // Configure validation logic for user names
            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User, int>
            {
                MessageFormat = "Your security code is {0}"
            });

            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public override Task<IdentityResult> AddLoginAsync(int userId, UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            logger.Debug("Adding login {0}, {1}, {2}", userId, login.LoginProvider, login.ProviderKey);
            return base.AddLoginAsync(userId, login);
        }

        public override Task<IdentityResult> AddClaimAsync(int userId, Claim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            logger.Debug("Adding claim {0}, {1}, {2}, {3}", userId, claim.Issuer, claim.Type, claim.Value);
            return base.AddClaimAsync(userId, claim);
        }

        public override Task<IdentityResult> CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            logger.Debug("Adding user {0}, {1}", user.UserName, user.DisplayName??user.UserName );
            return base.CreateAsync(user);
        }

        public override Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }
            bool passwordOk = user.Password == password || user.Password == password.Sha256Hash();
            return Task.FromResult(passwordOk);
        }

        public override async Task<IdentityResult> ResetAccessFailedCountAsync(int userId)
        {
            
            IUserLockoutStore<User, int> userLockoutStore = this.Store as IUserLockoutStore<User, int>;
            User user = await this.FindByIdAsync(userId).WithCurrentCulture<User>();
            if (user == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Common.UserWithIdNotFound, userId));
            }
            IdentityResult result;

            var lockOutAttempts = await this.GetAccessFailedCountAsync(user.Id).WithCurrentCulture();
            
            if (lockOutAttempts <= this.MaxFailedAccessAttemptsBeforeLockout)
            {
                result = IdentityResult.Success;
            }
            else
            {
                await userLockoutStore.ResetAccessFailedCountAsync(user).WithCurrentCulture();
                result = await this.UpdateAsync(user).WithCurrentCulture();
            }
            return result;
        }

        public override async Task<int> GetAccessFailedCountAsync(int userId)
        {
            IUserLockoutStore<User, int> userLockoutStore = this.Store as IUserLockoutStore<User, int>;
            User user = await this.FindByIdAsync(userId).WithCurrentCulture();
            if (user == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Common.UserWithIdNotFound, userId));
            }
            return await userLockoutStore.GetAccessFailedCountAsync(user).WithCurrentCulture<int>();
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrWhiteSpace(authenticationType))
            {
                throw new ArgumentNullException("authenticationType");
            }
            return await this.ClaimsIdentityFactory.CreateAsync(this, user, authenticationType);
        }
    }

}
