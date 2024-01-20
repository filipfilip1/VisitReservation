using Microsoft.AspNetCore.Identity;

namespace VisitReservation.Services
{
    public class RolesInitializer
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // tworzenie nowego zakresu zależności
            using (var scope = serviceProvider.CreateScope())
            {
                // pobiera instancje RoleManager
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { "Admin", "Patient", "Doctor" };
                foreach (var roleName in roleNames)
                {
                    var role = await roleManager.RoleExistsAsync(roleName);
                    if (!role)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }
    }
}
