using Microsoft.AspNetCore.Identity;
using VisitReservation.Models;

namespace VisitReservation.Services.UserManagmentServices.AdminServices
{
    public interface IAdminService
    {
        Task<Admin> GetAdminByIdAsync(string adminId);
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
        Task<IdentityResult> CreateAdminAsync(Admin admin, string password);
        Task<IdentityResult> UpdateAdminAsync(Admin admin);
        Task<IdentityResult> DeleteAdminAsync(string adminId);
    }

}
