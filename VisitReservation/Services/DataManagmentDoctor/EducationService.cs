using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using VisitReservation.Data;
using VisitReservation.Models;
using VisitReservation.Models.LinkTables;
using VisitReservation.Services.DataManagmentDoctor;

public class EducationService : IEducationService
{
    private readonly ApplicationDbContext _context;

    public EducationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<SelectListItem> GetEducationSelectList()
    {
        return _context.Educations
            .Select(e => new SelectListItem
            {
                Value = e.EducationId.ToString(),
                Text = e.University 
            }).ToList();
    }

    public void AssignEducationsToDoctor(string doctorId, List<int> educationIds)
    {
        var doctor = _context.Doctors.Find(doctorId);
        if (doctor == null)
        {
            throw new ArgumentException("Doctor not found.");
        }

        var existingEducations = _context.DoctorEducations.Where(de => de.DoctorId == doctorId);
        _context.DoctorEducations.RemoveRange(existingEducations);

        foreach (var educationId in educationIds)
        {
            var education = _context.Educations.Find(educationId);
            if (education != null)
            {
                var doctorEducation = new DoctorEducation
                {
                    DoctorId = doctorId,
                    EducationId = educationId
                };
                _context.DoctorEducations.Add(doctorEducation);
            }
        }

        _context.SaveChanges();
    }
}
