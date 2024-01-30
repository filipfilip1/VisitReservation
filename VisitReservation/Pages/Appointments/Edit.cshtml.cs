using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using VisitReservation.Models;
using VisitReservation.Services;

namespace VisitReservation.Pages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        [BindProperty]
        public Appointment Appointment { get; set; }

        public EditModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> OnGetAsync(int appointmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Appointment = await _appointmentService.GetAppointmentAsync(appointmentId);

            if (Appointment == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _appointmentService.UpdateAppointmentAsync(Appointment, userId);
                return RedirectToPage("./Index");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
