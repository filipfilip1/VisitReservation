using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using VisitReservation.Models;
using VisitReservation.Services.UserManagmentServices.AdminServices;

namespace VisitReservation.Pages.Login
{
    [AllowAnonymous]
    public class LoginAdminModel : PageModel
    {
        private readonly SignInManager<Admin> _signInManager;
        private readonly IAdminService _adminService; // Dodaj serwis

        public LoginAdminModel(SignInManager<Admin> signInManager, IAdminService adminService)
        {
            _signInManager = signInManager;
            _adminService = adminService; // Przypisz serwis
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nieprawid³owe dane logowania.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
