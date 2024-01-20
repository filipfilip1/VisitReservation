using Microsoft.AspNetCore.Mvc.Rendering;

namespace VisitReservation.Services.DataManagmentDoctor
{
    public interface ISpecializationService
    {
        List<SelectListItem> GetSpecializationSelectList();
        void AssignSpecializationsToDoctor(string doctorId, List<int> specializationIds);
    }
}
