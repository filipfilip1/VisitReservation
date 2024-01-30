using VisitReservation.Data;
using VisitReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace VisitReservation.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public BookingService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        public async Task<IEnumerable<DoctorAppointmentSlot>> CheckAvailabilityAsync(DateTime startDate, DateTime endDate, string doctorId)
        {
            // Pobieranie czasu trwania wizyty z appsettings.json
            var appointmentDuration = _configuration.GetValue<int>("AppointmentSettings:DurationMinutes");

            // Zwraca dostępne terminy dla danego lekarza w określonym zakresie dat
            return await _context.DoctorAppointmentSlots
                .Where(slot => slot.DoctorId == doctorId &&
                               slot.StartTime >= startDate &&
                               slot.StartTime.AddMinutes(appointmentDuration) <= endDate &&
                               slot.Status == AppointmentSlotStatus.Available)
                .ToListAsync();
        }


        public async Task<Appointment> BookAppointmentAsync(Appointment appointment)
        {
            // Tworzy nową wizytę na podstawie dostępnego terminu
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            // Anuluje wizytę
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Appointment> RescheduleAppointmentAsync(int appointmentId, DateTime newDateTime)
        {
            // Zmienia termin wizyty
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.AppointmentDateTime = newDateTime;
                await _context.SaveChangesAsync();
                return appointment;
            }
            return null;
        }


        // metoda generuje sloty czasowe od istniejących wizyt
        public async Task<IList<DoctorAppointmentSlot>> GetAvailableTimeSlotsForDayAsync(DateTime date, string doctorId)
        {
            // Zakładamy, że jedna wizyta trwa 30 minut
            var duration = TimeSpan.FromMinutes(30);

            // Pobierz wszystkie wizyty lekarza w danym dniu
            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.AppointmentDateTime.Date == date.Date)
                .ToListAsync();

            // Generowanie dostępnych slotów czasowych
            var slots = new List<DoctorAppointmentSlot>();
            for (var time = date.Date; time < date.Date.AddDays(1); time = time.Add(duration))
            {
                if (!appointments.Any(a => a.AppointmentDateTime == time))
                {
                    slots.Add(new DoctorAppointmentSlot { StartTime = time });
                }
            }

            return slots;
        }

        public async Task<IList<DateTime>> GetDistinctAvailableDaysForDoctorAsync(string doctorId)
        {
            // Zwraca unikalne daty, w których lekarz ma dostępne sloty czasowe
            return await _context.DoctorAppointmentSlots
                .Where(slot => slot.DoctorId == doctorId &&
                               slot.StartTime > DateTime.Now &&
                               slot.Status == AppointmentSlotStatus.Available)
                .Select(slot => slot.StartTime.Date)
                .Distinct()
                .OrderBy(date => date) 
                .ToListAsync();
        }



    }
}
