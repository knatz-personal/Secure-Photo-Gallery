using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SecurityUtil;
using WebPortal.Extensions;
using WebPortal.Helpers;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AccountController : Controller
    {
        #region Properties

        public AppSignInManager GetSignInManager => HttpContext.GetOwinContext().Get<AppSignInManager>();

        public AppUserManager GetUserManager => HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

        #endregion Properties

        public AccountController()
        {
            _dbcontext = new ApplicationDbContext();

            _client = HttpClientHelpers.GetApiClient(Properties.Settings.Default.SSDAPI);
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("ErrorRedirect", "Error", new { code = "404" });
            }
            var result = await GetUserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user
            // already has a login
            var result = await GetSignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });

                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user
                    // to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new AppUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await GetUserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await GetUserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await GetSignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await GetUserManager.FindByNameAsync(model.Email);
                if (user == null || !(await GetUserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and
                // password reset please visit
                // http://go.microsoft.com/fwlink/?LinkID=320771 Send an email
                // with this link string code = await
                // UserManager.GeneratePasswordResetTokenAsync(user.Id); var
                // callbackUrl = Url.Action("ResetPassword", "Account", new {
                // userId = user.Id, code = code }, protocol:
                // Request.Url.Scheme); await UserManager.SendEmailAsync(user.Id,
                // "Reset Password", "Please reset your password by clicking <a
                // href=\"" + callbackUrl + "\">here</a>"); return
                // RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result =
                       await GetSignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                switch (result)
                {
                    case SignInStatus.Success:
                        var sym = EncryptionUtil.GenerateSymmetricParameters();
                        Session.Add("SymmetricParameters", sym);
                        return RedirectToAction("Index", "Home");

                    case SignInStatus.LockedOut:
                        return View("Lockout");

                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Request.GetOwinContext().Authentication.SignOut();
            if (User.Identity.IsAuthenticated)
            {
                GetSignInManager.AuthenticationManager.SignOut();
            }
            SessionExtensions.DestroySessions();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterModel()
            {
                Genders = GetGenderList(),
                Towns = GetTownList()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var asym = EncryptionUtil.GenerateAsymmetricKeys();
                AppUser user = new AppUser
                {
                    Name = model.FirstName,
                    Surname = model.LastName,
                    Mobile = model.Mobile,
                    Address = model.Address,
                    PhoneNumber = model.Telephone,
                    Email = model.Email,
                    GenderID = model.GenderId,
                    TownID = model.GenderId,
                    PrivateKey = asym.PrivateKey,
                    PublicKey = asym.PublicKey,
                    UserName = model.UserName,
                    LockoutEnabled = false
                };

                var result = await GetUserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var currentUser = GetUserManager.FindByName(user.UserName);
                    GetUserManager.AddToRole(currentUser.Id, "Customer");
                    await GetSignInManager.SignInAsync(user, false, false);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            model.Genders = GetGenderList();
            model.Towns = GetTownList();

            return View(model);
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? (ActionResult)RedirectToAction("ErrorRedirect", "Error", new { code = "404" }) : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetUserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await GetUserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await GetSignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("ErrorRedirect", "Error", new { code = "404" });
            }
            var userFactors = await GetUserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await GetSignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return RedirectToAction("ErrorRedirect", "Error", new { code = "500" });
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via Username/password
            // or external login
            if (!await GetSignInManager.HasBeenVerifiedAsync())
            {
                return RedirectToAction("ErrorRedirect", "Error", new { code = "500" });
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the
            // two factor codes. If a user enters incorrect codes for a specified
            // amount of time then the user account will be locked out for a
            // specified amount of time. You can configure the account lockout
            // settings in IdentityConfig
            var result = await GetSignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        private SelectList GetGenderList()
        {
            HttpResponseMessage responseMessage = _client.GetAsync(_baseUrl + "Gender").Result;

            if (responseMessage == null)
            {
                throw new NullReferenceException("AccountController>GetGenderList: responseMessage is null");
            }

            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            var genders = _serializer.Deserialize<List<GenderModel>>(responseData);

            return new SelectList(genders, "Id", "Name");
        }

        private SelectList GetTownList()
        {
            HttpResponseMessage responseMessage = _client.GetAsync(_baseUrl + "Town").Result;

            if (responseMessage == null)
            {
                throw new NullReferenceException("AccountController>GetGenderList: responseMessage is null");
            }

            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            var towns = _serializer.Deserialize<List<TownModel>>(responseData);

            return new SelectList(towns, "Id", "Name");
        }

        #region Destructor

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbcontext != null)
                {
                    _dbcontext.Dispose();
                    _dbcontext = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion Destructor

        #region Fields

        private readonly HttpClient _client;
        private string _baseUrl = "http://localhost:60934/api/";
        private ApplicationDbContext _dbcontext;
        private JavaScriptSerializer _serializer = new JavaScriptSerializer();

        #endregion Fields

        #region Helpers

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = GetUserManager.FindById(User.Identity.GetUserId());
            return user?.PasswordHash != null;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task SignInAsync(AppUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await GetUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        #endregion Helpers
    }
}