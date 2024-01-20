namespace VisitReservation.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string Opinion { get; set; }
        public decimal Rating { get; set; }

        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public ICollection<Report> Reports { get; set; }
    }
}
