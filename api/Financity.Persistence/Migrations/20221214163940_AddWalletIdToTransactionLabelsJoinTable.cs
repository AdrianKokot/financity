using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletIdToTransactionLabelsJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryWalletId_CategoryName",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Recipients_RecipientWalletId_RecipientName",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "LabelTransaction");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryWalletId_CategoryName",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RecipientWalletId_RecipientName",
                table: "Transactions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Recipients_WalletId_Name",
                table: "Recipients");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Categories_WalletId_Name",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Transactions");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecipientId",
                table: "Transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Transactions_Id_WalletId",
                table: "Transactions",
                columns: new[] { "Id", "WalletId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Recipients_Id_WalletId",
                table: "Recipients",
                columns: new[] { "Id", "WalletId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Labels_Id_WalletId",
                table: "Labels",
                columns: new[] { "Id", "WalletId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Categories_Id_WalletId",
                table: "Categories",
                columns: new[] { "Id", "WalletId" });

            migrationBuilder.CreateTable(
                name: "Dictionary<string, Guid>",
                columns: table => new
                {
                    LabelId = table.Column<Guid>(type: "uuid", nullable: false),
                    LabelWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionWalletId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dictionary<string, Guid>", x => new { x.LabelId, x.LabelWalletId, x.TransactionId, x.TransactionWalletId });
                    table.ForeignKey(
                        name: "FK_Dictionary<string, Guid>_Labels_LabelId_LabelWalletId",
                        columns: x => new { x.LabelId, x.LabelWalletId },
                        principalTable: "Labels",
                        principalColumns: new[] { "Id", "WalletId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dictionary<string, Guid>_Transactions_TransactionId_Transac~",
                        columns: x => new { x.TransactionId, x.TransactionWalletId },
                        principalTable: "Transactions",
                        principalColumns: new[] { "Id", "WalletId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId_CategoryWalletId",
                table: "Transactions",
                columns: new[] { "CategoryId", "CategoryWalletId" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecipientId_RecipientWalletId",
                table: "Transactions",
                columns: new[] { "RecipientId", "RecipientWalletId" });

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary<string, Guid>_TransactionId_TransactionWalletId",
                table: "Dictionary<string, Guid>",
                columns: new[] { "TransactionId", "TransactionWalletId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId_CategoryWalletId",
                table: "Transactions",
                columns: new[] { "CategoryId", "CategoryWalletId" },
                principalTable: "Categories",
                principalColumns: new[] { "Id", "WalletId" },
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Recipients_RecipientId_RecipientWalletId",
                table: "Transactions",
                columns: new[] { "RecipientId", "RecipientWalletId" },
                principalTable: "Recipients",
                principalColumns: new[] { "Id", "WalletId" },
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId_CategoryWalletId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Recipients_RecipientId_RecipientWalletId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Dictionary<string, Guid>");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Transactions_Id_WalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryId_CategoryWalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RecipientId_RecipientWalletId",
                table: "Transactions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Recipients_Id_WalletId",
                table: "Recipients");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Labels_Id_WalletId",
                table: "Labels");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Categories_Id_WalletId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Transactions");

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

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Recipients_WalletId_Name",
                table: "Recipients",
                columns: new[] { "WalletId", "Name" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Categories_WalletId_Name",
                table: "Categories",
                columns: new[] { "WalletId", "Name" });

            migrationBuilder.CreateTable(
                name: "LabelTransaction",
                columns: table => new
                {
                    LabelsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelTransaction", x => new { x.LabelsId, x.TransactionsId });
                    table.ForeignKey(
                        name: "FK_LabelTransaction_Labels_LabelsId",
                        column: x => x.LabelsId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelTransaction_Transactions_TransactionsId",
                        column: x => x.TransactionsId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryWalletId_CategoryName",
                table: "Transactions",
                columns: new[] { "CategoryWalletId", "CategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecipientWalletId_RecipientName",
                table: "Transactions",
                columns: new[] { "RecipientWalletId", "RecipientName" });

            migrationBuilder.CreateIndex(
                name: "IX_LabelTransaction_TransactionsId",
                table: "LabelTransaction",
                column: "TransactionsId");

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
    }
}
