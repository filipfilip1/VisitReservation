using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisitReservation.Models;
using VisitReservation.Services;
using VisitReservation.Services.UserManagmentServices.DoctorServices;

namespace VisitReservation.Pages
{
    public class AvailableDoctorsModel : PageModel
    {
        private readonly IDoctorService _doctorService;

        public AvailableDoctorsModel(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IList<Account> AvailableDoctors { get; set; }

        public async Task OnGetAsync()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            AvailableDoctors = doctors.ToList();
        }
    }
}
