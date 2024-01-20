using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Models;

namespace VisitReservation.Services.UserManagmentServices.PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly UserManager<Patient> _userManager;

        public PatientService(UserManager<Patient> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Patient> GetPatientByIdAsync(string patientId)
        {
            return await _userManager.FindByIdAsync(patientId);
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> CreatePatientAsync(Patient patient, string password)
        {
            return await _userManager.CreateAsync(patient, password);
        }

        public async Task<IdentityResult> UpdatePatientAsync(Patient patient)
        {
            return await _userManager.UpdateAsync(patient);
        }

        public async Task<IdentityResult> DeletePatientAsync(string patientId)
        {
            var patient = await _userManager.FindByIdAsync(patientId);
            return patient != null ? await _userManager.DeleteAsync(patient) : IdentityResult.Failed();
        }
    }

}
