using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edent.Api.Migrations
{
    public partial class ReceptChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceptDentalService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DentalServiceId = table.Column<int>(type: "integer", nullable: false),
                    ReceptId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptDentalService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceptDentalService_DentalServices_DentalServiceId",
                        column: x => x.DentalServiceId,
                        principalTable: "DentalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceptDentalService_Recepts_ReceptId",
                        column: x => x.ReceptId,
                        principalTable: "Recepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceptDentalService_DentalServiceId",
                table: "ReceptDentalService",
                column: "DentalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptDentalService_ReceptId",
                table: "ReceptDentalService",
                column: "ReceptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceptDentalService");
        }
    }
}
