using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EventCitySnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "event_city_snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CollectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasPremiumBuildings = table.Column<bool>(type: "bit", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    PremiumExpansionCount = table.Column<int>(type: "int", nullable: false),
                    WonderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_city_snapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_event_city_snapshots_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_city_snapshot_data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EventCitySnapshotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_city_snapshot_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_event_city_snapshot_data_event_city_snapshots_EventCitySnapshotId",
                        column: x => x.EventCitySnapshotId,
                        principalTable: "event_city_snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_city_snapshot_data_EventCitySnapshotId",
                table: "event_city_snapshot_data",
                column: "EventCitySnapshotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_event_city_snapshots_HasPremiumBuildings",
                table: "event_city_snapshots",
                column: "HasPremiumBuildings");

            migrationBuilder.CreateIndex(
                name: "IX_event_city_snapshots_PlayerId_CityId_WonderId",
                table: "event_city_snapshots",
                columns: new[] { "PlayerId", "CityId", "WonderId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_city_snapshot_data");

            migrationBuilder.DropTable(
                name: "event_city_snapshots");
        }
    }
}
