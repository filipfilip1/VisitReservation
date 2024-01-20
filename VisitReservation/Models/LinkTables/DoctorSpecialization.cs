namespace VisitReservation.Models.LinkTables
{
    public class DoctorSpecialization
    {
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }

    }
}
