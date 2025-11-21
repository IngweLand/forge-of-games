using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PvpRankings2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PvpTier",
                table: "players");

            migrationBuilder.CreateTable(
                name: "pvp_rankings2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pvp_rankings2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pvp_rankings2_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pvp_rankings2_CollectedAt",
                table: "pvp_rankings2",
                column: "CollectedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_pvp_rankings2_PlayerId",
                table: "pvp_rankings2",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_pvp_rankings2_Tier",
                table: "pvp_rankings2",
                column: "Tier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pvp_rankings2");

            migrationBuilder.AddColumn<string>(
                name: "PvpTier",
                table: "players",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
