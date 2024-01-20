using Microsoft.AspNetCore.Identity;

namespace VisitReservation.Models
{
    // Model pusty, wystarczą bazowe pola z klasy IdentityUser
    // Najważeniejsze właściwości będą wynikać z otrzymywanej roli "admin"
    public class Admin : IdentityUser
    {
    }
}
