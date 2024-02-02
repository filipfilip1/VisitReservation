using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;
using VisitReservation.Data;
using Microsoft.AspNetCore.Authorization;


namespace VisitReservation.Pages.PatientDashboard
{
    [Authorize(Roles = "Patient")]
    public class HomeModel : PageModel
        {
            private readonly UserManager<Account> _userManager;
            private readonly ApplicationDbContext _context; 

            public HomeModel(UserManager<Account> userManager, ApplicationDbContext context)
            {
                _userManager = userManager;
                _context = context;
            }

            // Zak³adka "Historia Wizyt"
            public IList<Appointment> PastAppointments { get; private set; }

            // Zak³adka "Wizyty Nadchodz¹ce"
            public IList<Appointment> UpcomingAppointments { get; private set; }

            // Zak³adka "Twoje Opinie"
            public IList<Review> Reviews { get; private set; }

            public async Task OnGetAsync()
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    LoadPastAppointments(user.Id);
                    LoadUpcomingAppointments(user.Id);
                    LoadReviews(user.Id);
                }
            }

            private void LoadPastAppointments(string userId)
            {
                // Implementacja metody do pobierania przesz³ych wizyt
                PastAppointments = _context.Appointments
                    .Where(a => a.PatientId == userId && a.AppointmentDateTime < DateTime.Now)
                    .ToList();
            }

            private void LoadUpcomingAppointments(string userId)
            {
                // Implementacja metody do pobierania nadchodz¹cych wizyt
                UpcomingAppointments = _context.Appointments
                    .Where(a => a.PatientId == userId && a.AppointmentDateTime >= DateTime.Now)
                    .ToList();
            }

            private void LoadReviews(string userId)
            {
                // Implementacja metody do pobierania opinii
                Reviews = _context.Reviews
                    .Where(r => r.PatientId == userId)
                    .ToList();
            }

            // Tutaj mo¿na dodaæ metody do obs³ugi umawiania wizyt
        }


}

