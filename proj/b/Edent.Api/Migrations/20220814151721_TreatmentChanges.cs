using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edent.Api.Migrations
{
    public partial class TreatmentChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DentalServiceId",
                table: "Treatments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DentalServiceId",
                table: "Treatments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
