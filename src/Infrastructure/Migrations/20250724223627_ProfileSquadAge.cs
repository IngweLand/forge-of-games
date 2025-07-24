using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProfileSquadAge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Age",
                table: "profile_squads",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_profile_squads_Age",
                table: "profile_squads",
                column: "Age");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_profile_squads_Age",
                table: "profile_squads");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "profile_squads");
        }
    }
}
