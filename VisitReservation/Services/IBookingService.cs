using VisitReservation.Data;
using VisitReservation.Models;

namespace VisitReservation.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<DoctorAppointmentSlot>> CheckAvailabilityAsync(DateTime startDate, DateTime endDate, string doctorId);

        Task<Appointment> BookAppointmentAsync(Appointment appointment);

        Task<bool> CancelAppointmentAsync(int appointmentId);

        Task<Appointment> RescheduleAppointmentAsync(int appointmentId, DateTime newDateTime);

        Task<IList<DoctorAppointmentSlot>> GetAvailableTimeSlotsForDayAsync(DateTime date, string doctorId);
        Task<IList<DateTime>> GetDistinctAvailableDaysForDoctorAsync(string doctorId);
    }
}

