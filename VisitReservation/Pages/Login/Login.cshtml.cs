using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VisitReservation.Models;


namespace VisitReservation.Pages.Login
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Account> _signInManager;

        public LoginModel(SignInManager<Account> signInManager)
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
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Nie znaleziono u¿ytkownika.");
                        return Page();
                    }

                    var roles = await _signInManager.UserManager.GetRolesAsync(user);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToPage("/AdminDashboard/Home");
                    }
                    else if (roles.Contains("Doctor"))
                    {
                        return RedirectToPage("/DoctorDashboard/Home");
                    }
                    else if (roles.Contains("Patient"))
                    {
                        return RedirectToPage("/PatientDashboard/Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Nieprawid³owy typ konta.");
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