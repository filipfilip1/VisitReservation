using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using VisitReservation.Data;
using VisitReservation.Models;
using VisitReservation.Services;

namespace VisitReservation.Pages
{
    public class BookAppointmentModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly UserManager<Account> _userManager;

        private readonly ApplicationDbContext _context;

        // Ustawienie BindProperty dla ka¿dego z parametrów
        [BindProperty(SupportsGet = true)]
        public string DoctorId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Time { get; set; }
        [BindProperty(SupportsGet = true)]
        public string DoctorName { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public BookAppointmentModel(ApplicationDbContext context, IAppointmentService appointmentService, UserManager<Account> userManager)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
            _context = context;
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
                    var DoctorName = await _context.Doctors
                        .Where(d => d.Id == DoctorId)
                        .Select(d => d.UserName)
                        .FirstOrDefaultAsync();

                    // Bezpoœrednie wywo³anie nowej metody CreateAppointmentAsync z aktualnymi parametrami
                    await _appointmentService.CreateAppointmentAsync(DoctorId, user.Id, appointmentDateTime);
                    SuccessMessage = "Wizyta zosta³a pomyœlnie zarezerwowana.";
                    return RedirectToPage("/BookAppointment", new
                    {
                        DoctorName,
                        SelectedDate = appointmentDateTime,
                        success = true
                    });
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
