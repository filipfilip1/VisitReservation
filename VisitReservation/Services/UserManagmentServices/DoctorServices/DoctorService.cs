using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Models;

namespace VisitReservation.Services.UserManagmentServices.DoctorServices
{
    public class DoctorService : IDoctorService
    {
        private readonly UserManager<Account> _userManager;

        public DoctorService(UserManager<Account> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Account> GetDoctorByIdAsync(string doctorId)
        {
            return await _userManager.FindByIdAsync(doctorId);
        }

        public async Task<IEnumerable<Account>> GetAllDoctorsAsync()
        {
            return await _userManager.GetUsersInRoleAsync("Doctor");
        }

        public async Task<IdentityResult> CreateDoctorAsync(Account doctor, string password)
        {
            return await _userManager.CreateAsync(doctor, password);
        }

        public async Task<IdentityResult> UpdateDoctorAsync(Account doctor)
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
