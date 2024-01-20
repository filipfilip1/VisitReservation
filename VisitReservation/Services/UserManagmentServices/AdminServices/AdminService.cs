using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Models;

namespace VisitReservation.Services.UserManagmentServices.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<Admin> _userManager;

        public AdminService(UserManager<Admin> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Admin> GetAdminByIdAsync(string adminId)
        {
            return await _userManager.FindByIdAsync(adminId);
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> CreateAdminAsync(Admin admin, string password)
        {
            return await _userManager.CreateAsync(admin, password);
        }
        public async Task<IdentityResult> UpdateAdminAsync(Admin admin)
        {
            return await _userManager.UpdateAsync(admin);
        }

        public async Task<IdentityResult> DeleteAdminAsync(string adminId)
        {
            var admin = await _userManager.FindByIdAsync(adminId);
            return admin != null ? await _userManager.DeleteAsync(admin) : IdentityResult.Failed();
        }
    }

}
