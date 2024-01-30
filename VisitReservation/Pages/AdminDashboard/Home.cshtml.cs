using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Models;
using VisitReservation.Services;


namespace VisitReservation.Pages.AdminDashboard
{
    public class HomeModel : PageModel
    {
        private readonly UserManager<Account> _userManager;
        // private readonly IReportService _reportService; // Us�uga do zarz�dzania zg�oszeniami

        public HomeModel(UserManager<Account> userManager /*IReportService reportService*/)
        {
            _userManager = userManager;
            // utworzenie tej listy zapobiega odwo�ywaniu si� do null po zatwierdzeniu lekarza
            UsersToApprove = new List<Account>();
            // _reportService = reportService;
        }

        public List<Account> UsersToApprove { get; set; }
        public List<Report> ReportsToReview { get; set; } 

        public async Task OnGetAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            UsersToApprove = new List<Account>();

            foreach (var user in allUsers)
            {
                if (user is Doctor && !await _userManager.IsInRoleAsync(user, "Doctor"))
                {
                    UsersToApprove.Add(user);
                }
            }

            // ReportsToReview = await _reportService.GetPendingReportsAsync(); // Pobieranie zg�osze�
        }

        public async Task<IActionResult> OnPostApproveAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && user is Doctor)
            {
                await _userManager.AddToRoleAsync(user, "Doctor");
            }

            return RedirectToPage();
        }

        /*
        public async Task<IActionResult> OnPostReviewAsync(string reportId)
        {
            // Logika obs�ugi zg�osze�
            // await _reportService.HandleReportAsync(reportId);

            return RedirectToPage();
        }
        */
    }
}
