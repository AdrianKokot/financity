using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Financity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixBudgetCategoriesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Budgets_BudgetId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_BudgetId",
                table: "Categories");

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

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Appearance_IconName",
                table: "Labels",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Appearance_Color",
                table: "Labels",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Appearance_IconName",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Appearance_Color",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "BudgetCategory",
                columns: table => new
                {
                    BudgetsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrackedCategoriesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCategory", x => new { x.BudgetsId, x.TrackedCategoriesId });
                    table.ForeignKey(
                        name: "FK_BudgetCategory_Budgets_BudgetsId",
                        column: x => x.BudgetsId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetCategory_Categories_TrackedCategoriesId",
                        column: x => x.TrackedCategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("4773a5ae-eb68-469f-8afc-d6cda74b50a1"), "PLN", "Polski Złoty" },
                    { new Guid("66f001bf-6ef5-4f2e-894a-d0f56c830368"), "EUR", "Euro" },
                    { new Guid("ee9f493b-cf8e-45d1-b48e-d4e2905c7d2f"), "USD", "United States Dollar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCategory_TrackedCategoriesId",
                table: "BudgetCategory",
                column: "TrackedCategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetCategory");

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

            migrationBuilder.AlterColumn<string>(
                name: "Appearance_IconName",
                table: "Labels",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Appearance_Color",
                table: "Labels",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Appearance_IconName",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Appearance_Color",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetId",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("4f50a462-2c9c-42ed-8d0a-4de33b3997de"), "EUR", "Euro" },
                    { new Guid("5e630489-4965-4314-aabd-6209972eae98"), "PLN", "Polski Złoty" },
                    { new Guid("a3b3b070-6a59-4faf-93bf-c9e515393cdd"), "USD", "United States Dollar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BudgetId",
                table: "Categories",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Budgets_BudgetId",
                table: "Categories",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id");
        }
    }
}
