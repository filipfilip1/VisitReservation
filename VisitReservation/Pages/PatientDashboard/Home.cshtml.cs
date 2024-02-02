using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;
using Microsoft.AspNetCore.Authorization;
// Zak³adamy, ¿e masz zdefiniowane interfejsy serwisów IAppointmentService i IReviewService
using VisitReservation.Services;
using Microsoft.AspNetCore.Mvc;

namespace VisitReservation.Pages.PatientDashboard
{
    [Authorize(Roles = "Patient")]
    public class HomeModel : PageModel
    {
        private readonly UserManager<Account> _userManager;
        private readonly IAppointmentService _appointmentService;
        // private readonly IReviewService _reviewService;

        public HomeModel(UserManager<Account> userManager, IAppointmentService appointmentService /*, IReviewService reviewService*/ )
        {
            _userManager = userManager;
            _appointmentService = appointmentService;
            // _reviewService = reviewService;
        }

        // Zak³adka "Historia Wizyt"
        public IList<Appointment> PastAppointments { get; private set; }

        // Zak³adka "Wizyty Nadchodz¹ce"
        public IList<Appointment> UpcomingAppointments { get; private set; }

        // Zak³adka "Twoje Opinie"
        // public IList<Review> Reviews { get; private set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                PastAppointments = await _appointmentService.GetPastAppointmentsForPatientAsync(user.Id);
                UpcomingAppointments = await _appointmentService.GetUpcomingAppointmentsForPatientAsync(user.Id);
                //Reviews = await _reviewService.GetReviewsForPatientAsync(user.Id);
            }
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

    }
}
