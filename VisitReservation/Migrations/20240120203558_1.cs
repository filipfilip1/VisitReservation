using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VisitReservation.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "ac7a0222-a930-4b49-adfb-094861344832", "IdentityUser", "admin@admin.com", false, false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAIAAYagAAAAEEauhrI8r1wMwtMs9+YmjtRceH/zonZFfRt9DFh2Xcg1B5lLSVVL7UHT+A4j0OKDtQ==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "Educations",
                columns: new[] { "EducationId", "University" },
                values: new object[,]
                {
                    { 1, "Uniwersytet Medyczny w Warszawie" },
                    { 2, "Uniwersytet Medyczny w Krakowie" },
                    { 3, "Gdański Uniwersytet Medyczny" }
                });

            migrationBuilder.InsertData(
                table: "MedicalServices",
                columns: new[] { "MedicalServiceId", "Name" },
                values: new object[,]
                {
                    { 1, "Badanie EKG" },
                    { 2, "USG jamy brzusznej" },
                    { 3, "Konsultacja onkologiczna" }
                });

            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "SpecializationId", "Name" },
                values: new object[,]
                {
                    { 1, "Kardiologia" },
                    { 2, "Neurologia" },
                    { 3, "Pediatria" }
                });

            migrationBuilder.InsertData(
                table: "TreatedDiseases",
                columns: new[] { "TreatedDiseaseId", "Name" },
                values: new object[,]
                {
                    { 1, "Cukrzyca" },
                    { 2, "Astma" },
                    { 3, "Choroba Parkinsona" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3", "1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "1" });

            migrationBuilder.DeleteData(
                table: "Educations",
                keyColumn: "EducationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Educations",
                keyColumn: "EducationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Educations",
                keyColumn: "EducationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MedicalServices",
                keyColumn: "MedicalServiceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MedicalServices",
                keyColumn: "MedicalServiceId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MedicalServices",
                keyColumn: "MedicalServiceId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TreatedDiseases",
                keyColumn: "TreatedDiseaseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TreatedDiseases",
                keyColumn: "TreatedDiseaseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TreatedDiseases",
                keyColumn: "TreatedDiseaseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");
        }
    }
}
