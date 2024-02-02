using Microsoft.EntityFrameworkCore;
using VisitReservation.Data;
using VisitReservation.Models;
using VisitReservation.Services;

namespace VisitReservation.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public AppointmentService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // Pobiera wizytę na podstawie ID i sprawdza, czy użytkownik ma do niej dostęp
        public async Task<Appointment> GetAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            // sprawdzenie czy zalogowany użytkownik jest pacjentem, lekarzem lub administratorem związanym z wizytą
            if (!await _userService.IsCurrentUser(appointment.PatientId) &&
                !await _userService.IsCurrentUser(appointment.DoctorId) &&
                !await _userService.IsAdmin())
            {
                throw new UnauthorizedAccessException("Access denied.");
            }

            return appointment;
        }

        // Tworzy nową wizytę z danymi przekazanymi przez użytkownika
        public async Task<Appointment> CreateAppointmentAsync(string doctorId, string userId, DateTime appointmentDateTime)
        {
            Appointment appointment = new Appointment();

            // Sprawdzenie, czy użytkownik ma uprawnienia do utworzenia wizyty
            if (!await _userService.IsCurrentUser(userId) && !await _userService.IsAdmin())
            {
                throw new UnauthorizedAccessException("User is not authorized to create an appointment.");
            }

            // Ustawienie DoctorId i PatientId dla wizyty
            appointment.DoctorId = doctorId;
            appointment.PatientId = userId;

            // Ustawienie daty i godziny wizyty
            appointment.AppointmentDateTime = appointmentDateTime;

            // Ustawienie domyślnego statusu wizyty na Pending
            appointment.AppointmentStatus = AppointmentStatus.Pending;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }


        // Aktualizacja dane istniejącej wizyty
        public async Task UpdateAppointmentAsync(Appointment appointment, string userId)
        {
            var existingAppointment = await _context.Appointments.FindAsync(appointment.AppointmentId);
            if (existingAppointment == null)
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            // sprawdzenie czy użytkownik ma uprawnienia do modyfikacji wizyty
            if (!await IsUserAuthorizedToModifyAsync(userId, appointment.AppointmentId))
            {
                throw new UnauthorizedAccessException("User is not authorized to modify this appointment.");
            }

            // Aktualizowanie danych wizyty
            existingAppointment.AppointmentDateTime = appointment.AppointmentDateTime;
            await _context.SaveChangesAsync();
        }

        // Usuwa wizytę na podstawie ID
        public async Task DeleteAppointmentAsync(int appointmentId, string userId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            // Sprawdź, czy użytkownik ma uprawnienia do usunięcia wizyty
            if (!await IsUserAuthorizedToModifyAsync(userId, appointmentId))
            {
                throw new UnauthorizedAccessException("User is not authorized to delete this appointment.");
            }

            // Usuń wizytę z bazy danych
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        // Potwierdza wizytę, zmieniając jej status
        public async Task<AppointmentStatus> ConfirmAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            // Sprawdź, czy wizyta jest oczekująca
            if (appointment.AppointmentStatus != AppointmentStatus.Pending)
            {
                throw new InvalidOperationException("Appointment is not pending and cannot be confirmed.");
            }

            appointment.AppointmentStatus = AppointmentStatus.Confirmed;
            await _context.SaveChangesAsync();

            return AppointmentStatus.Confirmed;
        }


        // Anuluje wizytę, zmieniając jej status
        public AppointmentStatus CancelAppointment(int appointmentId)
        {
            var appointment = _context.Appointments.Find(appointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            // Sprawdź, czy wizyta jest oczekująca
            if (!IsAppointmentPending(appointmentId))
            {
                throw new InvalidOperationException("Appointment is not pending and cannot be cancelled.");
            }

            appointment.AppointmentStatus = AppointmentStatus.Cancelled;
            _context.SaveChanges();

            return AppointmentStatus.Cancelled;
        }

        // Zmienia termin wizyty
        public AppointmentStatus RescheduleAppointment(int appointmentId, DateTime newDate)
        {
            var appointment = _context.Appointments.Find(appointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            // Sprawdź, czy wizyta jest oczekująca
            if (!IsAppointmentPending(appointmentId))
            {
                throw new InvalidOperationException("Appointment is not pending and cannot be rescheduled.");
            }

            appointment.AppointmentDateTime = newDate;
            _context.SaveChanges();

            return AppointmentStatus.Rescheduled;
        }

        public async Task<List<Appointment>> GetPendingAppointmentsForDoctorAsync(string doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && a.AppointmentStatus == AppointmentStatus.Pending)
                .ToListAsync();
        }


        public async Task<IList<Appointment>> GetPastAppointmentsForDoctorAsync(string doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDateTime < DateTime.Now)
                .OrderByDescending(a => a.AppointmentDateTime)
                .ToListAsync();
        }

        public async Task<IList<Appointment>> GetUpcomingAppointmentsForDoctorAsync(string doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient) 
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.AppointmentDateTime >= DateTime.Now &&
                    a.AppointmentStatus == AppointmentStatus.Confirmed) 
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync();
        }


        public async Task<IList<Appointment>> GetPastAppointmentsForPatientAsync(string patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDateTime < DateTime.Now)
                .ToListAsync();
        }

        public async Task<IList<Appointment>> GetUpcomingAppointmentsForPatientAsync(string patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDateTime >= DateTime.Now)
                .ToListAsync();
        }


        // Pomocnicza metoda sprawdzająca, czy użytkownik jest autoryzowany do modyfikacji wizyty
        private async Task<bool> IsUserAuthorizedToModifyAsync(string userId, int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            // Sprawdzenie, czy userId odpowiada DoctorId lub PatientId dla tej wizyty, lub czy użytkownik jest adminem
            return appointment.DoctorId == userId || appointment.PatientId == userId || await _userService.IsAdmin();
        }


        // Pomocnicza metoda sprawdzająca, czy wizyta jest oczekująca
        private bool IsAppointmentPending(int appointmentId)
        {
            var appointment = _context.Appointments.Find(appointmentId);
            return appointment != null && appointment.AppointmentStatus == AppointmentStatus.Pending;
        }


    }

}


