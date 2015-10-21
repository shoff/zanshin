namespace Zanshin.Domain.Providers.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Utilities;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using NLog;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// </summary>
    public class ApplicationClaimsIdentityFactory : ClaimsIdentityFactory<User, int>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger(typeof(ApplicationClaimsIdentityFactory));

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="user">The user.</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// manager
        /// or
        /// user
        /// </exception>
        public override Task<ClaimsIdentity> CreateAsync(UserManager<User, int> manager, User user, string authenticationType)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(authenticationType, this.UserNameClaimType, this.RoleClaimType);
            claimsIdentity.AddClaim(new Claim(this.UserIdClaimType, this.ConvertIdToString(user.Id), "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim(this.UserNameClaimType, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
            
            if (manager.SupportsUserSecurityStamp)
            {
                logger.Warn(
                    "manager.SupportsUserSecurityStamp is true but we are not recording claims, need to create IUserSecurityStampStore");
                // TODO this is broken RIGHT HERE
                // claimsIdentity.AddClaim(new Claim(this.SecurityStampClaimType, await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture<string>()));
            }
            if (manager.SupportsUserRole)
            {
                logger.Warn(
                    "manager.SupportsUserRole is true but we are not recording claims, need to create IUserSecurityStampStore");
                // TODO this is broken RIGHT HERE
                //IList<string> list = await manager.GetRolesAsync(user.Id).WithCurrentCulture<IList<string>>();
                //foreach (string current in list)
                //{
                //    claimsIdentity.AddClaim(new Claim(this.RoleClaimType, current, "http://www.w3.org/2001/XMLSchema#string"));
                //}
            }
            if (manager.SupportsUserClaim)
            {
                logger.Warn(
                    "manager.SupportsUserClaim is true but we are not recording claims, need to create IUserSecurityStampStore");
                // TODO this is broken RIGHT HERE
                //claimsIdentity.AddClaims(await manager.GetClaimsAsync(user.Id).WithCurrentCulture<IList<Claim>>());
            }

            return Task.FromResult(claimsIdentity);
        }
    }
}