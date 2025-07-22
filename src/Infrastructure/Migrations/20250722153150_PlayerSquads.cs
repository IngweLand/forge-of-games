using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerSquads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "profile_squads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AbilityLevel = table.Column<int>(type: "int", nullable: false),
                    AscensionLevel = table.Column<int>(type: "int", nullable: false),
                    CollectedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    SerializedHero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerializedSupportUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_squads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_profile_squads_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_CollectedAt",
                table: "profile_squads",
                column: "CollectedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_PlayerId_UnitId_CollectedAt",
                table: "profile_squads",
                columns: new[] { "PlayerId", "UnitId", "CollectedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_UnitId",
                table: "profile_squads",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profile_squads");
        }
    }
}
