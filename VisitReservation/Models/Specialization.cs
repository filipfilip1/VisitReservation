using VisitReservation.Models.LinkTables;

namespace VisitReservation.Models
{
    public class Specialization
    {
        public int SpecializationId { get; set; }
        public string Name { get; set; }

        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; }
    }
}
