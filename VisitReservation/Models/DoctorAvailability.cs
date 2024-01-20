namespace VisitReservation.Models
{
    public class DoctorAvailability
    {
        public int DoctorAvailabilityId {  get; set; }

        // początek pracy
        public DateTime StartDateTime { get; set; }

        // koniec pracy
        public DateTime EndDateTime { get; set; } 

        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
