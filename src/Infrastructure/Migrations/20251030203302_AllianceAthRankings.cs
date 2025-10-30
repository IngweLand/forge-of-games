using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AllianceAthRankings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alliance_ath_rankings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    CollectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InGameEventId = table.Column<int>(type: "int", nullable: false),
                    League = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alliance_ath_rankings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_alliance_ath_rankings_alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalTable: "alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alliance_ath_rankings_AllianceId_InGameEventId",
                table: "alliance_ath_rankings",
                columns: new[] { "AllianceId", "InGameEventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_alliance_ath_rankings_CollectedAt",
                table: "alliance_ath_rankings",
                column: "CollectedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_alliance_ath_rankings_InGameEventId",
                table: "alliance_ath_rankings",
                column: "InGameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_alliance_ath_rankings_League",
                table: "alliance_ath_rankings",
                column: "League");

            migrationBuilder.CreateIndex(
                name: "IX_alliance_ath_rankings_Points",
                table: "alliance_ath_rankings",
                column: "Points",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alliance_ath_rankings");
        }
    }
}
