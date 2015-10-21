namespace Zanshin.Domain.Providers.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Owin.Security;

    /// <summary>
    /// TODO why did I suddenly have to override this to get it to work?
    /// </summary>
    public class ApplicationAuthenticationManager : IAuthenticationManager
    {
        private readonly IAuthenticationManager authenticationManager;

        /// <summary>Initializes a new instance of the <see cref="ApplicationAuthenticationManager"/> class.</summary>
        public ApplicationAuthenticationManager()
        {
            this.authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        }

        /// <summary>
        /// Lists all of the description data provided by authentication middleware that have been chained
        /// </summary>
        /// <returns>
        /// The authentication descriptions
        /// </returns>
        public IEnumerable<AuthenticationDescription> GetAuthenticationTypes()
        {
            return this.authenticationManager.GetAuthenticationTypes();
        }

        /// <summary>
        /// Lists the description data of all of the authentication middleware which are true for a given predicate
        /// </summary>
        /// <param name="predicate">A function provided by the caller which returns true for descriptions that should be in the returned list</param>
        /// <returns>
        /// The authentication descriptions
        /// </returns>
        public IEnumerable<AuthenticationDescription> GetAuthenticationTypes(Func<AuthenticationDescription, bool> predicate)
        {
            return this.authenticationManager.GetAuthenticationTypes(predicate);
        }

        /// <summary>
        /// Call back through the middleware to ask for a specific form of authentication to be performed
        ///             on the current request
        /// </summary>
        /// <param name="authenticationType">Identifies which middleware should respond to the request
        ///             for authentication. This value is compared to the middleware's Options.AuthenticationType property.</param>
        /// <returns>
        /// Returns an object with the results of the authentication. The AuthenticationResult.Identity
        ///             may be null if authentication failed. Even if the Identity property is null, there may still be 
        ///             AuthenticationResult.properties and AuthenticationResult.Description information returned.
        /// </returns>
        public Task<AuthenticateResult> AuthenticateAsync(string authenticationType)
        {
            return this.authenticationManager.AuthenticateAsync(authenticationType);
        }

        /// <summary>
        /// Called to perform any number of authentication mechanisms on the current request.
        /// </summary>
        /// <param name="authenticationTypes">Identifies one or more middleware which should attempt to respond</param>
        /// <returns>
        /// Returns the AuthenticationResult information from the middleware which responded. The 
        ///             order is determined by the order the middleware are in the pipeline. Latest added is first in the list.
        /// </returns>
        public Task<IEnumerable<AuthenticateResult>> AuthenticateAsync(string[] authenticationTypes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add information into the response environment that will cause the authentication middleware to challenge
        ///             the caller to authenticate. This also changes the status code of the response to 401. The nature of that 
        ///             challenge varies greatly, and ranges from adding a response header or changing the 401 status code to 
        ///             a 302 redirect.
        /// </summary>
        /// <param name="properties">Additional arbitrary values which may be used by particular authentication types.</param><param name="authenticationTypes">Identify which middleware should perform their alterations on the
        ///             response. If the authenticationTypes is null or empty, that means the 
        ///             AuthenticationMode.Active middleware should perform their alterations on the response.</param>
        public void Challenge(AuthenticationProperties properties, params string[] authenticationTypes)
        {
            this.authenticationManager.Challenge(properties, authenticationTypes);
        }

        /// <summary>
        /// Add information into the response environment that will cause the authentication middleware to challenge
        ///             the caller to authenticate. This also changes the status code of the response to 401. The nature of that 
        ///             challenge varies greatly, and ranges from adding a response header or changing the 401 status code to 
        ///             a 302 redirect.
        /// </summary>
        /// <param name="authenticationTypes">Identify which middleware should perform their alterations on the
        ///             response. If the authenticationTypes is null or empty, that means the 
        ///             AuthenticationMode.Active middleware should perform their alterations on the response.</param>
        public void Challenge(params string[] authenticationTypes)
        {
            this.authenticationManager.Challenge(authenticationTypes);
        }

        /// <summary>
        /// Add information to the response environment that will cause the appropriate authentication middleware
        ///             to grant a claims-based identity to the recipient of the response. The exact mechanism of this may vary.
        ///             Examples include setting a cookie, to adding a fragment on the redirect url, or producing an OAuth2
        ///             access code or token response.
        /// </summary>
        /// <param name="properties">Contains additional properties the middleware are expected to persist along with
        ///             the claims. These values will be returned as the AuthenticateResult.properties collection when AuthenticateAsync
        ///             is called on subsequent requests.</param><param name="identities">Determines which claims are granted to the signed in user. The 
        ///             ClaimsIdentity.AuthenticationType property is compared to the middleware's Options.AuthenticationType 
        ///             value to determine which claims are granted by which middleware. The recommended use is to have a single
        ///             ClaimsIdentity which has the AuthenticationType matching a specific middleware.</param>
        public void SignIn(AuthenticationProperties properties, params ClaimsIdentity[] identities)
        {
            this.authenticationManager.SignIn(properties, identities);
        }

        /// <summary>
        /// Add information to the response environment that will cause the appropriate authentication middleware
        ///             to grant a claims-based identity to the recipient of the response. The exact mechanism of this may vary.
        ///             Examples include setting a cookie, to adding a fragment on the redirect url, or producing an OAuth2
        ///             access code or token response.
        /// </summary>
        /// <param name="identities">Determines which claims are granted to the signed in user. The 
        ///             ClaimsIdentity.AuthenticationType property is compared to the middleware's Options.AuthenticationType 
        ///             value to determine which claims are granted by which middleware. The recommended use is to have a single
        ///             ClaimsIdentity which has the AuthenticationType matching a specific middleware.</param>
        public void SignIn(params ClaimsIdentity[] identities)
        {
            this.authenticationManager.SignIn(identities);
        }

        /// <summary>
        /// Add information to the response environment that will cause the appropriate authentication middleware
        ///             to revoke any claims identity associated the the caller. The exact method varies.
        /// </summary>
        /// <param name="properties">Additional arbitrary values which may be used by particular authentication types.</param><param name="authenticationTypes">Identifies which middleware should perform the work to sign out.
        ///             Multiple authentication types may be provided to clear out more than one cookie at a time, or to clear
        ///             cookies and redirect to an external single-sign out url.</param>
        public void SignOut(AuthenticationProperties properties, params string[] authenticationTypes)
        {
            this.authenticationManager.SignOut(properties, authenticationTypes);
        }

        /// <summary>
        /// Add information to the response environment that will cause the appropriate authentication middleware
        ///             to revoke any claims identity associated the the caller. The exact method varies.
        /// </summary>
        /// <param name="authenticationTypes">Identifies which middleware should perform the work to sign out.
        ///             Multiple authentication types may be provided to clear out more than one cookie at a time, or to clear
        ///             cookies and redirect to an external single-sign out url.</param>
        public void SignOut(params string[] authenticationTypes)
        {
            this.authenticationManager.SignOut(authenticationTypes);
        }

        /// <summary>
        /// Returns the current user for the request
        /// </summary>
        public ClaimsPrincipal User
        {
            get
            {
                return this.authenticationManager.User;
            }
            set
            {
                this.authenticationManager.User = value;
            }
        }

        /// <summary>
        /// Exposes the security.Challenge environment value as a strong type.
        /// </summary>
        public AuthenticationResponseChallenge AuthenticationResponseChallenge
        {
            get
            {
                return this.authenticationManager.AuthenticationResponseChallenge;
            }
            set
            {
                this.authenticationManager.AuthenticationResponseChallenge = value;
            }
        }

        /// <summary>
        /// Exposes the security.SignIn environment value as a strong type.
        /// </summary>
        public AuthenticationResponseGrant AuthenticationResponseGrant
        {
            get
            {
                return this.authenticationManager.AuthenticationResponseGrant;
            }
            set
            {
                this.authenticationManager.AuthenticationResponseGrant = value;
            }
        }

        /// <summary>
        /// Exposes the security.SignOut environment value as a strong type.
        /// </summary>
        public AuthenticationResponseRevoke AuthenticationResponseRevoke
        {
            get
            {
                return this.authenticationManager.AuthenticationResponseRevoke;
            }
            set
            {
                this.authenticationManager.AuthenticationResponseRevoke = value;
            }
        }
    }
}