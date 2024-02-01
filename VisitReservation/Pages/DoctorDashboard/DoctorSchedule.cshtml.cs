using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;
using VisitReservation.Services;

namespace VisitReservation.Pages.DoctorDashboard
{
    [Authorize(Roles = "Doctor")]
    public class DoctorScheduleModel : PageModel
    {
        private readonly IDoctorScheduleService _doctorScheduleService;
        private readonly UserManager<Account> _userManager;

        public DoctorScheduleModel(IDoctorScheduleService doctorScheduleService, UserManager<Account> userManager)
        {
            _doctorScheduleService = doctorScheduleService;
            _userManager = userManager;
        }

        [BindProperty]
        public List<DayOfWeek> AvailableDays { get; set; } // Dni tygodnia, w których lekarz jest dostêpny

        [BindProperty]
        public TimeSpan StartTime { get; set; } // Godzina rozpoczêcia przyjêæ

        [BindProperty]
        public TimeSpan EndTime { get; set; } // Godzina zakoñczenia przyjêæ

        [BindProperty]
        public int WeeksForward { get; set; } // Na ile tygodni do przodu ma obowi¹zywaæ harmonogram

        public void OnGet()
        {
            // Inicjalizacja domyœlnych wartoœci lub wczytanie istniej¹cego harmonogramu
            // Mo¿na tutaj wczytaæ aktualny harmonogram lekarza (jeœli istnieje)
            // i ustawiæ w³aœciwoœci AvailableDays, StartTime, EndTime, WeeksForward
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"StartTime: {StartTime}");
            Console.WriteLine($"EndTime: {EndTime}");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is Doctor doctor)
            {
                await _doctorScheduleService.SetDoctorWeeklyScheduleAsync(doctor.Id, AvailableDays, StartTime, EndTime, WeeksForward);
                return RedirectToPage("/DoctorDashboard/Home"); // Przekierowanie do strony g³ównej lekarza
            }

            return Unauthorized(); // W przypadku, gdy u¿ytkownik nie jest lekarzem
        }
    }
}

