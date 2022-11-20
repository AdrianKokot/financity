using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Financity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionsCollectionToWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("4773a5ae-eb68-469f-8afc-d6cda74b50a1"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("66f001bf-6ef5-4f2e-894a-d0f56c830368"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("ee9f493b-cf8e-45d1-b48e-d4e2905c7d2f"));

            migrationBuilder.AddColumn<decimal>(
                name: "StartingAmount",
                table: "Wallets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "Categories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("222a9676-2366-4a63-85c1-f892af3635bc"), "EUR", "Euro" },
                    { new Guid("34fa9244-3524-46c2-bb75-794a6ac5f82d"), "USD", "United States Dollar" },
                    { new Guid("fe72159d-8edc-4e04-a4de-561ac953b592"), "PLN", "Polski Złoty" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("222a9676-2366-4a63-85c1-f892af3635bc"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("34fa9244-3524-46c2-bb75-794a6ac5f82d"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("fe72159d-8edc-4e04-a4de-561ac953b592"));

            migrationBuilder.DropColumn(
                name: "StartingAmount",
                table: "Wallets");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "Categories",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("4773a5ae-eb68-469f-8afc-d6cda74b50a1"), "PLN", "Polski Złoty" },
                    { new Guid("66f001bf-6ef5-4f2e-894a-d0f56c830368"), "EUR", "Euro" },
                    { new Guid("ee9f493b-cf8e-45d1-b48e-d4e2905c7d2f"), "USD", "United States Dollar" }
                });
        }
    }
}
