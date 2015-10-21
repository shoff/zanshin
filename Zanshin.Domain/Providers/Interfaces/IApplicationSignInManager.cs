namespace Zanshin.Domain.Providers.Interfaces
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// </summary>
    public interface IApplicationSignInManager
    {
        /// <summary>
        /// Creates the user identity asynchronous.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The value of 'user' cannot be null. 
        /// </exception>
        Task<ClaimsIdentity> CreateUserIdentityAsync(User user);

        /// <summary>
        /// Converts the identifier to string.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        string ConvertIdToString(int id);

        /// <summary>
        /// Converts the identifier from string.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        int ConvertIdFromString(string id);

        /// <summary>
        /// Signs the in asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        /// <param name="rememberBrowser">if set to <c>true</c> [remember browser].</param>
        /// <returns></returns>
        Task SignInAsync(User user, bool isPersistent, bool rememberBrowser);

        /// <summary>
        /// Sends the two factor code asynchronous.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        Task<bool> SendTwoFactorCodeAsync(string provider);

        /// <summary>
        /// Gets the verified user identifier asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<int> GetVerifiedUserIdAsync();

        /// <summary>
        /// Determines whether [has been verified asynchronous].
        /// </summary>
        /// <returns></returns>
        Task<bool> HasBeenVerifiedAsync();

        /// <summary>
        /// Twoes the factor sign in asynchronous.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="code">The code.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        /// <param name="rememberBrowser">if set to <c>true</c> [remember browser].</param>
        /// <returns></returns>
        Task<SignInStatus> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser);

        /// <summary>
        /// Externals the sign in asynchronous.
        /// </summary>
        /// <param name="loginInfo">The login information.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        /// <returns></returns>
        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent);

        /// <summary>
        /// Passwords the sign in asynchronous.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        /// <param name="shouldLockout">if set to <c>true</c> [should lockout].</param>
        /// <returns></returns>
        Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Gets or sets the type of the authentication.
        /// </summary>
        /// <value>
        /// The type of the authentication.
        /// </value>
        string AuthenticationType { get; set; }
       
        /// <summary>
        /// Gets or sets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        UserManager<User, int> UserManager { get; set; }
        
        /// <summary>
        /// Gets or sets the authentication manager.
        /// </summary>
        /// <value>
        /// The authentication manager.
        /// </value>
        IAuthenticationManager AuthenticationManager { get; set; }
    }
}