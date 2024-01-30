using Microsoft.AspNetCore.Identity;
using VisitReservation.Models.LinkTables;

namespace VisitReservation.Models
{
    public class Doctor : Account
    {
        public string Description {  get; set; }
        public string WorkAddress { get; set; }
        public VerificationStatus VerificationStatus { get; set; }
        
        public ICollection<DoctorEducation> DoctorEducations { get; set; }
        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set;}
        public ICollection<DoctorMedicalService> DoctorMedicalServices { get; set; }
        public ICollection<DoctorTreatedDisease> DoctorTreatedDiseases { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Appointment> Appointments { get; set; }   
        public ICollection<DoctorAppointmentSlot> Availabilities { get; set; }
        public ICollection<Report> Reports { get; set; }
    }

    public enum VerificationStatus
    {
        PendingVerification,
        Verified,
        Rejected,
        AdditionalInfoRequired
    }
}
