using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignEntitiesWalletIdKeysInTransactionCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CH__WalletId_CategoryWalletId",
                table: "Transactions",
                sql: "\"CategoryWalletId\" is null or \"CategoryWalletId\" = \"WalletId\"");

            migrationBuilder.AddCheckConstraint(
                name: "CH__WalletId_RecipientWalletId",
                table: "Transactions",
                sql: "\"RecipientWalletId\" is null or \"RecipientWalletId\" = \"WalletId\"");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CH__WalletId_CategoryWalletId",
                table: "Transactions");

            migrationBuilder.DropCheckConstraint(
                name: "CH__WalletId_RecipientWalletId",
                table: "Transactions");
        }
    }
}
