using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using VisitReservation.Services;

namespace VisitReservation.Pages.Appointments
{
    public class DeleteModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        [BindProperty]
        public int AppointmentId { get; set; }

        public DeleteModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> OnGetAsync(int appointmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointment = await _appointmentService.GetAppointmentAsync(appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            AppointmentId = appointmentId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _appointmentService.DeleteAppointmentAsync(AppointmentId, userId);
                return RedirectToPage("./Index");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
