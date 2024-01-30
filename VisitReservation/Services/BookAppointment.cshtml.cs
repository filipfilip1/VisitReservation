using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;

namespace VisitReservation.Services
{
    public class BookAppointmentModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly UserManager<IdentityUser> _userManager;

        public BookAppointmentModel(IAppointmentService appointmentService, UserManager<IdentityUser> userManager)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
        }

        [BindProperty]
        public Appointment Appointment { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync(string doctorId, DateTime date, TimeSpan time)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                Appointment = new Appointment
                {
                    DoctorId = doctorId,
                    PatientId = user.Id,
                    AppointmentDateTime = date.Add(time),
                    AppointmentStatus = AppointmentStatus.Pending
                };

                try
                {
                    await _appointmentService.CreateAppointmentAsync(Appointment, user.Id);
                    SuccessMessage = "Wizyta zosta³a pomyœlnie zarezerwowana.";
                    return RedirectToPage("/PatientDashboard/Home");
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Wyst¹pi³ b³¹d podczas rezerwacji: {ex.Message}";
                }
            }

            return Page();
        }
    }
}
