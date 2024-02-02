using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VisitReservation.Models;
using VisitReservation.Services.DataManagmentDoctor;

namespace VisitReservation.Pages.Register
{
    public class RegisterDoctorModel : PageModel
    {
        private readonly UserManager<Account> _userManager;
        private readonly IEducationService _educationService;
        private readonly ISpecializationService _specializationService;
        private readonly ITreatedDiseaseService _treatedDiseaseService;
        private readonly IMedicalServiceService _medicalServiceService;


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

            [Required]
            public string Description { get; set; }

            [Required]
            public string WorkAddress { get; set; }

            public List<int> EducationIds { get; set; }
            public List<int> SpecializationIds { get; set; }
            public List<int> MedicalServiceIds { get; set; }
            public List<int> TreatedDiseaseIds { get; set; }
        }

        public List<SelectListItem> Educations { get; set; }
        public List<SelectListItem> Specializations { get; set; }
        public List<SelectListItem> MedicalServices { get; set; }
        public List<SelectListItem> TreatedDiseases { get; set; }



        public RegisterDoctorModel(UserManager<Account> userManager,
                                   IEducationService educationService,
                                   ISpecializationService specializationService,
                                   IMedicalServiceService medicalServiceService,
                                   ITreatedDiseaseService treatedDiseaseService)

        {
            _userManager = userManager;
            _educationService = educationService;
            _specializationService = specializationService;
            _medicalServiceService = medicalServiceService;
            _treatedDiseaseService = treatedDiseaseService;
        }

        public void OnGet()
        {
            Educations = _educationService.GetEducationSelectList();
            Specializations = _specializationService.GetSpecializationSelectList();
            MedicalServices = _medicalServiceService.GetMedicalServiceSelectList();
            TreatedDiseases = _treatedDiseaseService.GetTreatedDiseaseSelectList();
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

                    VerificationStatus = VerificationStatus.PendingVerification, // Status weryfikacji
                                                                                
                };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // Przypisywanie powi¹zañ edukacji, specjalizacji itd.
                    _educationService.AssignEducationsToDoctor(user.Id, Input.EducationIds);
                    _specializationService.AssignSpecializationsToDoctor(user.Id, Input.SpecializationIds);
                    _medicalServiceService.AssignMedicalServicesToDoctor(user.Id, Input.MedicalServiceIds);
                    _treatedDiseaseService.AssignTreatedDiseasesToDoctor(user.Id, Input.TreatedDiseaseIds);


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
