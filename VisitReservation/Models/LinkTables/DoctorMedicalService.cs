namespace VisitReservation.Models.LinkTables
{
    public class DoctorMedicalService
    {
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int MedicalServiceId { get; set; }
        public MedicalService MedicalService { get; set; }


        public int Price { get; set; }
    }
}
