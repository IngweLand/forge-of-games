using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AllianceBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvatarIconId",
                table: "alliances",
                newName: "BannerIconId");

            migrationBuilder.RenameColumn(
                name: "AvatarBackgroundId",
                table: "alliances",
                newName: "BannerCrestId");

            migrationBuilder.AddColumn<int>(
                name: "BannerCrestColorId",
                table: "alliances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BannerIconColorId",
                table: "alliances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerCrestColorId",
                table: "alliances");

            migrationBuilder.DropColumn(
                name: "BannerIconColorId",
                table: "alliances");

            migrationBuilder.RenameColumn(
                name: "BannerIconId",
                table: "alliances",
                newName: "AvatarIconId");

            migrationBuilder.RenameColumn(
                name: "BannerCrestId",
                table: "alliances",
                newName: "AvatarBackgroundId");
        }
    }
}
