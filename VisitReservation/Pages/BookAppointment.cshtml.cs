using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;
using VisitReservation.Services;

namespace VisitReservation.Pages
{
    public class BookAppointmentModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly UserManager<Account> _userManager;

        public BookAppointmentModel(IAppointmentService appointmentService, UserManager<Account> userManager)
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

        public async Task<IActionResult> OnGetAsync(string doctorId, DateTime date, TimeSpan time)
        {
            // Sprawdzenie czy dane zosta³y przekazane
            if (!string.IsNullOrEmpty(doctorId) && date != DateTime.MinValue && time != null)
            {
                Appointment = new Appointment
                {
                    DoctorId = doctorId,
                    AppointmentDateTime = date.Add(time)
                    // Ustaw pozosta³e wymagane pola, jeœli to konieczne
                };

                // Mo¿esz tak¿e za³adowaæ dodatkowe informacje, jeœli to potrzebne
                // Na przyk³ad nazwê lekarza, szczegó³y o u¿ytkowniku itp.
            }
            else
            {
                // Ustawienie odpowiedniej wiadomoœci b³êdu lub przekierowanie
                // w przypadku braku niektórych danych
                return RedirectToPage();
            }

            return Page();
        }

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
