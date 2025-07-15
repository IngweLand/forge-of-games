using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerCitySnapshotNewProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "HappinessUsageRatio",
                table: "player_city_snapshots",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "TotalArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_TotalArea",
                table: "player_city_snapshots",
                column: "TotalArea");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_TotalArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "HappinessUsageRatio",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "TotalArea",
                table: "player_city_snapshots");
        }
    }
}
