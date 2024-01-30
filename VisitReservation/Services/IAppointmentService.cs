using Microsoft.EntityFrameworkCore;
using VisitReservation.Models;

namespace VisitReservation.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> GetAppointmentAsync(int appointmentId);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment, string userId);
        Task UpdateAppointmentAsync(Appointment appointment, string userId);
        Task DeleteAppointmentAsync(int appointmentId, string userId);
        Task<IList<Appointment>> GetPastAppointmentsForDoctorAsync(string doctorId);
        Task<IList<Appointment>> GetUpcomingAppointmentsForDoctorAsync(string doctorId);
        Task<IList<Appointment>> GetPastAppointmentsForPatientAsync(string patientId);
        Task<IList<Appointment>> GetUpcomingAppointmentsForPatientAsync(string patientId);

        AppointmentStatus ConfirmAppointment(int appointmentId);
        AppointmentStatus CancelAppointment(int appointmentId);
        AppointmentStatus RescheduleAppointment(int appointmentId, DateTime newDate);

    }


}
