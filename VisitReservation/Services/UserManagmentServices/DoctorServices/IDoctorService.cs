using Microsoft.AspNetCore.Identity;
using VisitReservation.Models;

namespace VisitReservation.Services.UserManagmentServices.DoctorServices
{
    public interface IDoctorService
    {
        Task<Doctor> GetDoctorByIdAsync(string doctorId);
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<IdentityResult> CreateDoctorAsync(Doctor doctor, string password);
        Task<IdentityResult> UpdateDoctorAsync(Doctor doctor);
        Task<IdentityResult> DeleteDoctorAsync(string doctorId);
    }

}
