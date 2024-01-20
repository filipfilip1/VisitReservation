using VisitReservation.Models.LinkTables;

namespace VisitReservation.Models
{
    public class TreatedDisease
    {
        public int TreatedDiseaseId { get; set; }
        public string Name { get; set; }

        public ICollection<DoctorTreatedDisease> DoctorTreatedDiseases { get; set; }
    }
}
