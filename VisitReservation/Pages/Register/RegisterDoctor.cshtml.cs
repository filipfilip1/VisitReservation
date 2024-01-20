using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VisitReservation.Models;
using VisitReservation.Services.DataManagmentDoctor;

namespace VisitReservation.Pages.Register
{
    public class RegisterDoctorModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        // Zak³adaj¹c, ¿e masz serwisy do obs³ugi danych edukacji, specjalizacji itp.
        private readonly IEducationService _educationService;
        private readonly ISpecializationService _specializationService;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string WorkAddress { get; set; }

            public List<int> EducationIds { get; set; }
            public List<int> SpecializationIds { get; set; }
            // ... inne powi¹zania
        }

        // Listy dla danych rozwijanych
        public List<SelectListItem> Educations { get; set; }
        public List<SelectListItem> Specializations { get; set; }
        // ... inne listy

        public RegisterDoctorModel(UserManager<IdentityUser> userManager,
                                   IEducationService educationService,
                                   ISpecializationService specializationService)
        {
            _userManager = userManager;
            _educationService = educationService;
            _specializationService = specializationService;
        }

        public void OnGet()
        {
            // Wczytanie danych do list rozwijanych
            Educations = _educationService.GetEducationSelectList();
            Specializations = _specializationService.GetSpecializationSelectList();
            // ... wczytywanie innych list
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new Doctor
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Description = Input.Description,
                    WorkAddress = Input.WorkAddress,
                    // ... przypisywanie pozosta³ych pól
                };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // Przypisywanie powi¹zañ, np. edukacji, specjalizacji itp.
                    _educationService.AssignEducationsToDoctor(user.Id, Input.EducationIds);
                    _specializationService.AssignSpecializationsToDoctor(user.Id, Input.SpecializationIds);
                    // ... przypisywanie innych powi¹zañ

                    // Dodatkowe akcje po pomyœlnej rejestracji, np. przekierowanie
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }

}
