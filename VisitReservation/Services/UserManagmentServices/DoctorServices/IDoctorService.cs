using Microsoft.AspNetCore.Identity;
using VisitReservation.Models;

namespace VisitReservation.Services.UserManagmentServices.DoctorServices
{
    public interface IDoctorService
    {
        Task<Account> GetDoctorByIdAsync(string doctorId);
        Task<IEnumerable<Account>> GetAllDoctorsAsync();
        Task<IdentityResult> CreateDoctorAsync(Account doctor, string password);
        Task<IdentityResult> UpdateDoctorAsync(Account doctor);
        Task<IdentityResult> DeleteDoctorAsync(string doctorId);
    }

}
