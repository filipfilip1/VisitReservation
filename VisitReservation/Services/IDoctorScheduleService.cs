namespace VisitReservation.Services
{
    public interface IDoctorScheduleService
    {
        Task CreateScheduleSlotsAsync(string doctorId, DateTime start, DateTime end);
        Task SetDoctorWeeklyScheduleAsync(string doctorId, List<DayOfWeek> availableDays, TimeSpan startTime, TimeSpan endTime, int weeksForward);
        Task<string> GetDoctorNameAsync(string doctorId);
        Task<IList<DateTime>> GetAvailableDaysForDoctorAsync(string doctorId);
    }
}
