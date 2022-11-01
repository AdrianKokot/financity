using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financity.Persistence.Migrations
{
    public partial class AddColorAndIconToLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("49cb9206-1c43-42e5-a202-8d747621c091"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("81ade7e4-e4d0-45a0-a5c5-eb2f25417238"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("aadbb33c-4157-40fd-b4b2-546961729166"));

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconName",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("9fe44097-b5a2-4a17-88b5-7940ec81b5d5"), "EUR", "Euro" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("bf1fad19-4c31-4226-b886-2a4ba53d4d19"), "PLN", "Polski Złoty" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("cb95bed3-bd67-4046-8a58-c248e9ed70db"), "USD", "United States Dollar" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("9fe44097-b5a2-4a17-88b5-7940ec81b5d5"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("bf1fad19-4c31-4226-b886-2a4ba53d4d19"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("cb95bed3-bd67-4046-8a58-c248e9ed70db"));

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "IconName",
                table: "Labels");

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("49cb9206-1c43-42e5-a202-8d747621c091"), "EUR", "Euro" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("81ade7e4-e4d0-45a0-a5c5-eb2f25417238"), "PLN", "Polski Złoty" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("aadbb33c-4157-40fd-b4b2-546961729166"), "USD", "United States Dollar" });
        }
    }
}
