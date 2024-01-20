using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using VisitReservation.Data;
using VisitReservation.Models;
using VisitReservation.Models.LinkTables;


namespace VisitReservation.Services.DataManagmentDoctor
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ApplicationDbContext _context;

        public SpecializationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetSpecializationSelectList()
        {
            return _context.Specializations
                .Select(s => new SelectListItem
                {
                    Value = s.SpecializationId.ToString(),
                    Text = s.Name
                }).ToList();
        }

        public void AssignSpecializationsToDoctor(string doctorId, List<int> specializationIds)
        {
            var doctor = _context.Doctors.Find(doctorId);
            if (doctor == null)
            {
                throw new ArgumentException("Doctor not found.");
            }

            var existingSpecializations = _context.DoctorSpecializations.Where(ds => ds.DoctorId == doctorId);
            _context.DoctorSpecializations.RemoveRange(existingSpecializations);

            foreach (var specializationId in specializationIds)
            {
                var specialization = _context.Specializations.Find(specializationId);
                if (specialization != null)
                {
                    var doctorSpecialization = new DoctorSpecialization
                    {
                        DoctorId = doctorId,
                        SpecializationId = specializationId
                    };
                    _context.DoctorSpecializations.Add(doctorSpecialization);
                }
            }

            _context.SaveChanges();
        }
    }

}
