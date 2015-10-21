namespace Zanshin.Domain.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Zanshin.Domain.Data;
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Providers.Interfaces;
    using Zanshin.Domain.Repositories.Interfaces;

    /// <summary>
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public sealed class UserStoreProvider<TUser, TKey> :
        IUserLoginStore<TUser, TKey>,
        IUserClaimStore<TUser, TKey>,
        IUserRoleStore<TUser, TKey>,
        IUserPasswordStore<TUser, TKey>,
        IUserSecurityStampStore<TUser, TKey>,
        IQueryableUserStore<TUser, TKey>,
        IUserEmailStore<TUser, TKey>,
        IUserPhoneNumberStore<TUser, TKey>,
        IUserTwoFactorStore<TUser, TKey>,
        IUserLockoutStore<TUser, TKey>,
        IUserStoreProvider<TUser, TKey>

        // constraints
        where TUser : User, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IEntityRepository<User, int> userRepository;
        private readonly IEntityRepository<UserClaim, int> userClaimRepository;
        private readonly IEntityRepository<UserLogin, int> userLoginRepository;

        /// <summary>
        /// Initializes a new instance of the <see><cref>UserStoreProvider</cref></see>
        /// class.
        /// </summary>
        /// <param name="userRepository">The user store.</param>
        /// <param name="userClaimRepository">The user claim repository.</param>
        /// <param name="userLoginRepository">The user login repository.</param>
        public UserStoreProvider(IEntityRepository<User, int> userRepository,
            IEntityRepository<UserClaim, int> userClaimRepository, IEntityRepository<UserLogin, int> userLoginRepository)
        {
            this.Context = (DataContext)userRepository.Context;
            this.AutoSaveChanges = true;

            // this.userStore = (IEntityStore<TUser>)userRepository;
            this.userRepository = userRepository;
            this.userClaimRepository = userClaimRepository;
            this.userLoginRepository = userLoginRepository;

            //this.logins = this.Context.SetEntity<TUserLogin>();
            //this.userClaims = this.Context.SetEntity<TUserClaim>();

        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IQueryable<TUser> Users
        {
            get
            {
                // TODO ugly hack
                var collection = this.userRepository.Get().ToList();
                List<TUser> queryAble = new List<TUser>();
                collection.ForEach(x => queryAble.Add((TUser)x));
                return queryAble.AsQueryable();
            }
        }

        /// <summary>
        /// Gets the claims asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {

            return await Task.Run(() => (from c in user.Claims
                                         select new Claim(c.ClaimType, c.ClaimValue)).ToList<Claim>());
        }

        /// <summary>
        /// Adds the claim asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        public async Task AddClaimAsync(TUser user, Claim claim)
        {


            await Task.Run(() => user.Claims.Add(new UserClaim
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            }));
        }

        /// <summary>
        /// Removes the claim asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        public async Task RemoveClaimAsync(TUser user, Claim claim)
        {


            await Task.Run(() =>
            {
                var list =
                    (from uc in user.Claims
                     where uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type
                     select uc).ToList();

                list.ForEach(uc => user.Claims.Remove(uc));

                //foreach (UserClaim current in list)
                //{
                //    user.Claims.Remove(current);
                //}

                //var queryable = from uc in this.userClaims
                //    where uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type select uc;

                var queryable =
                    this.userClaimRepository.Get(
                        uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);

                foreach (var current2 in queryable)
                {
                    this.userClaimRepository.Delete(current2);
                }
            });
        }

        /// <summary>
        /// Sets the email asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="ArgumentNullException">The value of 'user' cannot be null.</exception>
        public async Task SetEmailAsync(TUser user, string email)
        {

            await Task.Run(() => user.Email = email);
        }

        /// <summary>
        /// Gets the email asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<string> GetEmailAsync(TUser user)
        {

            return await Task.FromResult(user.Email);
        }

        /// <summary>
        /// Gets the email confirmed asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {

            return Task.FromResult(user.EmailConfirmed);
        }

        /// <summary>
        /// Sets the email confirmed asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="confirmed">if set to <c>true</c> [confirmed].</param>
        /// <returns></returns>
        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {

            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Finds the by email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public async Task<TUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }
            var user = await this.userRepository.GetAsync(u => u.Email.ToUpper() == email.ToUpper());

            if (user != null)
            {
                return (TUser)user.FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Gets the lockout end date asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {

            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : default(DateTimeOffset));
        }

        /// <summary>
        /// Sets the lockout end date asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="lockoutEnd">The lockout end.</param>
        /// <returns></returns>
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {

            user.LockoutEndDateUtc = ((lockoutEnd == DateTimeOffset.MinValue) ? null : new DateTime?(lockoutEnd.UtcDateTime));
            return Task.FromResult(0);
        }

        /// <summary>
        /// Increments the access failed count asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<int> IncrementAccessFailedCountAsync(TUser user)
        {

            user.AccessFailedCount++;
            return await Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Resets the access failed count asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task ResetAccessFailedCountAsync(TUser user)
        {

            user.AccessFailedCount = 0;
            await Task.FromResult(0);
        }

        /// <summary>
        /// Gets the access failed count asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<int> GetAccessFailedCountAsync(TUser user)
        {

            return await Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Gets the lockout enabled asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<bool> GetLockoutEnabledAsync(TUser user)
        {

            return await Task.FromResult(user.LockoutEnabled);
        }

        /// <summary>
        /// Sets the lockout enabled asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns></returns>
        public async Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {

            await Task.Run(() => user.LockoutEnabled = enabled);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task CreateAsync(TUser user)
        {

            User u = user;
            await this.userRepository.InsertAsync(u);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task UpdateAsync(TUser user)
        {

            await this.userRepository.UpdateAsync(user, user.Id);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task DeleteAsync(TUser user)
        {

            await this.userRepository.DeleteAsync(user);
        }

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///   <name>s</name>
        ///   is null. </exception>
        /// <exception cref="FormatException">
        ///   <name>s</name>
        ///   is not in the correct format. </exception>
        /// <exception cref="OverflowException">
        ///   <name>s</name>
        ///   represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
        public async Task<TUser> FindByIdAsync(TKey userId)
        {

            var x = userId.ToString();
            var uid = int.Parse(x);
            return (TUser)await this.userRepository.GetByIdAsync(uid);
        }

        /// <summary>
        /// Finds the by name asynchronous.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public async Task<TUser> FindByNameAsync(string userName)
        {
            var xx = this.userRepository.Get(x => (x.UserName == userName || x.Email == userName)).FirstOrDefault();

            return (TUser)await this.userRepository.FindAsync(x => (x.UserName == userName || x.Email == userName));
        }

        /// <summary>
        /// Adds the login asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="login">The login.</param>
        /// <returns></returns>
        public async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {

            await Task.Run(() =>
            {
                UserLogin item = new UserLogin
                {
                    UserId = user.Id,
                    ProviderKey = login.ProviderKey,
                    LoginProvider = login.LoginProvider
                };

                user.Logins.Add(item);
            });
        }

        /// <summary>
        /// Removes the login asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="login">The login.</param>
        /// <returns></returns>
        public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {

            string provider = login.LoginProvider;
            string key = login.ProviderKey;

            await Task.Run(() =>
            {
                UserLogin tUserLogin = user.Logins.SingleOrDefault(l => l.LoginProvider == provider && l.ProviderKey == key);
                if (tUserLogin != null)
                {
                    user.Logins.Remove(tUserLogin);
                }
            });
        }

        /// <summary>
        /// Gets the logins asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            return await Task.Run(() => (from l in user.Logins
                                         select new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList<UserLoginInfo>());
        }

        /// <summary>
        /// Finds the asynchronous.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns></returns>
        public async Task<TUser> FindAsync(UserLoginInfo login)
        {

            var provider = login.LoginProvider;
            var key = login.ProviderKey;
            try
            {
                var userLogin = this.userLoginRepository.Get(l => l.LoginProvider == provider && l.ProviderKey == key).FirstOrDefault();


                //await this.logins.FirstOrDefaultAsync(l => l.LoginProvider == provider && l.ProviderKey == key);

                TUser result;
                if (userLogin != null)
                {
                    var x = userLogin.UserId.ToString();
                    var uid = int.Parse(x);
                    result = (TUser)await this.userRepository.GetByIdAsync(uid);

                }
                else
                {
                    result = default(TUser);
                }
                return result;
            }
            catch
            {
                return default(TUser);
            }
        }


        /// <summary>
        /// Sets the password hash asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="passwordHash">The password hash.</param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets the password hash asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            // TODO this is not going to work with my extension

            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// Determines whether [has password asynchronous] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<bool> HasPasswordAsync(TUser user)
        {
            return await Task.Run(() => ((!string.IsNullOrEmpty(user.PasswordHash))
                && (!string.IsNullOrEmpty(user.Password))));
        }

        /// <summary>
        /// Sets the phone number asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns></returns>
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {

            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets the phone number asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(TUser user)
        {

            return Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// Gets the phone number confirmed asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// Sets the phone number confirmed asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="confirmed">if set to <c>true</c> [confirmed].</param>
        /// <returns></returns>
        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {

            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Adds to role asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public async Task AddToRoleAsync(TUser user, string roleName)
        {

            await Task.FromResult("");

            //await Task.Run(() =>
            //{
            //    var role = this.groupStore.DbEntitySet.SingleOrDefault(r => r.Name.ToUpper() == roleName.ToUpper());
            //    if (role == null)
            //    {
            //        // TODO log this
            //        return;
            //    }
            //    var tUserRole = new UserRole
            //    {
            //        UserId = user.Id,
            //        RoleId = role.Id
            //    };

            //    UserRole entity = tUserRole;
            //    this.userGroups.Add((TUserGroup)entity);
            //});
        }

        /// <summary>
        /// Removes from role asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public async Task RemoveFromRoleAsync(TUser user, string roleName)
        {

            await Task.FromResult("");

            //await Task.Run(() =>
            //{
            //    var group = this.groupStore.DbEntitySet.SingleOrDefault(r => r.Name.ToUpper() == roleName.ToUpper());
            //    if (group != null)
            //    {
            //        int groupId = group.Id;
            //        int userId = user.Id;

            //        var userGroup = this.userGroups.FirstOrDefault(r => groupId.Equals(r.RoleId) && r.UserId.Equals(userId));
            //        if (userGroup != null)
            //        {
            //            this.userGroups.Remove(userGroup);
            //        }
            //    }
            //});
        }

        /// <summary>
        /// Gets the roles asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<IList<string>> GetRolesAsync(TUser user)
        {

            return await Task.FromResult(new List<string>());

            //Task.Run(
            //    () =>
            //        (from userRoles in user.Roles join roles in this.groupStore.DbEntitySet on userRoles.RoleId equals roles.Id
            //            select roles.Name).ToList());
        }

        /// <summary>
        /// Determines whether [is in role asynchronous] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {

            return await Task.FromResult(false);

            //return await Task.Run(() =>
            //{
            //    var role = this.groupStore.DbEntitySet.SingleOrDefault(r => r.Name.ToUpper() == roleName.ToUpper());

            //    if (role != null)
            //    {
            //        bool result = role.Users.Any(delegate(UserRole ur)
            //        {
            //            int roleId = ur.RoleId;
            //            if (roleId.Equals(role.Id))
            //            {
            //                int userId = ur.UserId;

            //                return userId.Equals(user.Id);
            //            }
            //            return false;
            //        });
            //        return result;
            //    }
            //    return false;
            //});
        }

        /// <summary>
        /// Sets the security stamp asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="stamp">The stamp.</param>
        /// <returns></returns>
        public async Task SetSecurityStampAsync(TUser user, string stamp)
        {

            user.SecurityStamp = stamp;
            await Task.FromResult(0);
        }

        /// <summary>
        /// Gets the security stamp asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<string> GetSecurityStampAsync(TUser user)
        {

            return await Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        /// Context for the store
        /// </summary>
        public DataContext Context
        {
            get;
            private set;
        }

        /// <summary>
        /// If true will call dispose on the DbContext during Dispose
        /// </summary>
        public bool DisposeContext
        {
            get;
            set;
        }

        /// <summary>
        /// If true will call SaveChanges after Create/Update/Delete
        /// </summary>
        public bool AutoSaveChanges
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the two factor enabled asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns></returns>
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {

            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets the two factor enabled asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {

            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}