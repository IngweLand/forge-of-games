using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerLeaderboardIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_players_WorldId_Status_RankingPoints_Rank",
                table: "players",
                columns: new[] { "WorldId", "Status", "RankingPoints", "Rank" },
                descending: new[] { false, false, true, false })
                .Annotation("SqlServer:Include", new[] { "Id", "Age", "AvatarId", "Name", "UpdatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_players_WorldId_Status_RankingPoints_Rank",
                table: "players");
        }
    }
}
