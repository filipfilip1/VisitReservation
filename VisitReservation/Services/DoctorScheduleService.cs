﻿using VisitReservation.Data;
using VisitReservation.Models;
using Microsoft.EntityFrameworkCore;


namespace VisitReservation.Services
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly ApplicationDbContext _context;
        private const int AppointmentDurationMinutes = 30; // Stała długość wizyty

        public DoctorScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tworzy sloty wizyt dla danego lekarza w określonym zakresie czasowym
        public async Task CreateScheduleSlotsAsync(string doctorId, DateTime start, DateTime end)
        {
            var slots = GenerateSlots(doctorId, start, end);

            foreach (var slot in slots)
            {
                // Dodaj slot do bazy danych
                await _context.DoctorAppointmentSlots.AddAsync(slot);
            }

            await _context.SaveChangesAsync();
        }

        // Generuje sloty wizyt na podstawie podanego zakresu czasowego
        private IEnumerable<DoctorAppointmentSlot> GenerateSlots(string doctorId, DateTime start, DateTime end)
        {
            var slots = new List<DoctorAppointmentSlot>();
            var startTime = start;

            while (startTime < end)
            {
                var endTime = startTime.AddMinutes(AppointmentDurationMinutes);
                slots.Add(new DoctorAppointmentSlot
                {
                    DoctorId = doctorId,
                    StartTime = startTime,
                    Status = AppointmentSlotStatus.Available
                });

                startTime = endTime;
            }

            return slots;
        }
        public async Task SetDoctorWeeklyScheduleAsync(string doctorId, List<DayOfWeek> availableDays, TimeSpan startTime, TimeSpan endTime, int weeksForward)
        {
            var currentDate = DateTime.Today;
            var endDate = currentDate.AddDays(weeksForward * 7); // Ustalanie końca harmonogramu

            for (var date = currentDate; date < endDate; date = date.AddDays(1))
            {
                if (availableDays.Contains(date.DayOfWeek))
                {
                    var startDateTime = date + startTime;
                    var endDateTime = date + endTime;
                    await CreateScheduleSlotsAsync(doctorId, startDateTime, endDateTime);
                }
            }
        }

        public async Task<string> GetDoctorNameAsync(string doctorId)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            return doctor?.UserName ?? "Nieznany Lekarz";
        }

        public async Task<IList<DateTime>> GetAvailableDaysForDoctorAsync(string doctorId)
        {
            return await _context.DoctorAppointmentSlots
                .Where(slot => slot.DoctorId == doctorId && slot.StartTime > DateTime.Now)
                .Select(slot => slot.StartTime.Date)
                .Distinct()
                .ToListAsync();
        }
    }
}