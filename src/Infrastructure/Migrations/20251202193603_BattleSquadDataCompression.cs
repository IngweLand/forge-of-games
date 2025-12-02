using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BattleSquadDataCompression : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Squads",
                table: "battle_squads");

            migrationBuilder.RenameColumn(
                name: "TempSquads",
                table: "battle_squads",
                newName: "Squads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Squads",
                table: "battle_squads",
                newName: "TempSquads");

            
            migrationBuilder.AddColumn<string>(
                name: "Squads",
                table: "battle_squads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
