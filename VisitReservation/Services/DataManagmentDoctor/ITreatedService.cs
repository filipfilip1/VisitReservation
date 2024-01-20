using Microsoft.AspNetCore.Mvc.Rendering;
using VisitReservation.Models;

namespace VisitReservation.Services.DataManagmentDoctor
{
    public interface ITreatedDiseaseService
    {
        List<SelectListItem> GetTreatedDiseaseSelectList();
        void AssignTreatedDiseasesToDoctor(string doctorId, List<int> diseaseIds);
    }
}
