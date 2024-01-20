using Microsoft.AspNetCore.Mvc.Rendering;
using VisitReservation.Models;

namespace VisitReservation.Services.DataManagmentDoctor
{
    public interface IMedicalServiceService
    {
        List<SelectListItem> GetMedicalServiceSelectList();
        void AssignMedicalServicesToDoctor(string doctorId, List<int> serviceIds);
    }
}
