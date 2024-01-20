namespace VisitReservation.Models.LinkTables
{
    public class DoctorTreatedDisease
    {
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int TreatedDiseaseId { get; set; }
        public TreatedDisease TreatedDisease { get; set; }
    }
}
