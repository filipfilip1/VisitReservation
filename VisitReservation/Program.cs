using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitReservation.Data;
using VisitReservation.Models;
using VisitReservation.Services.DataManagmentDoctor;
using VisitReservation.Services.UserManagmentServices.AdminServices;
using VisitReservation.Services.UserManagmentServices.DoctorServices;
using VisitReservation.Services.UserManagmentServices.PatientServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// dodanie niestandardowych modeli u¿ytkownika
builder.Services.AddIdentityCore<Admin>() // Dla admina
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityCore<Patient>() // Dla pacjenta
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityCore<Doctor>() // Dla lekarza
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// skonfigurowanie serwisu pod obs³ugê ról
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

// Rejestracja serwisów
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();
builder.Services.AddScoped<IMedicalServiceService, MedicalServiceService>();
builder.Services.AddScoped<ITreatedDiseaseService, TreatedDiseaseService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// dodanie autentykacji
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
