using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Models;

namespace VisitReservation.Services.UserManagmentServices.DoctorServices
{
    public class DoctorService : IDoctorService
    {
        private readonly UserManager<Doctor> _userManager;

        public DoctorService(UserManager<Doctor> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Doctor> GetDoctorByIdAsync(string doctorId)
        {
            return await _userManager.FindByIdAsync(doctorId);
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> CreateDoctorAsync(Doctor doctor, string password)
        {
            return await _userManager.CreateAsync(doctor, password);
        }

        public async Task<IdentityResult> UpdateDoctorAsync(Doctor doctor)
        {
            return await _userManager.UpdateAsync(doctor);
        }

        public async Task<IdentityResult> DeleteDoctorAsync(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            return doctor != null ? await _userManager.DeleteAsync(doctor) : IdentityResult.Failed();
        }
    }


}
