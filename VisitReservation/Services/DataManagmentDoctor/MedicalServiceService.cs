using VisitReservation.Data;
using VisitReservation.Models;
using VisitReservation.Models.LinkTables;

namespace VisitReservation.Services.DataManagmentDoctor
{
    public class MedicalServiceService : IMedicalServiceService
    {
        private readonly ApplicationDbContext _context;

        public MedicalServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MedicalService> GetAllMedicalServices()
        {
            return _context.MedicalServices.ToList();
        }


        public void AssignMedicalServicesToDoctor(string doctorId, List<int> serviceIds)
        {
            var doctor = _context.Doctors.Find(doctorId);
            if (doctor == null)
            {
                throw new ArgumentException("Doctor not found.");
            }

            var existingServices = _context.DoctorMedicalServices.Where(dms => dms.DoctorId == doctorId);
            _context.DoctorMedicalServices.RemoveRange(existingServices);

            foreach (var serviceId in serviceIds)
            {
                var service = _context.MedicalServices.Find(serviceId);
                if (service != null)
                {
                    var doctorService = new DoctorMedicalService
                    {
                        DoctorId = doctorId,
                        MedicalServiceId = serviceId
                    };
                    _context.DoctorMedicalServices.Add(doctorService);
                }
            }

            _context.SaveChanges();
        }

    }
}
