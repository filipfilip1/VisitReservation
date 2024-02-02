using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;
using VisitReservation.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
        [BindProperty]
        public string Time { get; set; }

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

        public async Task OnGetAsync()
        {

            Console.WriteLine($"DoctorId: {DoctorId}, SelectedDate: {SelectedDate}, {DoctorName }");
            // DoctorName = await _doctorScheduleService.GetDoctorNameAsync(DoctorId);
            AvailableDays = await _doctorScheduleService.GetAvailableDaysForDoctorAsync(DoctorId);
            if (SelectedDate.HasValue)
            {
                AvailableTimeSlots = await _bookingService.GetAvailableTimeSlotsForDayAsync(SelectedDate.Value, DoctorId);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            // Konwersja string na TimeSpan
            TimeSpan selectedTime;
            bool isTimeValid = TimeSpan.TryParse(Time, out selectedTime);
            if (!isTimeValid)
            {
                // obs³u¿enie sytuacji gdy czas jest nieprawid³owy
                ModelState.AddModelError("", "Nieprawid³owy czas.");
                return Page();
            }

            Console.WriteLine($"DoctorId: {DoctorId}, SelectedDate: {SelectedDate}, Time: {Time}");

            // Przekierowanie do strony BookAppointment z wykorzystaniem w³aœciwoœci zwi¹zanych z modelem
            return RedirectToPage("/BookAppointment", new { DoctorId, SelectedDate = SelectedDate.Value.ToString("yyyy-MM-dd"), Time = selectedTime.ToString(@"hh\:mm") });
        }
    }

}
