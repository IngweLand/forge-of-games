using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelicInsights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "relic_insights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromLevel = table.Column<int>(type: "int", nullable: false),
                    Relics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToLevel = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relic_insights", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_relic_insights_FromLevel",
                table: "relic_insights",
                column: "FromLevel");

            migrationBuilder.CreateIndex(
                name: "IX_relic_insights_ToLevel",
                table: "relic_insights",
                column: "ToLevel");

            migrationBuilder.CreateIndex(
                name: "IX_relic_insights_UnitId",
                table: "relic_insights",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_relic_insights_UnitId_FromLevel_ToLevel",
                table: "relic_insights",
                columns: new[] { "UnitId", "FromLevel", "ToLevel" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "relic_insights");
        }
    }
}
