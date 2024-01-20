using VisitReservation.Models;

namespace VisitReservation.Services.DataManagmentDoctor
{
    public interface IMedicalServiceService
    {
        List<MedicalService> GetAllMedicalServices();
        void AssignMedicalServicesToDoctor(string doctorId, List<int> serviceIds);
    }
}
