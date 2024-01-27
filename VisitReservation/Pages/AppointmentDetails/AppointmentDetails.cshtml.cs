using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Data;
using VisitReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace VisitReservation.Pages.AppointmentDetails
{
    public class AppointmentDetailsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public Appointment Appointment { get; set; }

        public AppointmentDetailsModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int appointmentId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            Appointment = await _context.Appointments.FindAsync(appointmentId);

            if (Appointment == null)
            {
                return NotFound();
            }

            // Walidacja czy u¿ytkownik to lekarz, pacjent wizyty lub admin
            if (currentUser.Id != Appointment.DoctorId && currentUser.Id != Appointment.PatientId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return Page();
        }
    }

}



