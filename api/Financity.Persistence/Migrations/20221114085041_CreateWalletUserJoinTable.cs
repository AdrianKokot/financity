using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financity.Persistence.Migrations
{
    public partial class CreateWalletUserJoinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Users_UserId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("3729bbe9-6321-453f-8851-9a98b7130479"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("45cc816b-c933-4fa0-be64-fff9542ce98b"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("805e3f33-8ead-4c42-a9c9-263480c38555"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Wallets");

            migrationBuilder.CreateTable(
                name: "WalletAccesses",
                columns: table => new
                {
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletAccessLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletAccesses", x => new { x.UserId, x.WalletId });
                    table.ForeignKey(
                        name: "FK_WalletAccesses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletAccesses_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("b83415ce-6c66-4a2b-acb1-f29429097ad3"), "USD", "United States Dollar" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("ebd5787e-ef20-40d1-8867-e1089f197179"), "EUR", "Euro" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("f6450bd1-8c6f-4105-8927-7fad5242664c"), "PLN", "Polski Złoty" });

            migrationBuilder.CreateIndex(
                name: "IX_WalletAccesses_WalletId",
                table: "WalletAccesses",
                column: "WalletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletAccesses");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("b83415ce-6c66-4a2b-acb1-f29429097ad3"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("ebd5787e-ef20-40d1-8867-e1089f197179"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("f6450bd1-8c6f-4105-8927-7fad5242664c"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Wallets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("3729bbe9-6321-453f-8851-9a98b7130479"), "PLN", "Polski Złoty" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("45cc816b-c933-4fa0-be64-fff9542ce98b"), "USD", "United States Dollar" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("805e3f33-8ead-4c42-a9c9-263480c38555"), "EUR", "Euro" });

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Users_UserId",
                table: "Wallets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
