namespace VisitReservation.Models.LinkTables
{
    public class DoctorEducation
    {
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int EducationId { get; set; }
        public Education Education { get; set; }
    }
}
