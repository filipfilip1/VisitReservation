using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using VisitReservation.Models;
using VisitReservation.Services;

namespace VisitReservation.Pages
{
    public class BookAppointmentModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly UserManager<Account> _userManager;

        // Ustawienie BindProperty dla ka¿dego z parametrów
        [BindProperty(SupportsGet = true)]
        public string DoctorId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Time { get; set; }

        public Appointment Appointment { get; set; } = new Appointment();

        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public BookAppointmentModel(IAppointmentService appointmentService, UserManager<Account> userManager)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
        }



        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ErrorMessage = "Nale¿y siê zalogowaæ.";
                return RedirectToPage("/Index");
            }

            Console.WriteLine($"OnPost BookAppointment: doctorId={DoctorId}, selectedDate={SelectedDate}, time={Time}");

            if (DateTime.TryParse(SelectedDate, out DateTime parsedDate) && TimeSpan.TryParse(Time, out TimeSpan parsedTime) && !string.IsNullOrEmpty(DoctorId))
            {
                DateTime appointmentDateTime = parsedDate.Add(parsedTime);

                try
                {
                    // Bezpoœrednie wywo³anie nowej metody CreateAppointmentAsync z aktualnymi parametrami
                    await _appointmentService.CreateAppointmentAsync(DoctorId, user.Id, appointmentDateTime);
                    SuccessMessage = "Wizyta zosta³a pomyœlnie zarezerwowana.";
                    return RedirectToPage("/PatientDashboard/Home");
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Wyst¹pi³ b³¹d podczas rezerwacji: {ex.Message}";
                    return Page();
                }
            }
            else
            {
                ErrorMessage = "Nieprawid³owe dane. Proszê spróbowaæ ponownie.";
                return Page();
            }
        }


    }
}
