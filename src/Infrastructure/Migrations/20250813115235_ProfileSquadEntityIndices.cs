using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProfileSquadEntityIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_AbilityLevel",
                table: "profile_squads",
                column: "AbilityLevel");

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_AscensionLevel",
                table: "profile_squads",
                column: "AscensionLevel");

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_Level",
                table: "profile_squads",
                column: "Level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_profile_squads_AbilityLevel",
                table: "profile_squads");

            migrationBuilder.DropIndex(
                name: "IX_profile_squads_AscensionLevel",
                table: "profile_squads");

            migrationBuilder.DropIndex(
                name: "IX_profile_squads_Level",
                table: "profile_squads");
        }
    }
}
