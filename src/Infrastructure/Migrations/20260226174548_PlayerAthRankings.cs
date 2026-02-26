using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerAthRankings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "player_ath_rankings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InGameEventId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_ath_rankings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_player_ath_rankings_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_player_ath_rankings_InGameEventId",
                table: "player_ath_rankings",
                column: "InGameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_player_ath_rankings_PlayerId_InGameEventId",
                table: "player_ath_rankings",
                columns: new[] { "PlayerId", "InGameEventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_ath_rankings_Points",
                table: "player_ath_rankings",
                column: "Points",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_ath_rankings");
        }
    }
}
