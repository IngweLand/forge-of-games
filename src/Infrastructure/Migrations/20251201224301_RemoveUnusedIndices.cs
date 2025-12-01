using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_pvp_rankings2_Tier",
                table: "pvp_rankings2");

            migrationBuilder.DropIndex(
                name: "IX_pvp_battles_WorldId",
                table: "pvp_battles");

            migrationBuilder.DropIndex(
                name: "IX_profile_squads_AbilityLevel",
                table: "profile_squads");

            migrationBuilder.DropIndex(
                name: "IX_profile_squads_AscensionLevel",
                table: "profile_squads");

            migrationBuilder.DropIndex(
                name: "IX_profile_squads_AwakeningLevel",
                table: "profile_squads");

            migrationBuilder.DropIndex(
                name: "IX_players_UpdatedAt",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_player_name_history_entries_Name",
                table: "player_name_history_entries");

            migrationBuilder.DropIndex(
                name: "IX_battles_Difficulty",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_InGameBattleId",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_WorldId",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_WorldId_InGameBattleId",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_alliance_ath_rankings_CollectedAt",
                table: "alliance_ath_rankings");

            migrationBuilder.CreateIndex(
                name: "IX_battles_InGameBattleId",
                table: "battles",
                column: "InGameBattleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_battles_InGameBattleId",
                table: "battles");

            migrationBuilder.CreateIndex(
                name: "IX_pvp_rankings2_Tier",
                table: "pvp_rankings2",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_pvp_battles_WorldId",
                table: "pvp_battles",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_AbilityLevel",
                table: "profile_squads",
                column: "AbilityLevel");

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_AscensionLevel",
                table: "profile_squads",
                column: "AscensionLevel");

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_AwakeningLevel",
                table: "profile_squads",
                column: "AwakeningLevel");

            migrationBuilder.CreateIndex(
                name: "IX_players_UpdatedAt",
                table: "players",
                column: "UpdatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_name_history_entries_Name",
                table: "player_name_history_entries",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_battles_Difficulty",
                table: "battles",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_battles_InGameBattleId",
                table: "battles",
                column: "InGameBattleId");

            migrationBuilder.CreateIndex(
                name: "IX_battles_WorldId",
                table: "battles",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_battles_WorldId_InGameBattleId",
                table: "battles",
                columns: new[] { "WorldId", "InGameBattleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_alliance_ath_rankings_CollectedAt",
                table: "alliance_ath_rankings",
                column: "CollectedAt",
                descending: new bool[0]);
        }
    }
}
