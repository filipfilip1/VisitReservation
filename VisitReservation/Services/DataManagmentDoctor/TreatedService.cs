using Microsoft.AspNetCore.Mvc.Rendering;
using VisitReservation.Data;
using VisitReservation.Models;
using VisitReservation.Models.LinkTables;

namespace VisitReservation.Services.DataManagmentDoctor
{
    public class TreatedDiseaseService : ITreatedDiseaseService
    {
        private readonly ApplicationDbContext _context;

        public TreatedDiseaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetTreatedDiseaseSelectList()
        {
            return _context.TreatedDiseases
                .Select(td => new SelectListItem
                {
                    Value = td.TreatedDiseaseId.ToString(),
                    Text = td.Name
                }).ToList();
        }


        public void AssignTreatedDiseasesToDoctor(string doctorId, List<int> diseaseIds)
        {
            var doctor = _context.Doctors.Find(doctorId);
            if (doctor == null)
            {
                throw new ArgumentException("Doctor not found.");
            }

            var existingDiseases = _context.DoctorTreatedDiseases.Where(dtd => dtd.DoctorId == doctorId);
            _context.DoctorTreatedDiseases.RemoveRange(existingDiseases);

            foreach (var diseaseId in diseaseIds)
            {
                var disease = _context.TreatedDiseases.Find(diseaseId);
                if (disease != null)
                {
                    var doctorDisease = new DoctorTreatedDisease
                    {
                        DoctorId = doctorId,
                        TreatedDiseaseId = diseaseId
                    };
                    _context.DoctorTreatedDiseases.Add(doctorDisease);
                }
            }

            _context.SaveChanges();
        }
    }
}
