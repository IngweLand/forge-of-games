using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alliances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alliances",
                columns: table => new
                {
                    InGameAllianceId = table.Column<int>(type: "int", nullable: false),
                    WorldId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    AvatarBackgroundId = table.Column<int>(type: "int", nullable: false),
                    AvatarIconId = table.Column<int>(type: "int", nullable: false),
                    MemberCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    RankingPoints = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alliances", x => new { x.WorldId, x.InGameAllianceId });
                });

            migrationBuilder.CreateTable(
                name: "alliance_rankings",
                columns: table => new
                {
                    CollectedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    InGameAllianceId = table.Column<int>(type: "int", nullable: false),
                    WorldId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    MemberCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alliance_rankings", x => new { x.WorldId, x.InGameAllianceId, x.CollectedAt });
                    table.ForeignKey(
                        name: "FK_alliance_rankings_alliances_WorldId_InGameAllianceId",
                        columns: x => new { x.WorldId, x.InGameAllianceId },
                        principalTable: "alliances",
                        principalColumns: new[] { "WorldId", "InGameAllianceId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alliance_rankings_CollectedAt",
                table: "alliance_rankings",
                column: "CollectedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_alliance_rankings_WorldId",
                table: "alliance_rankings",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_alliance_rankings_WorldId_InGameAllianceId",
                table: "alliance_rankings",
                columns: new[] { "WorldId", "InGameAllianceId" });

            migrationBuilder.CreateIndex(
                name: "IX_alliances_InGameAllianceId",
                table: "alliances",
                column: "InGameAllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_alliances_Name",
                table: "alliances",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_alliances_RankingPoints",
                table: "alliances",
                column: "RankingPoints",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_alliances_WorldId",
                table: "alliances",
                column: "WorldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alliance_rankings");

            migrationBuilder.DropTable(
                name: "alliances");
        }
    }
}
