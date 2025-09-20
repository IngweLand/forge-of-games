using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProfileSquadCleanUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedHero",
                table: "profile_squads");

            migrationBuilder.DropColumn(
                name: "SerializedSupportUnit",
                table: "profile_squads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerializedHero",
                table: "profile_squads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SerializedSupportUnit",
                table: "profile_squads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
