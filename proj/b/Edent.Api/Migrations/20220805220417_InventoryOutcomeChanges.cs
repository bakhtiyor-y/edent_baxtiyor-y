using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edent.Api.Migrations
{
    public partial class InventoryOutcomeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                table: "InventoryOutcomes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryOutcomes_RecipientId",
                table: "InventoryOutcomes",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryOutcomes_AspNetUsers_RecipientId",
                table: "InventoryOutcomes",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryOutcomes_AspNetUsers_RecipientId",
                table: "InventoryOutcomes");

            migrationBuilder.DropIndex(
                name: "IX_InventoryOutcomes_RecipientId",
                table: "InventoryOutcomes");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "InventoryOutcomes");
        }
    }
}
