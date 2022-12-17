using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignEntitiesWalletIdKeysInTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Recipients_RecipientId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_OwnerId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RecipientId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Recipients_WalletId",
                table: "Recipients");

            migrationBuilder.DropIndex(
                name: "IX_Labels_WalletId",
                table: "Labels");

            migrationBuilder.DropIndex(
                name: "IX_Categories_WalletId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_UserId",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "Transactions",
                newName: "RecipientWalletId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Transactions",
                newName: "CategoryWalletId");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "Wallets",
                type: "character varying(16)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "Transactions",
                type: "character varying(16)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Transactions",
                type: "character varying(64)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "Transactions",
                type: "character varying(64)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Recipients",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Currencies",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Currencies",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "Budgets",
                type: "character varying(16)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Recipients_WalletId_Name",
                table: "Recipients",
                columns: new[] { "WalletId", "Name" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Categories_WalletId_Name",
                table: "Categories",
                columns: new[] { "WalletId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_OwnerId_Name",
                table: "Wallets",
                columns: new[] { "OwnerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryWalletId_CategoryName",
                table: "Transactions",
                columns: new[] { "CategoryWalletId", "CategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecipientWalletId_RecipientName",
                table: "Transactions",
                columns: new[] { "RecipientWalletId", "RecipientName" });

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_WalletId_Name",
                table: "Recipients",
                columns: new[] { "WalletId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Labels_WalletId_Name",
                table: "Labels",
                columns: new[] { "WalletId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_WalletId_Name",
                table: "Categories",
                columns: new[] { "WalletId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserId_Name",
                table: "Budgets",
                columns: new[] { "UserId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryWalletId_CategoryName",
                table: "Transactions",
                columns: new[] { "CategoryWalletId", "CategoryName" },
                principalTable: "Categories",
                principalColumns: new[] { "WalletId", "Name" },
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Recipients_RecipientWalletId_RecipientName",
                table: "Transactions",
                columns: new[] { "RecipientWalletId", "RecipientName" },
                principalTable: "Recipients",
                principalColumns: new[] { "WalletId", "Name" },
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryWalletId_CategoryName",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Recipients_RecipientWalletId_RecipientName",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_OwnerId_Name",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryWalletId_CategoryName",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RecipientWalletId_RecipientName",
                table: "Transactions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Recipients_WalletId_Name",
                table: "Recipients");

            migrationBuilder.DropIndex(
                name: "IX_Recipients_WalletId_Name",
                table: "Recipients");

            migrationBuilder.DropIndex(
                name: "IX_Labels_WalletId_Name",
                table: "Labels");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Categories_WalletId_Name",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_WalletId_Name",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_UserId_Name",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "RecipientWalletId",
                table: "Transactions",
                newName: "RecipientId");

            migrationBuilder.RenameColumn(
                name: "CategoryWalletId",
                table: "Transactions",
                newName: "CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "Wallets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(16)");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "Transactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(16)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Recipients",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Currencies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Currencies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "Budgets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(16)");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_OwnerId",
                table: "Wallets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecipientId",
                table: "Transactions",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_WalletId",
                table: "Recipients",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_WalletId",
                table: "Labels",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_WalletId",
                table: "Categories",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserId",
                table: "Budgets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Recipients_RecipientId",
                table: "Transactions",
                column: "RecipientId",
                principalTable: "Recipients",
                principalColumn: "Id");
        }
    }
}
