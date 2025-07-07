using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerCitySnapshots2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OpenedExpansionsHash",
                table: "player_city_snapshots",
                type: "char(16)",
                unicode: false,
                fixedLength: true,
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.AddColumn<string>(
                name: "AgeId",
                table: "player_city_snapshots",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_AgeId",
                table: "player_city_snapshots",
                column: "AgeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_AgeId",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "AgeId",
                table: "player_city_snapshots");

            migrationBuilder.AlterColumn<decimal>(
                name: "OpenedExpansionsHash",
                table: "player_city_snapshots",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(16)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 16);
        }
    }
}
