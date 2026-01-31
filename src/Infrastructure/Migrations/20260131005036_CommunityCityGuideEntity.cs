using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CommunityCityGuideEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_community_city_strategies_community_city_strategy_guides_GuideId",
                table: "community_city_strategies");

            migrationBuilder.DropTable(
                name: "community_city_strategy_guides");

            migrationBuilder.DropIndex(
                name: "IX_community_city_strategies_GuideId",
                table: "community_city_strategies");

            migrationBuilder.DropColumn(
                name: "GuideId",
                table: "community_city_strategies");

            migrationBuilder.CreateTable(
                name: "community_city_guides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WonderId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_city_guides", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "community_city_guides");

            migrationBuilder.AddColumn<int>(
                name: "GuideId",
                table: "community_city_strategies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "community_city_strategy_guides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_city_strategy_guides", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_community_city_strategies_GuideId",
                table: "community_city_strategies",
                column: "GuideId",
                unique: true,
                filter: "[GuideId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_community_city_strategies_community_city_strategy_guides_GuideId",
                table: "community_city_strategies",
                column: "GuideId",
                principalTable: "community_city_strategy_guides",
                principalColumn: "Id");
        }
    }
}
