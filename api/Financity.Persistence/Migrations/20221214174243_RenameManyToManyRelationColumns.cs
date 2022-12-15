using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameManyToManyRelationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetCategory_Budgets_BudgetsId",
                table: "BudgetCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetCategory_Categories_TrackedCategoriesId",
                table: "BudgetCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWallet_Users_UsersWithSharedAccessId",
                table: "UserWallet");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWallet_Wallets_SharedWalletsId",
                table: "UserWallet");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UsersWithSharedAccessId",
                table: "UserWallet",
                newName: "WalletId");

            migrationBuilder.RenameColumn(
                name: "SharedWalletsId",
                table: "UserWallet",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWallet_UsersWithSharedAccessId",
                table: "UserWallet",
                newName: "IX_UserWallet_WalletId");

            migrationBuilder.RenameColumn(
                name: "TrackedCategoriesId",
                table: "BudgetCategory",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "BudgetsId",
                table: "BudgetCategory",
                newName: "BudgetId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetCategory_TrackedCategoriesId",
                table: "BudgetCategory",
                newName: "IX_BudgetCategory_CategoryId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCategory_Budgets_BudgetId",
                table: "BudgetCategory",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCategory_Categories_CategoryId",
                table: "BudgetCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWallet_Users_UserId",
                table: "UserWallet",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWallet_Wallets_WalletId",
                table: "UserWallet",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetCategory_Budgets_BudgetId",
                table: "BudgetCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetCategory_Categories_CategoryId",
                table: "BudgetCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWallet_Users_UserId",
                table: "UserWallet");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWallet_Wallets_WalletId",
                table: "UserWallet");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "UserWallet",
                newName: "UsersWithSharedAccessId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserWallet",
                newName: "SharedWalletsId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWallet_WalletId",
                table: "UserWallet",
                newName: "IX_UserWallet_UsersWithSharedAccessId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "BudgetCategory",
                newName: "TrackedCategoriesId");

            migrationBuilder.RenameColumn(
                name: "BudgetId",
                table: "BudgetCategory",
                newName: "BudgetsId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetCategory_CategoryId",
                table: "BudgetCategory",
                newName: "IX_BudgetCategory_TrackedCategoriesId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCategory_Budgets_BudgetsId",
                table: "BudgetCategory",
                column: "BudgetsId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCategory_Categories_TrackedCategoriesId",
                table: "BudgetCategory",
                column: "TrackedCategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWallet_Users_UsersWithSharedAccessId",
                table: "UserWallet",
                column: "UsersWithSharedAccessId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWallet_Wallets_SharedWalletsId",
                table: "UserWallet",
                column: "SharedWalletsId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
