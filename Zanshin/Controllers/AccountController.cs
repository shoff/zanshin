namespace Zanshin.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Http.Cors;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using NLog;

    using Domain.Entities.Identity;
    using Domain.Models.Account;
    using Domain.Providers.Identity;
    using Zanshin.Domain;
    using Zanshin.Domain.Providers.Interfaces;

    /// <summary>
    /// The account controller.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private static readonly Logger logger = LogManager.GetLogger("AccountController");
        private ApplicationSignInManager signInManager;
        private ApplicationUserManager userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">
        /// The user manager.
        /// </param>
        /// <param name="signInManager">
        /// The sign in manager.
        /// </param>
        public AccountController(IApplicationUserManager userManager,
            IApplicationSignInManager signInManager)
        {
            if (logger != null)
            {
                logger.Info("AccountController created.");
            }
            this.UserManager = (ApplicationUserManager)userManager;
            this.SignInManager = (ApplicationSignInManager)signInManager;
        }

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager
                       ?? (this.userManager = this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
            private set
            {
                this.userManager = value;
            }
        }

        /// <summary>
        /// Gets the sign in manager.
        /// </summary>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return this.signInManager ?? this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

            private set
            {
                this.signInManager = value;
            }
        }

        // GET: /Account/Login
        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return URL.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            return this.View(new LoginViewModel());
        }

        // POST: /Account/Login
        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // This doesn't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await this.SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return this.RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return this.View("Lockout");

                case SignInStatus.RequiresVerification:
                    return this.RedirectToAction("SendCode", new
                    {
                        ReturnUrl = returnUrl
                    });

                // ReSharper disable once RedundantCaseLabel
                case SignInStatus.Failure:

                default:
                    this.ModelState.AddModelError(string.Empty, Common.InvalidLoginAttempt);
                    return this.View(model);
            }
        }

        // GET: /Account/VerifyCode
        /// <summary>
        /// The verify code.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await this.SignInManager.HasBeenVerifiedAsync())
            {
                return this.View("Error");
            }

            var user = await this.UserManager.FindByIdAsync(await this.SignInManager.GetVerifiedUserIdAsync());

            if (user != null)
            {
                this.ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " +
                    await this.UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }

            return this.View(new VerifyCodeViewModel
            {
                Provider = provider,
                ReturnUrl = returnUrl
            });
        }

        // POST: /Account/VerifyCode
        /// <summary>
        /// The verify code.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, false, model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return this.RedirectToLocal(model.ReturnUrl);

                case SignInStatus.LockedOut:
                    return this.View("Lockout");

                // ReSharper disable once RedundantCaseLabel
                case SignInStatus.Failure:

                default:
                    this.ModelState.AddModelError(string.Empty, Common.InvalidCode);
                    return this.View(model);
            }
        }

        // GET: /Account/Register
        /// <summary>
        /// The register.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            return this.View();
        }

        // POST: /Account/Register
        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">Always.</exception>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await this.UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var url = this.Request.Url;
                    if (url != null)
                    {
                        var callbackUrl = this.Url.Action("ConfirmEmail", "Account", new
                        {
                            userId = user.Id,
                            code
                        }, url.Scheme);

                        await this.UserManager.SendEmailAsync(user.Id, "Confirm your account",
                                "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                        this.ViewBag.Link = callbackUrl;
                    }
                    return this.View("DisplayEmail");
                }

                this.AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        // GET: /Account/ConfirmEmail
        /// <summary>
        /// The confirm email.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId > 0 || code == null)
            {
                var result = await this.UserManager.ConfirmEmailAsync(userId, code);
                return this.View(result.Succeeded ? "ConfirmEmail" : "Error");
            }

            return this.View("Error");
        }

        // GET: /Account/ForgotPassword
        /// <summary>
        /// The forgot password.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return this.View();
        }

        // POST: /Account/ForgotPassword
        /// <summary>
        /// The forgot password.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">Always.</exception>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await this.UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return this.View("ForgotPasswordConfirmation");
                }

                var code = await this.UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var url = this.Request.Url;
                if (url != null)
                {
                    var callbackUrl = this.Url.Action("ResetPassword", "Account",
                        new
                        {
                            userId = user.Id,
                            code
                        }, url.Scheme);
                    await
                        this.UserManager.SendEmailAsync(user.Id, "Reset Password",
                            "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                    this.ViewBag.Link = callbackUrl;
                }
                return this.View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        /// <summary>
        /// The forgot password confirmation.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return this.View();
        }

        // GET: /Account/ResetPassword
        /// <summary>
        /// The reset password.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? this.View("Error") : this.View();
        }

        // POST: /Account/ResetPassword
        /// <summary>
        /// The reset password.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return this.RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await this.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return this.RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            this.AddErrors(result);
            return this.View();
        }

        // GET: /Account/ResetPasswordConfirmation
        /// <summary>
        /// The reset password confirmation.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return this.View();
        }

        // POST: /Account/ExternalLogin
        /// <summary>
        /// The external login.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, this.Url.Action("ExternalLoginCallback", "Account", new
            {
                ReturnUrl = returnUrl
            }));
        }

        // GET: /Account/SendCode
        /// <summary>
        /// The send code.
        /// </summary>
        /// <param name="returnUrl">
        /// The return URL.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await this.SignInManager.GetVerifiedUserIdAsync();
            if (userId > 0)
            {
                var userFactors = await this.UserManager.GetValidTwoFactorProvidersAsync(userId);
                var factorOptions = userFactors.Select(purpose => new SelectListItem
                {
                    Text = purpose,
                    Value = purpose
                }).ToList();
                return this.View(new SendCodeViewModel
                {
                    Providers = factorOptions,
                    ReturnUrl = returnUrl
                });
            }

            return this.View("Error");
        }

        // POST: /Account/SendCode
        /// <summary>
        /// The send code.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            // Generate the token and send it
            if (!await this.SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return this.View("Error");
            }

            return this.RedirectToAction("VerifyCode", new
            {
                Provider = model.SelectedProvider,
                model.ReturnUrl
            });
        }

        // GET: /Account/ExternalLoginCallback
        /// <summary>
        /// The external login callback.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return this.RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await this.SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return this.RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return this.View("Lockout");

                case SignInStatus.RequiresVerification:
                    return this.RedirectToAction("SendCode", new
                    {
                        ReturnUrl = returnUrl
                    });

                // ReSharper disable once RedundantCaseLabel
                case SignInStatus.Failure:

                default:

                    // If the user does not have an account, then prompt the user to create an account
                    this.ViewBag.ReturnUrl = returnUrl;
                    this.ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return this.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel
                    {
                        Email = loginInfo.Email
                    });
            }
        }

        // POST: /Account/ExternalLoginConfirmation
        /// <summary>
        /// The external login confirmation.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="returnUrl">
        /// The return URL.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Manage");
            }

            if (this.ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await this.AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return this.View("ExternalLoginFailure");
                }

                // TODO This should not be created here, move to factory
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    JoinedDate = DateTime.Now,
                    PasswordLastChangedDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    AvatarId = 1,
                    Active = true,
                    Profile = new UserProfile()
                };

                var result = await this.UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await this.UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await this.SignInManager.SignInAsync(user, false, false);
                        return this.RedirectToLocal(returnUrl);
                    }
                }

                this.AddErrors(result);
            }

            this.ViewBag.ReturnUrl = returnUrl;
            return this.View(model);
        }

        // POST: /Account/LogOff
        /// <summary>
        /// The log off.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return this.RedirectToAction("Index", "Home");
        }

        // GET: /Account/ExternalLoginFailure
        /// <summary>
        /// The external login failure.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return this.View();
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        /// <summary>
        /// The xsrf key.
        /// </summary>
        private const string XsrfKey = "XsrfId";

        /// <summary>
        /// Gets the authentication manager.
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return this.HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// The add errors.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error);
            }
        }

        /// <summary>
        /// The redirect to local.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// The challenge result.
        /// </summary>
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class.
            /// </summary>
            /// <param name="provider">
            /// The provider.
            /// </param>
            /// <param name="redirectUri">
            /// The redirect uri.
            /// </param>
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class.
            /// </summary>
            /// <param name="provider">
            /// The provider.
            /// </param>
            /// <param name="redirectUri">
            /// The redirect uri.
            /// </param>
            /// <param name="userId">
            /// The user id.
            /// </param>
            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                this.LoginProvider = provider;
                this.RedirectUri = redirectUri;
                this.UserId = userId;
            }

            /// <summary>
            /// Gets or sets the login provider.
            /// </summary>
            public string LoginProvider
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the redirect uri.
            /// </summary>
            public string RedirectUri
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the user id.
            /// </summary>
            public string UserId
            {
                get;
                set;
            }

            /// <summary>
            /// The execute result.
            /// </summary>
            /// <param name="context">
            /// The context.
            /// </param>
            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = this.RedirectUri
                };
                if (this.UserId != null)
                {
                    properties.Dictionary[XsrfKey] = this.UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.LoginProvider);
            }
        }

        #endregion
    }
}