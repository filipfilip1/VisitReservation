using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VisitReservation.Models;

namespace VisitReservation.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Account> _userManager;

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<Account> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<bool> IsCurrentUser(string userId)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return currentUserId == userId;
        }

        public async Task<bool> IsAdmin()
        {
            var user = await GetCurrentUser();
            return user != null && await _userManager.IsInRoleAsync(user, "Admin");
        }

        public async Task<bool> IsDoctor()
        {
            var user = await GetCurrentUser();
            return user != null && await _userManager.IsInRoleAsync(user, "Doctor");
        }

        public async Task<bool> IsPatient()
        {
            var user = await GetCurrentUser();
            return user != null && await _userManager.IsInRoleAsync(user, "Patient");
        }

        /*
        // ewentualne sprawdzenie czy doktor czeka na akceptacje, tymczasowo niepotrzebne
        public async Task<bool> IsDoctorAwaitingApproval()
        {
            var user = await GetCurrentUser();
            // Załóżmy, że "DoctorAwaitingApproval" to status w modelu użytkownika
            return user != null && user.Status == "DoctorAwaitingApproval";
        }
        */

        private async Task<Account> GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userManager.FindByIdAsync(userId);
        }
    }
}


