using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AwakeningLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwakeningLevel",
                table: "profile_squads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_AwakeningLevel",
                table: "profile_squads",
                column: "AwakeningLevel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_profile_squads_AwakeningLevel",
                table: "profile_squads");

            migrationBuilder.DropColumn(
                name: "AwakeningLevel",
                table: "profile_squads");
        }
    }
}
