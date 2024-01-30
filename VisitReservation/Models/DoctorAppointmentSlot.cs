namespace VisitReservation.Models
{
    public class DoctorAppointmentSlot
    {
        public int DoctorAppointmentSlotId { get; set; }

        // Czas rozpoczęcia slotu wizyty
        public DateTime StartTime { get; set; }


        // Identyfikator lekarza
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        // Status wizyty (np. dostępna, zarezerwowana)
        public AppointmentSlotStatus Status { get; set; }

        // Opcjonalnie: Identyfikator pacjenta, który zarezerwował wizytę
        public string? PatientId { get; set; }
        public Patient? Patient { get; set; }
    }

    public enum AppointmentSlotStatus
    {
        Available,
        Booked
    }
}
