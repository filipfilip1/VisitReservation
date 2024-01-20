using VisitReservation.Models.LinkTables;

namespace VisitReservation.Models
{
    public class Education
    {
        public int EducationId { get; set; }
        public string University { get; set; }

        public ICollection<DoctorEducation> DoctorEducations { get; set; }
    }
}
