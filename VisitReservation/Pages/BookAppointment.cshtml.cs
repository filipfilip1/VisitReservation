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
            // Sprawdzenie czy dane zosta�y przekazane
            if (!string.IsNullOrEmpty(doctorId) && date != DateTime.MinValue && time != null)
            {
                Appointment = new Appointment
                {
                    DoctorId = doctorId,
                    AppointmentDateTime = date.Add(time)
                    // Ustaw pozosta�e wymagane pola, je�li to konieczne
                };

                // Mo�esz tak�e za�adowa� dodatkowe informacje, je�li to potrzebne
                // Na przyk�ad nazw� lekarza, szczeg�y o u�ytkowniku itp.
            }
            else
            {
                // Ustawienie odpowiedniej wiadomo�ci b��du lub przekierowanie
                // w przypadku braku niekt�rych danych
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
                    SuccessMessage = "Wizyta zosta�a pomy�lnie zarezerwowana.";
                    return RedirectToPage("/PatientDashboard/Home");
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Wyst�pi� b��d podczas rezerwacji: {ex.Message}";
                }
            }

            return Page();
        }

    }

}
