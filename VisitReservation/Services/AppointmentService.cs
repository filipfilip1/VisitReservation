using VisitReservation.Models;

namespace VisitReservation.Services
{
    public class AppointmentService
    {
        // Metody dla zarządzania statusami wizyt
        public AppointmentStatus ConfirmAppointment(int appointmentId) { ... }
        public AppointmentStatus CancelAppointment(int appointmentId) { ... }
        public AppointmentStatus RescheduleAppointment(int appointmentId, DateTime newDate) { ... }

        // Metody pomocnicze
        private bool IsUserAuthorizedToModify(int userId, int appointmentId) { ... }
        private bool IsAppointmentPending(int appointmentId) { ... }
    }

}
