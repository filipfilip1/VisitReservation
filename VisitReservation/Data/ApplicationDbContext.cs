using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using VisitReservation.Models;
using VisitReservation.Models.LinkTables;

namespace VisitReservation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<MedicalService> MedicalServices { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<TreatedDisease> TreatedDiseases { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<DoctorEducation> DoctorEducations { get; set; }
        public DbSet<DoctorMedicalService> DoctorMedicalServices { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        public DbSet<DoctorTreatedDisease> DoctorTreatedDiseases { get; set; }

        // konfiguracja serwisu do początkowego ustawienia specyficznych wamagań dla rejestracji i logowania
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Ustawienia hasła
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;


                // Ustawienia logowania
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // początkowe tworzenie ról
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Patient", NormalizedName = "PATIENT" },
                new IdentityRole { Id = "2", Name = "Doctor", NormalizedName = "DOCTOR" },
                new IdentityRole { Id = "3", Name = "Admin", NormalizedName = "ADMIN" }
            );



            // tworzenie relacji wiele do wielu
            // relacja wiele do wielu Doctor - Specialization
            builder.Entity<DoctorSpecialization>()
                .HasKey(ds => new { ds.DoctorId, ds.SpecializationId });

            builder.Entity<DoctorSpecialization>()
                .HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSpecializations)
                .HasForeignKey(ds => ds.DoctorId);

            builder.Entity<DoctorSpecialization>()
                .HasOne(ds => ds.Specialization)
                .WithMany(s => s.DoctorSpecializations)
                .HasForeignKey(ds => ds.SpecializationId);

            // relacja wiele do wielu Doctor - Education
            builder.Entity<DoctorEducation>()
               .HasKey(de => new { de.DoctorId, de.EducationId });

            builder.Entity<DoctorEducation>()
                .HasOne(de => de.Doctor)
                .WithMany(d => d.DoctorEducations)
                .HasForeignKey(de => de.DoctorId);

            builder.Entity<DoctorEducation>()
                .HasOne(de => de.Education)
                .WithMany(e => e.DoctorEducations)
                .HasForeignKey(de => de.EducationId);

            // relacja wiele do wielu dla Doctor - MedicalService
            builder.Entity<DoctorMedicalService>()
                .HasKey(dms => new { dms.DoctorId, dms.MedicalServiceId });

            builder.Entity<DoctorMedicalService>()
                .HasOne(dms => dms.Doctor)
                .WithMany(d => d.DoctorMedicalServices)
                .HasForeignKey(dms => dms.DoctorId);

            builder.Entity<DoctorMedicalService>()
                .HasOne(dms => dms.MedicalService)
                .WithMany(ms => ms.DoctorMedicalServices)
                .HasForeignKey(dms => dms.MedicalServiceId);

            // relacja wiele do wielu dla Doctor - TreatedDisease
            builder.Entity<DoctorTreatedDisease>()
                .HasKey(dtd => new { dtd.DoctorId, dtd.TreatedDiseaseId });

            builder.Entity<DoctorTreatedDisease>()
                .HasOne(dtd => dtd.Doctor)
                .WithMany(d => d.DoctorTreatedDiseases)
                .HasForeignKey(dtd => dtd.DoctorId);

            builder.Entity<DoctorTreatedDisease>()
                .HasOne(dtd => dtd.TreatedDisease)
                .WithMany(td => td.DoctorTreatedDiseases)
                .HasForeignKey(dtd => dtd.TreatedDiseaseId);


            // tworzenie relacji wiele do jednego
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Patient)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Doctor)
                .WithMany(d => d.Reviews)
                .HasForeignKey(r => r.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Doctor>()
                .HasMany(d => d.Availabilities)
                .WithOne(a => a.Doctor)
                .HasForeignKey(da => da.DoctorId);

            builder.Entity<Report>()
                .HasOne(r => r.Review)
                .WithMany(rev => rev.Reports)
                .HasForeignKey(r => r.ReviewId);

            builder.Entity<Report>()
                .HasOne(r => r.Doctor)
                .WithMany(d => d.Reports)
                .HasForeignKey(r => r.DoctorId);


            // ustawienie precyzji dla typu decimal
            builder.Entity<Review>()
                .Property(r => r.Rating)
                .HasColumnType("decimal(2, 1)");


        }
    }
}