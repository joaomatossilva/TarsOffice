using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TarsOffice.Data;

namespace TarsOffice.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        //private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly IConfiguration configuration;

        public ExternalLoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<ExternalLoginModel> logger,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            this.configuration = configuration;
            //_emailSender = emailSender;
        }

        //[BindProperty]
        //public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        //public class InputModel
        //{
        //    [Required]
        //    [EmailAddress]
        //    public string Email { get; set; }
        //}

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            var emailSuffix = configuration.GetValue<string>("Authentication:AllowDomain");
            if(!string.IsNullOrEmpty(emailSuffix))
            {
                if(!email.EndsWith(emailSuffix, StringComparison.InvariantCultureIgnoreCase))
                {
                    ErrorMessage = "This email address is not allowed.";
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }
            }

            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                user = new User { UserName = email, Email = email };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                    ErrorMessage = "Error loading external login information.";
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }
            }

            //update userInformation
            if (info.Principal.HasClaim(c => c.Type == "urn:google:name"))
            {
                user.DisplayName = info.Principal.FindFirstValue("urn:google:name");
                await _userManager.AddClaimAsync(user, info.Principal.FindFirst("urn:google:name"));
            }

            if (info.Principal.HasClaim(c => c.Type == "urn:google:picture"))
            {
                user.Picture = info.Principal.FindFirstValue("urn:google:picture");
                await _userManager.AddClaimAsync(user, info.Principal.FindFirst("urn:google:picture"));
            }

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);



            //How to check if there is a login, without check if already exists by error code?
            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded && !addLoginResult.Errors.Any(x => x.Code == "LoginAlreadyAssociated"))
            {
                foreach (var error in addLoginResult.Errors)
                {
                    _logger.LogError(error.Description);
                }
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            return await SignInUser(info, returnUrl);
        }

        private async Task<IActionResult> SignInUser(ExternalLoginInfo info, string returnUrl)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (signInResult.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }

            ErrorMessage = "Error loading external login information.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        //*** Original Code ***
        //public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? Url.Content("~/");
        //    // Get the information about the user from the external login provider
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        ErrorMessage = "Error loading external login information during confirmation.";
        //        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };

        //        var result = await _userManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await _userManager.AddLoginAsync(user, info);
        //            if (result.Succeeded)
        //            {
        //                _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

        //                var userId = await _userManager.GetUserIdAsync(user);
        //                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //                var callbackUrl = Url.Page(
        //                    "/Account/ConfirmEmail",
        //                    pageHandler: null,
        //                    values: new { area = "Identity", userId = userId, code = code },
        //                    protocol: Request.Scheme);

        //                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //                // If account confirmation is required, we need to show the link if we don't have a real email sender
        //                if (_userManager.Options.SignIn.RequireConfirmedAccount)
        //                {
        //                    return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
        //                }

        //                await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

        //                return LocalRedirect(returnUrl);
        //            }
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    ProviderDisplayName = info.ProviderDisplayName;
        //    ReturnUrl = returnUrl;
        //    return Page();
        //}
    }
}
