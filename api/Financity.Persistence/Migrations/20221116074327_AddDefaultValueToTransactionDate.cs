using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Financity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValueToTransactionDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("80c3d1bd-0ae2-4594-a96b-bb600b6d2c7f"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("ed1c273f-d57a-41c3-94e8-f465ae02cf4c"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("ff648f1d-191e-45c8-8d0e-2545b303664d"));

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("4851b010-1c81-4825-b72e-fb44641069dd"), "EUR", "Euro" },
                    { new Guid("c5f2ba40-716e-4792-873e-1ac788242ebf"), "PLN", "Polski Złoty" },
                    { new Guid("ee13b663-0bc4-477d-93ca-545bcc960b34"), "USD", "United States Dollar" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("4851b010-1c81-4825-b72e-fb44641069dd"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("c5f2ba40-716e-4792-873e-1ac788242ebf"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("ee13b663-0bc4-477d-93ca-545bcc960b34"));

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("80c3d1bd-0ae2-4594-a96b-bb600b6d2c7f"), "USD", "United States Dollar" },
                    { new Guid("ed1c273f-d57a-41c3-94e8-f465ae02cf4c"), "PLN", "Polski Złoty" },
                    { new Guid("ff648f1d-191e-45c8-8d0e-2545b303664d"), "EUR", "Euro" }
                });
        }
    }
}
