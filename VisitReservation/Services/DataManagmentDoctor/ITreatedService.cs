using VisitReservation.Models;

namespace VisitReservation.Services.DataManagmentDoctor
{
    public interface ITreatedDiseaseService
    {
        List<TreatedDisease> GetAllTreatedDiseases();
        void AssignTreatedDiseasesToDoctor(string doctorId, List<int> diseaseIds);
    }
}
s