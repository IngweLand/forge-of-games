using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntermediateCompressionProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TempLoserTeam",
                table: "pvp_battle_teams",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TempWinnerTeam",
                table: "pvp_battle_teams",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TempSerializedHero",
                table: "profile_squad_data",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TempSerializedSupportUnit",
                table: "profile_squad_data",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TempEntries",
                table: "battle_timelines",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempLoserTeam",
                table: "pvp_battle_teams");

            migrationBuilder.DropColumn(
                name: "TempWinnerTeam",
                table: "pvp_battle_teams");

            migrationBuilder.DropColumn(
                name: "TempSerializedHero",
                table: "profile_squad_data");

            migrationBuilder.DropColumn(
                name: "TempSerializedSupportUnit",
                table: "profile_squad_data");

            migrationBuilder.DropColumn(
                name: "TempEntries",
                table: "battle_timelines");
        }
    }
}
