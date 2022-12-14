using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionLabelTableCheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dictionary<string, Guid>_Labels_LabelId_LabelWalletId",
                table: "Dictionary<string, Guid>");

            migrationBuilder.DropForeignKey(
                name: "FK_Dictionary<string, Guid>_Transactions_TransactionId_Transac~",
                table: "Dictionary<string, Guid>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dictionary<string, Guid>",
                table: "Dictionary<string, Guid>");

            migrationBuilder.RenameTable(
                name: "Dictionary<string, Guid>",
                newName: "TransactionLabel");

            migrationBuilder.RenameIndex(
                name: "IX_Dictionary<string, Guid>_TransactionId_TransactionWalletId",
                table: "TransactionLabel",
                newName: "IX_TransactionLabel_TransactionId_TransactionWalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionLabel",
                table: "TransactionLabel",
                columns: new[] { "LabelId", "LabelWalletId", "TransactionId", "TransactionWalletId" });

            migrationBuilder.AddCheckConstraint(
                name: "CH_TransactionLabel_TransactionWalletId_LabelWalletId",
                table: "TransactionLabel",
                sql: "\"TransactionWalletId\" = \"LabelWalletId\"");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLabel_Labels_LabelId_LabelWalletId",
                table: "TransactionLabel",
                columns: new[] { "LabelId", "LabelWalletId" },
                principalTable: "Labels",
                principalColumns: new[] { "Id", "WalletId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLabel_Transactions_TransactionId_TransactionWall~",
                table: "TransactionLabel",
                columns: new[] { "TransactionId", "TransactionWalletId" },
                principalTable: "Transactions",
                principalColumns: new[] { "Id", "WalletId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLabel_Labels_LabelId_LabelWalletId",
                table: "TransactionLabel");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLabel_Transactions_TransactionId_TransactionWall~",
                table: "TransactionLabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionLabel",
                table: "TransactionLabel");

            migrationBuilder.DropCheckConstraint(
                name: "CH_TransactionLabel_TransactionWalletId_LabelWalletId",
                table: "TransactionLabel");

            migrationBuilder.RenameTable(
                name: "TransactionLabel",
                newName: "Dictionary<string, Guid>");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionLabel_TransactionId_TransactionWalletId",
                table: "Dictionary<string, Guid>",
                newName: "IX_Dictionary<string, Guid>_TransactionId_TransactionWalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dictionary<string, Guid>",
                table: "Dictionary<string, Guid>",
                columns: new[] { "LabelId", "LabelWalletId", "TransactionId", "TransactionWalletId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Dictionary<string, Guid>_Labels_LabelId_LabelWalletId",
                table: "Dictionary<string, Guid>",
                columns: new[] { "LabelId", "LabelWalletId" },
                principalTable: "Labels",
                principalColumns: new[] { "Id", "WalletId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dictionary<string, Guid>_Transactions_TransactionId_Transac~",
                table: "Dictionary<string, Guid>",
                columns: new[] { "TransactionId", "TransactionWalletId" },
                principalTable: "Transactions",
                principalColumns: new[] { "Id", "WalletId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
