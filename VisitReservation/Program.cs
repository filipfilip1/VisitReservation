using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Data;
using VisitReservation.Models;
using VisitReservation.Services;
using VisitReservation.Services.DataManagmentDoctor;
using VisitReservation.Services.UserManagmentServices.AdminServices;
using VisitReservation.Services.UserManagmentServices.DoctorServices;
using VisitReservation.Services.UserManagmentServices.PatientServices;

var builder = WebApplication.CreateBuilder(args);

// Dodanie u³ug do kontenera
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Konfiguracja Identity dla niestandardowego u¿ytkownika typu Account
builder.Services.AddDefaultIdentity<Account>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<Account>>();


// Konfiguracja Razor Pages
builder.Services.AddRazorPages();
builder.Services.AddAuthentication();

// Rejestracja serwisów
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();
builder.Services.AddScoped<IMedicalServiceService, MedicalServiceService>();
builder.Services.AddScoped<ITreatedDiseaseService, TreatedDiseaseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddHttpContextAccessor();
/*
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IPatientService, PatientService>();

*/




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<Account>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await ApplicationDbInitializer.SeedData(context, userManager, roleManager);
}


// Konfiguracja potoku ¿¹dañ HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // Domyœlna wartoœæ HSTS to 30 dni. Mo¿esz to zmieniæ dla scenariuszy produkcyjnych, zobacz https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Dodanie autentykacji i autoryzacji

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

