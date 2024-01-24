using Microsoft.AspNetCore.Identity;
using VisitReservation.Models.LinkTables;

namespace VisitReservation.Models
{
    public class Patient : Account
    { 
        public DateTime DateOfBirth { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
