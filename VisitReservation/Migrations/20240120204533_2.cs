using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitReservation.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash" },
                values: new object[] { "3ec1dd15-2995-4439-a47c-4f13f28caaf8", true, "AQAAAAIAAYagAAAAENlVauwN36WzQvjZ2UtxeqEJr12RD0B1kDVJVY8T5RfaZ+1iNIDjyZXDBIe8rmpXZQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash" },
                values: new object[] { "ac7a0222-a930-4b49-adfb-094861344832", false, "AQAAAAIAAYagAAAAEEauhrI8r1wMwtMs9+YmjtRceH/zonZFfRt9DFh2Xcg1B5lLSVVL7UHT+A4j0OKDtQ==" });
        }
    }
}
