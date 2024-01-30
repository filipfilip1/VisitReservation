using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;
using System.Security.Claims;
using VisitReservation.Services;


namespace VisitReservation.Pages.AppointmentDetails
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppointmentService _appointmentService;

        public Appointment Appointment { get; set; }

        public DetailsModel(UserManager<IdentityUser> userManager, IAppointmentService appointmentService)
        {
            _userManager = userManager;
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> OnGetAsync(int appointmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                Appointment = await _appointmentService.GetAppointmentAsync(appointmentId);

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser.Id != Appointment.DoctorId && currentUser.Id != Appointment.PatientId && !User.IsInRole("Admin"))
                {
                    return Forbid(); // Zwraca b³¹d dostêpu, jeœli u¿ytkownik nie jest uprawniony
                }

                return Page(); // Zwraca stronê, jeœli wszystko jest w porz¹dku
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(); // Zwraca b³¹d 401, jeœli dostêp jest nieautoryzowany
            }
            catch
            {
                return NotFound(); // Zwraca b³¹d 404, jeœli wizyta nie zostanie znaleziona 
            }
        }
    }
}





