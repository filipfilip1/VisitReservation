using Microsoft.AspNetCore.Mvc.Rendering;

namespace VisitReservation.Services.DataManagmentDoctor
{
    public interface IEducationService
    {
        List<SelectListItem> GetEducationSelectList();
        void AssignEducationsToDoctor(string doctorId, List<int> educationIds);
    }
}
