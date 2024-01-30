using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;
using VisitReservation.Data;


namespace VisitReservation.Pages.PatientDashboard
{

    
        public class HomeModel : PageModel
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly ApplicationDbContext _context; // Za��my, �e to jest kontekst bazy danych

            public HomeModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
            {
                _userManager = userManager;
                _context = context;
            }

            // Zak�adka "Historia Wizyt"
            public IList<Appointment> PastAppointments { get; private set; }

            // Zak�adka "Wizyty Nadchodz�ce"
            public IList<Appointment> UpcomingAppointments { get; private set; }

            // Zak�adka "Twoje Opinie"
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
                // Implementacja metody do pobierania przesz�ych wizyt
                PastAppointments = _context.Appointments
                    .Where(a => a.PatientId == userId && a.AppointmentDateTime < DateTime.Now)
                    .ToList();
            }

            private void LoadUpcomingAppointments(string userId)
            {
                // Implementacja metody do pobierania nadchodz�cych wizyt
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

            // Tutaj mo�na doda� metody do obs�ugi umawiania wizyt
        }


}

