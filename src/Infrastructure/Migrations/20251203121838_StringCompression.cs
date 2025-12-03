using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StringCompression : Migration
    {
       /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entries",
                table: "battle_timelines");
            migrationBuilder.RenameColumn(
                name: "TempEntries",
                table: "battle_timelines",
                newName: "Entries");
				
				 migrationBuilder.DropColumn(
                name: "SerializedHero",
                table: "profile_squad_data");
            migrationBuilder.RenameColumn(
                name: "TempSerializedHero",
                table: "profile_squad_data",
                newName: "SerializedHero");
				
					 migrationBuilder.DropColumn(
                name: "SerializedSupportUnit",
                table: "profile_squad_data");
            migrationBuilder.RenameColumn(
                name: "TempSerializedSupportUnit",
                table: "profile_squad_data",
                newName: "SerializedSupportUnit");
				
				migrationBuilder.DropColumn(
                name: "WinnerTeam",
                table: "pvp_battle_teams");
            migrationBuilder.RenameColumn(
                name: "TempWinnerTeam",
                table: "pvp_battle_teams",
                newName: "WinnerTeam");
				
					migrationBuilder.DropColumn(
                name: "LoserTeam",
                table: "pvp_battle_teams");
            migrationBuilder.RenameColumn(
                name: "TempLoserTeam",
                table: "pvp_battle_teams",
                newName: "LoserTeam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Entries",
                table: "battle_timelines",
                newName: "TempEntries");
			migrationBuilder.AddColumn<string>(
                name: "Entries",
                table: "battle_timelines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
				
				migrationBuilder.RenameColumn(
                name: "SerializedHero",
                table: "profile_squad_data",
                newName: "TempSerializedHero");
			migrationBuilder.AddColumn<string>(
                name: "SerializedHero",
                table: "profile_squad_data",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
				
					migrationBuilder.RenameColumn(
                name: "SerializedSupportUnit",
                table: "profile_squad_data",
                newName: "TempSerializedSupportUnit");
			migrationBuilder.AddColumn<string>(
                name: "SerializedSupportUnit",
                table: "profile_squad_data",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
				
					migrationBuilder.RenameColumn(
                name: "WinnerTeam",
                table: "pvp_battle_teams",
                newName: "TempWinnerTeam");
			migrationBuilder.AddColumn<string>(
                name: "WinnerTeam",
                table: "pvp_battle_teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
				
					migrationBuilder.RenameColumn(
                name: "LoserTeam",
                table: "pvp_battle_teams",
                newName: "TempLoserTeam");
			migrationBuilder.AddColumn<string>(
                name: "LoserTeam",
                table: "pvp_battle_teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
