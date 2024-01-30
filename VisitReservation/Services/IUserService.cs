namespace VisitReservation.Services
{
    public interface IUserService
    {
        Task<bool> IsCurrentUser(string userId);
        Task<bool> IsAdmin();
        Task<bool> IsDoctor();
        Task<bool> IsPatient();
        // Task<bool> IsDoctorAwaitingApproval();
    }
}




