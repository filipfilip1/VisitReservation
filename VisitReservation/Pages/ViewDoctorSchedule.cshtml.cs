using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;
using VisitReservation.Services;
using static VisitReservation.Services.BookingService;

namespace VisitReservation.Pages
{
    public class ViewDoctorScheduleModel : PageModel
    {
        private readonly IDoctorScheduleService _doctorScheduleService;
        private readonly IBookingService _bookingService;

        [BindProperty(SupportsGet = true)]
        public string DoctorId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SelectedDate { get; set; }

        public string DoctorName { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<DateTime> AvailableDays { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<DoctorAppointmentSlot> AvailableTimeSlots { get; set; }

        public ViewDoctorScheduleModel(IDoctorScheduleService doctorScheduleService, IBookingService bookingService)
        {
            _doctorScheduleService = doctorScheduleService;
            _bookingService = bookingService;
        }

        public async Task OnGetAsync(string doctorId, DateTime? selectedDate = null)
        {
            DoctorId = doctorId;
            DoctorName = await _doctorScheduleService.GetDoctorNameAsync(DoctorId);
            AvailableDays = await _doctorScheduleService.GetAvailableDaysForDoctorAsync(DoctorId);

            if (selectedDate.HasValue)
            {
                SelectedDate = selectedDate;
                AvailableTimeSlots = await _bookingService.GetAvailableTimeSlotsForDayAsync(selectedDate.Value, DoctorId);
            }
        }

        public async Task<IActionResult> OnPostAsync(string doctorId, DateTime selectedDate, TimeSpan selectedTime)
        {
            // Przekieruje do strony rezerwacji wizyty, przekazuj¹c zebrane dane
            return RedirectToPage("/BookAppointment", new { doctorId = doctorId, date = selectedDate, time = selectedTime });
        }
    }
}
