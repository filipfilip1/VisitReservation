using System.Globalization;

namespace VisitReservation.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string SubmittedByUserId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Description { get; set; }

        // opcjonalne relacje
        public int? ReviewId { get; set; }
        public Review Review { get; set; }

        public string? DoctorId { get; set; }
        public Doctor Doctor { get; set; }

    }

}
