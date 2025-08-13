using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TopHeroInsights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "top_hero_insights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    FromLevel = table.Column<int>(type: "int", nullable: true),
                    Heroes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToLevel = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_top_hero_insights", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_top_hero_insights_AgeId",
                table: "top_hero_insights",
                column: "AgeId");

            migrationBuilder.CreateIndex(
                name: "IX_top_hero_insights_CreatedAt",
                table: "top_hero_insights",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_top_hero_insights_FromLevel",
                table: "top_hero_insights",
                column: "FromLevel");

            migrationBuilder.CreateIndex(
                name: "IX_top_hero_insights_Mode",
                table: "top_hero_insights",
                column: "Mode");

            migrationBuilder.CreateIndex(
                name: "IX_top_hero_insights_ToLevel",
                table: "top_hero_insights",
                column: "ToLevel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "top_hero_insights");
        }
    }
}
