namespace Zanshin.Domain.Providers.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Zanshin.Domain.Data;
    using Zanshin.Domain.Entities.Identity;


    // ReSharper disable once TypeParameterCanBeVariant
    /// <summary>
    ///  </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IUserStoreProvider<TUser, TKey>
        where TUser : User, IUser<TKey>
        where TKey : IEquatable<TKey>
    {

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task CreateAsync(TUser user);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task UpdateAsync(TUser user);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task DeleteAsync(TUser user);

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<TUser> FindByIdAsync(TKey userId);

        /// <summary>
        /// Finds the by name asynchronous.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Task<TUser> FindByNameAsync(string userName);

        /// <summary>
        /// Adds the login asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="login">The login.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task AddLoginAsync(TUser user, UserLoginInfo login);

        /// <summary>
        /// Removes the login asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="login">The login.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task RemoveLoginAsync(TUser user, UserLoginInfo login);

        /// <summary>
        /// Gets the logins asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);

        /// <summary>
        /// Finds the asynchronous.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">login</exception>
        Task<TUser> FindAsync(UserLoginInfo login);

        /// <summary>
        /// Gets the claims asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<IList<Claim>> GetClaimsAsync(TUser user);

        /// <summary>
        /// Adds the claim asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task AddClaimAsync(TUser user, Claim claim);

        /// <summary>
        /// Removes the claim asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task RemoveClaimAsync(TUser user, Claim claim);

        /// <summary>
        /// Adds to role asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task AddToRoleAsync(TUser user, string roleName);

        /// <summary>
        /// Removes from role asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task RemoveFromRoleAsync(TUser user, string roleName);

        /// <summary>
        /// Gets the roles asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<IList<string>> GetRolesAsync(TUser user);

        /// <summary>
        /// Determines whether [is in role asynchronous] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<bool> IsInRoleAsync(TUser user, string roleName);

        /// <summary>
        /// Sets the password hash asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="passwordHash">The password hash.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task SetPasswordHashAsync(TUser user, string passwordHash);

        /// <summary>
        /// Gets the password hash asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<string> GetPasswordHashAsync(TUser user);

        /// <summary>
        /// Determines whether [has password asynchronous] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<bool> HasPasswordAsync(TUser user);

        /// <summary>
        /// Sets the security stamp asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="stamp">The stamp.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="ArgumentNullException">The value of 'user' cannot be null. </exception>
        Task SetSecurityStampAsync(TUser user, string stamp);

        /// <summary>
        /// Gets the security stamp asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="ArgumentNullException">The value of 'user' cannot be null. </exception>
        Task<string> GetSecurityStampAsync(TUser user);

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        IQueryable<TUser> Users { get; }

        /// <summary>
        /// Context for the store
        /// </summary>
        DataContext Context { get; }


        /// <summary>
        /// If true will call SaveChanges after Create/Update/Delete
        /// </summary>
        bool AutoSaveChanges { get; set; }

        /// <summary>
        /// Sets the email asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="ArgumentNullException">The value of 'user' cannot be null. </exception>
        Task SetEmailAsync(TUser user, string email);

        /// <summary>
        /// Gets the email asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<string> GetEmailAsync(TUser user);

        /// <summary>
        /// Gets the email confirmed asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<bool> GetEmailConfirmedAsync(TUser user);

        /// <summary>
        /// Sets the email confirmed asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="confirmed">if set to <c>true</c> [confirmed].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task SetEmailConfirmedAsync(TUser user, bool confirmed);

        /// <summary>
        /// Finds the by email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<TUser> FindByEmailAsync(string email);

        /// <summary>
        /// Sets the phone number asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task SetPhoneNumberAsync(TUser user, string phoneNumber);

        /// <summary>
        /// Gets the phone number asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<string> GetPhoneNumberAsync(TUser user);

        /// <summary>
        /// Gets the phone number confirmed asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<bool> GetPhoneNumberConfirmedAsync(TUser user);

        /// <summary>
        /// Sets the phone number confirmed asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="confirmed">if set to <c>true</c> [confirmed].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed);

        /// <summary>
        /// Sets the two factor enabled asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task SetTwoFactorEnabledAsync(TUser user, bool enabled);

        /// <summary>
        /// Gets the two factor enabled asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<bool> GetTwoFactorEnabledAsync(TUser user);

        /// <summary>
        /// Gets the lockout end date asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user);

        /// <summary>
        /// Sets the lockout end date asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="lockoutEnd">The lockout end.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd);

        /// <summary>
        /// Increments the access failed count asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<int> IncrementAccessFailedCountAsync(TUser user);

        /// <summary>
        /// Resets the access failed count asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task ResetAccessFailedCountAsync(TUser user);

        /// <summary>
        /// Gets the access failed count asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<int> GetAccessFailedCountAsync(TUser user);

        /// <summary>
        /// Gets the lockout enabled asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task<bool> GetLockoutEnabledAsync(TUser user);

        /// <summary>
        /// Sets the lockout enabled asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Task SetLockoutEnabledAsync(TUser user, bool enabled);
    }
}