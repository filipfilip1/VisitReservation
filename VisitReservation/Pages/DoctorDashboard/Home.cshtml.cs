using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Data;
using VisitReservation.Models;
using Microsoft.EntityFrameworkCore;


namespace VisitReservation.Pages.DoctorDashboard
{
    public class HomeModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<Visit> PastVisits { get; set; }
        public IList<Visit> UpcomingVisits { get; set; }
        public IList<Review> Reviews { get; set; }

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser is Doctor doctor)
            {
                PastVisits = await _context.Visits
                    .Where(v => v.DoctorId == doctor.Id && v.Date < DateTime.Now)
                    .OrderByDescending(v => v.Date)
                    .ToListAsync();

                UpcomingVisits = await _context.Visits
                    .Where(v => v.DoctorId == doctor.Id && v.Date >= DateTime.Now)
                    .OrderBy(v => v.Date)
                    .ToListAsync();

                Reviews = await _context.Reviews
                    .Where(r => r.DoctorId == doctor.Id)
                    .ToListAsync();
            }
            else
            {
                // Jeœli u¿ytkownik nie jest doktorem, przekieruj do strony g³ównej lub poka¿ komunikat o b³êdzie
                // Mo¿na dodaæ odpowiedni¹ logikê tutaj
            }
        }

        // Tutaj mo¿na dodaæ dodatkowe metody, np. do ustalania harmonogramu, aktualizacji kalendarza itp.
    }
}

