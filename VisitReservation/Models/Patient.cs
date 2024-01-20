using Microsoft.AspNetCore.Identity;
using VisitReservation.Models.LinkTables;

namespace VisitReservation.Models
{
    public class Patient : IdentityUser 
    { 
        public DateTime DateOfBirth { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
