using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using KosarkaskaLiga2019.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace KosarkaskaLiga2019.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage ="Uneiste Email")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Uneiste ime")]
            [StringLength(30,ErrorMessage ="Maksimalno 30 karaktera.")]
            public string Ime { get; set; }

            [Required(ErrorMessage = "Uneiste prezime")]
            [StringLength(30, ErrorMessage = "Maksimalno 30 karaktera.")]
            public string Prezime { get; set; }

            [Required(ErrorMessage ="Unesite lozinku")]
            [StringLength(100, ErrorMessage = "Najmanje 6 karaktera", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Lozinka")]
            public string Lozinka { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potvrdi lozinku")]
            [Compare("Lozinka", ErrorMessage = "Lozinke se ne podudaraju")]
            public string Potvrda { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email,Ime=Input.Ime,Prezime=Input.Prezime };
                var result = await _userManager.CreateAsync(user, Input.Lozinka);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Kreiran korisnik");

                   
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
