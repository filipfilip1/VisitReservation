using System.Collections;
using VisitReservation.Models.LinkTables;

namespace VisitReservation.Models
{
    public class MedicalService
    {
        public int MedicalServiceId { get; set; }
        public string Name { get; set; }

        public ICollection<DoctorMedicalService> DoctorMedicalServices { get; set; }

    }
}
