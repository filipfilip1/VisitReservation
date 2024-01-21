using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitReservation.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "574efcda-5cc6-4501-93fb-3a8a4b3b3fee", "AQAAAAIAAYagAAAAENStlEmoYoLO8E1T1vcUvr28q/fDaq18rbRuo/l/O7j5NMvbnSUFeZNM3Wk0HO5d6A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "28c9b523-66d6-4f8b-8016-eddb290b3edf", "AQAAAAIAAYagAAAAEKK13UuKq+rXd2kE666v6+WA37QVuHPGHLRvhSDldyBTbRF/rfLnCHdWd5qpXZ3l6A==" });
        }
    }
}
