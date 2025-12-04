using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RankingIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_player_rankings_PlayerId",
                table: "player_rankings");

            migrationBuilder.DropIndex(
                name: "IX_alliance_rankings_AllianceId",
                table: "alliance_rankings");

            migrationBuilder.CreateIndex(
                name: "IX_player_rankings_PlayerId_Type_CollectedAt",
                table: "player_rankings",
                columns: new[] { "PlayerId", "Type", "CollectedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_player_rankings_Type_CollectedAt",
                table: "player_rankings",
                columns: new[] { "Type", "CollectedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_alliance_rankings_AllianceId_Type_CollectedAt",
                table: "alliance_rankings",
                columns: new[] { "AllianceId", "Type", "CollectedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_alliance_rankings_Type_CollectedAt",
                table: "alliance_rankings",
                columns: new[] { "Type", "CollectedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_player_rankings_PlayerId_Type_CollectedAt",
                table: "player_rankings");

            migrationBuilder.DropIndex(
                name: "IX_player_rankings_Type_CollectedAt",
                table: "player_rankings");

            migrationBuilder.DropIndex(
                name: "IX_alliance_rankings_AllianceId_Type_CollectedAt",
                table: "alliance_rankings");

            migrationBuilder.DropIndex(
                name: "IX_alliance_rankings_Type_CollectedAt",
                table: "alliance_rankings");

            migrationBuilder.CreateIndex(
                name: "IX_player_rankings_PlayerId",
                table: "player_rankings",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_alliance_rankings_AllianceId",
                table: "alliance_rankings",
                column: "AllianceId");
        }
    }
}
