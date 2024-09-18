using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edent.Api.Migrations
{
    public partial class DentalServiceChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ToothState",
                table: "DentalServices",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToothState",
                table: "DentalServices");
        }
    }
}
