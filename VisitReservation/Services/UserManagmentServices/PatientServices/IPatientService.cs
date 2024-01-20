using Microsoft.AspNetCore.Identity;
using VisitReservation.Models;

namespace VisitReservation.Services.UserManagmentServices.PatientServices
{
    public interface IPatientService
    {
        Task<Patient> GetPatientByIdAsync(string patientId);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<IdentityResult> CreatePatientAsync(Patient patient, string password);
        Task<IdentityResult> UpdatePatientAsync(Patient patient);
        Task<IdentityResult> DeletePatientAsync(string patientId);
    }

}
