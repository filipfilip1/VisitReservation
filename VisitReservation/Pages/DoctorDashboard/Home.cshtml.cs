using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Data;
using VisitReservation.Models;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Services;


namespace VisitReservation.Pages.DoctorDashboard
{
    public class HomeModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppointmentService _appointmentService;
        private readonly ApplicationDbContext _context;

        public HomeModel(UserManager<IdentityUser> userManager, IAppointmentService appointmentService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _appointmentService = appointmentService;
            _context = context;
        }

        public IList<Appointment> PastAppointments { get; set; }
        public IList<Appointment> UpcomingAppointments { get; set; }
        public IList<Review> Reviews { get; set; }

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser is Doctor doctor)
            {
                PastAppointments = await _appointmentService.GetPastAppointmentsForDoctorAsync(doctor.Id);
                UpcomingAppointments = await _appointmentService.GetUpcomingAppointmentsForDoctorAsync(doctor.Id);

                Reviews = await _context.Reviews
                    .Where(r => r.DoctorId == doctor.Id)
                    .ToListAsync();
            }
            else
            {
                // Je�li u�ytkownik nie jest doktorem, przekieruj do strony g��wnej lub poka� komunikat o b��dzie
            }
        }

        // dodatkowe metody, np. do ustalania harmonogramu, aktualizacji kalendarza itp. ??
    }

}

