using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Models;

namespace VisitReservation.Data
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedData(ApplicationDbContext context, UserManager<Account> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedAdminUser(userManager, roleManager);
            await SeedOtherEntities(context);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Patient", "Doctor", "Admin" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName);
                    await roleManager.CreateAsync(role);
                }
            }
        }


        private static async Task SeedAdminUser(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await userManager.Users.AnyAsync(u => u.UserName == "admin"))
            {
                var adminUser = new Admin
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };

                var passwordHasher = new PasswordHasher<Account>();
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Adminadmin1!");

                var result = await userManager.CreateAsync(adminUser);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }


        private static async Task SeedOtherEntities(ApplicationDbContext context)
        {
            bool isModified = false;

            if (!await context.Specializations.AnyAsync())
            {
                context.Specializations.AddRange(
                    new Specialization { Name = "Kardiologia" },
                    new Specialization { Name = "Neurologia" },
                    new Specialization { Name = "Pediatria" }
                );
                isModified = true;
            }

            if (!await context.Educations.AnyAsync())
            {
                context.Educations.AddRange(
                    new Education {  University = "Uniwersytet Medyczny w Warszawie" },
                    new Education {  University = "Uniwersytet Medyczny w Krakowie" },
                    new Education {  University = "Gdański Uniwersytet Medyczny" }
                );
                isModified = true;
            }

            if (!await context.MedicalServices.AnyAsync())
            {
                context.MedicalServices.AddRange(
                    new MedicalService { Name = "Badanie EKG" },
                    new MedicalService { Name = "USG jamy brzusznej" },
                    new MedicalService { Name = "Konsultacja onkologiczna" }
                );
                isModified = true;
            }

            if (!await context.TreatedDiseases.AnyAsync())
            {
                context.TreatedDiseases.AddRange(
                    new TreatedDisease { Name = "Cukrzyca" },
                    new TreatedDisease { Name = "Astma" },
                    new TreatedDisease { Name = "Choroba Parkinsona" }
                );
                isModified = true;
            }

            if (isModified )
            {
                await context.SaveChangesAsync();
            }

        }
    }
}

