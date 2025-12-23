using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WonderRanking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "event_city_wonder_rankings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    CollectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InGameEventId = table.Column<int>(type: "int", nullable: false),
                    WonderLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_city_wonder_rankings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_event_city_wonder_rankings_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_city_wonder_rankings_InGameEventId",
                table: "event_city_wonder_rankings",
                column: "InGameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_event_city_wonder_rankings_PlayerId_InGameEventId",
                table: "event_city_wonder_rankings",
                columns: new[] { "PlayerId", "InGameEventId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_city_wonder_rankings");
        }
    }
}
