using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Financity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentPeriodExpensesToBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("34f90ceb-f4b5-4a36-a105-c031e0206082"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("392902ec-f1d1-4198-a5d5-49d56257efa4"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("a6bbe70a-9794-4c06-9728-efd1f59e2327"));

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("4f50a462-2c9c-42ed-8d0a-4de33b3997de"), "EUR", "Euro" },
                    { new Guid("5e630489-4965-4314-aabd-6209972eae98"), "PLN", "Polski Złoty" },
                    { new Guid("a3b3b070-6a59-4faf-93bf-c9e515393cdd"), "USD", "United States Dollar" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("4f50a462-2c9c-42ed-8d0a-4de33b3997de"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("5e630489-4965-4314-aabd-6209972eae98"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("a3b3b070-6a59-4faf-93bf-c9e515393cdd"));

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("34f90ceb-f4b5-4a36-a105-c031e0206082"), "USD", "United States Dollar" },
                    { new Guid("392902ec-f1d1-4198-a5d5-49d56257efa4"), "PLN", "Polski Złoty" },
                    { new Guid("a6bbe70a-9794-4c06-9728-efd1f59e2327"), "EUR", "Euro" }
                });
        }
    }
}
