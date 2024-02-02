using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Data;
using VisitReservation.Models;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace VisitReservation.Pages.DoctorDashboard
{
    [Authorize(Roles = "Doctor")]
    public class HomeModel : PageModel
    {
        private readonly UserManager<Account> _userManager;
        private readonly IAppointmentService _appointmentService;
        private readonly ApplicationDbContext _context;

        public HomeModel(UserManager<Account> userManager, IAppointmentService appointmentService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _appointmentService = appointmentService;
            _context = context;
        }

        public IList<Appointment> PastAppointments { get; set; }
        public IList<Appointment> UpcomingAppointments { get; set; }
        public IList<Review> Reviews { get; set; }
        public IList<Appointment> PendingAppointments { get; set; }

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is Doctor doctor)
            {
                PastAppointments = await _appointmentService.GetPastAppointmentsForDoctorAsync(doctor.Id);
                UpcomingAppointments = await _appointmentService.GetUpcomingAppointmentsForDoctorAsync(doctor.Id);
                PendingAppointments = await _appointmentService.GetPendingAppointmentsForDoctorAsync(doctor.Id); 

                Reviews = await _context.Reviews
                    .Where(r => r.DoctorId == doctor.Id)
                    .ToListAsync();
            }
            else
            {

            }
        }

        public async Task<IActionResult> OnPostConfirmAppointmentAsync(int appointmentId)
        {
            try
            {
                var result = await _appointmentService.ConfirmAppointmentAsync(appointmentId);
                TempData["SuccessMessage"] = "Wizyta zosta³a potwierdzona.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie uda³o siê potwierdziæ wizyty: {ex.Message}";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCancelAppointmentAsync(int appointmentId)
        {
            try
            {
                var result = await _appointmentService.CancelAppointmentAsync(appointmentId);
                TempData["SuccessMessage"] = "Wizyta zosta³a odwo³ana.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nie uda³o siê odwo³aæ wizyty: {ex.Message}";
            }

            return RedirectToPage();
        }





        // dodatkowe metody, np. do ustalania harmonogramu, aktualizacji kalendarza itp. ??
    }

}

