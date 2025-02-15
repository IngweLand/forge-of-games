using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    InGamePlayerId = table.Column<int>(type: "int", nullable: false),
                    WorldId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    Age = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AllianceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AvatarId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    RankingPoints = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => new { x.WorldId, x.InGamePlayerId });
                });

            migrationBuilder.CreateTable(
                name: "player_rankings",
                columns: table => new
                {
                    CollectedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    InGamePlayerId = table.Column<int>(type: "int", nullable: false),
                    WorldId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    Age = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AllianceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_rankings", x => new { x.WorldId, x.InGamePlayerId, x.CollectedAt });
                    table.ForeignKey(
                        name: "FK_player_rankings_players_WorldId_InGamePlayerId",
                        columns: x => new { x.WorldId, x.InGamePlayerId },
                        principalTable: "players",
                        principalColumns: new[] { "WorldId", "InGamePlayerId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_player_rankings_CollectedAt",
                table: "player_rankings",
                column: "CollectedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_rankings_WorldId",
                table: "player_rankings",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_player_rankings_WorldId_InGamePlayerId",
                table: "player_rankings",
                columns: new[] { "WorldId", "InGamePlayerId" });

            migrationBuilder.CreateIndex(
                name: "IX_players_Age",
                table: "players",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_players_InGamePlayerId",
                table: "players",
                column: "InGamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_players_Name",
                table: "players",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_players_RankingPoints",
                table: "players",
                column: "RankingPoints",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_players_WorldId",
                table: "players",
                column: "WorldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_rankings");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
