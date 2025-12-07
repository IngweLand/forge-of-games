using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerCitySnapshotAdditionalStats2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_HasPremiumBuildings",
                table: "player_city_snapshots");
            
            migrationBuilder.DropColumn(
                name: "HasPremiumBuildings",
                table: "player_city_snapshots");
            
            migrationBuilder.AddColumn<bool>(
                name: "HasPremiumHomeBuildings",
                table: "player_city_snapshots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPremiumCultureBuildings",
                table: "player_city_snapshots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPremiumFarmBuildings",
                table: "player_city_snapshots",
                type: "bit",
                nullable: false,
                defaultValue: false);
            
            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_HasPremiumHomeBuildings",
                table: "player_city_snapshots",
                column: "HasPremiumHomeBuildings");

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_HasPremiumCultureBuildings",
                table: "player_city_snapshots",
                column: "HasPremiumCultureBuildings");

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_HasPremiumFarmBuildings",
                table: "player_city_snapshots",
                column: "HasPremiumFarmBuildings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_HasPremiumHomeBuildings",
                table: "player_city_snapshots");
            
            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_HasPremiumCultureBuildings",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_HasPremiumFarmBuildings",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "HasPremiumHomeBuildings",
                table: "player_city_snapshots");
            
            migrationBuilder.DropColumn(
                name: "HasPremiumCultureBuildings",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "HasPremiumFarmBuildings",
                table: "player_city_snapshots");

            migrationBuilder.AddColumn<bool>(
                name: "HasPremiumBuildings",
                table: "player_city_snapshots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_HasPremiumBuildings",
                table: "player_city_snapshots",
                column: "HasPremiumBuildings");
        }
    }
}
