// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Gardens2024.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace Garden2024.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ICountriesService _countriesService;
        private readonly IStatesService _statesService;
        private readonly ICitiesService _citiesService;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, 
            ICountriesService countriesService,
            IStatesService statesService,
            ICitiesService citiesService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _countriesService = countriesService;
            _statesService = statesService;
            _citiesService = citiesService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "{0} is required")]
            [StringLength(100, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
            [DisplayName("First Name")]
            public string FirstName { get; set; } = null!;

            [Required(ErrorMessage = "{0} is required")]
            [StringLength(100, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
            [DisplayName("Last Name")]
            public string LastName { get; set; } = null!;

            [Required(ErrorMessage = "{0} is required")]
            [StringLength(200, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
            public string Address { get; set; } = null!;

            [MaxLength(20, ErrorMessage = "{0} must have at least {1} characters")]
            public string? Phone { get; set; }

            [Required(ErrorMessage = "{0} is required")]
            [StringLength(10, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
            [DisplayName("Zip Code")]
            public string ZipCode { get; set; } = null!;

            [Required(ErrorMessage = "Country is required")]
            [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
            [DisplayName("Country")]
            public int CountryId { get; set; }

            [Required(ErrorMessage = "State is required")]
            [Range(1, int.MaxValue, ErrorMessage = "You must select a state")]
            [DisplayName("State")]
            public int StateId { get; set; }

            [Required(ErrorMessage = "City is required")]
            [Range(1, int.MaxValue, ErrorMessage = "You must select a city")]
            [DisplayName("City")]
            public int CityId { get; set; }


            [ValidateNever]
            public IEnumerable<SelectListItem> Countries { get; set; } = null!;

            [ValidateNever]
            public IEnumerable<SelectListItem> States { get; set; } = null!;

            [ValidateNever]
            public IEnumerable<SelectListItem> Cities { get; set; } = null!;

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Input = new InputModel
            {
                Countries = _countriesService.GetAll(orderBy:
                    o => o.OrderBy(c => c.CountryName)).Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    }),
                States = _statesService.GetAll(orderBy:
                    o => o.OrderBy(c => c.StateName)).Select(c => new SelectListItem
                    {
                        Text = c.StateName,
                        Value = c.StateId.ToString()
                    }),
                Cities = _citiesService.GetAll(orderBy:
                    o => o.OrderBy(c => c.CityName)).Select(c => new SelectListItem
                    {
                        Text = c.CityName,
                        Value = c.CityId.ToString()
                    }),

            };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Address = Input.Address;
                user.ZipCode = Input.ZipCode;
                user.CountryId = Input.CountryId;
                user.StateId = Input.StateId;
                user.CityId = Input.CityId;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, WC.Role_Customer);

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }

        public JsonResult OnGetGetStates(int countryId)
        {
            var stateList = _statesService.GetAll(
                filter: s => s.CountryId == countryId).ToList();
            return new JsonResult(stateList);

        }
        public JsonResult OnGetGetCities(int stateId)
        {
            var citiesList = _citiesService.GetAll(
                filter: s => s.StateId == stateId).ToList();
            return new JsonResult(citiesList);

        }

    }
}
