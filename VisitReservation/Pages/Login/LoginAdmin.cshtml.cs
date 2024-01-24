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
        private readonly SignInManager<Account> _signInManager;

        public LoginAdminModel(SignInManager<Account> signInManager)
        {
            _signInManager = signInManager;
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
                    var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                    if (user != null && await _signInManager.UserManager.IsInRoleAsync(user, "Admin"))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        // Jeœli u¿ytkownik nie jest adminem, wyloguj go.
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError(string.Empty, "Tylko administratorzy maj¹ dostêp do tej strony.");
                        return Page();
                    }
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