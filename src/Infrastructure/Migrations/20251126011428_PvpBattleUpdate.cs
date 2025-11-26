using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PvpBattleUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoserUnits",
                table: "pvp_battles");

            migrationBuilder.DropColumn(
                name: "WinnerUnits",
                table: "pvp_battles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoserUnits",
                table: "pvp_battles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WinnerUnits",
                table: "pvp_battles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
