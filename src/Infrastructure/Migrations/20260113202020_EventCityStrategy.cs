using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EventCityStrategy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InGameEventId",
                table: "event_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "event_city_strategy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasPremiumBuildings = table.Column<bool>(type: "bit", nullable: false),
                    HasPremiumExpansion = table.Column<bool>(type: "bit", nullable: false),
                    InGameEventId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    WonderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_city_strategy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_event_city_strategy_in_game_events_InGameEventId",
                        column: x => x.InGameEventId,
                        principalTable: "in_game_events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_event_city_strategy_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_city_strategy_data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EventCityStrategyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_city_strategy_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_event_city_strategy_data_event_city_strategy_EventCityStrategyId",
                        column: x => x.EventCityStrategyId,
                        principalTable: "event_city_strategy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_city_snapshots_InGameEventId",
                table: "event_city_snapshots",
                column: "InGameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_event_city_strategy_CityId_WonderId",
                table: "event_city_strategy",
                columns: new[] { "CityId", "WonderId" });

            migrationBuilder.CreateIndex(
                name: "IX_event_city_strategy_HasPremiumBuildings",
                table: "event_city_strategy",
                column: "HasPremiumBuildings");

            migrationBuilder.CreateIndex(
                name: "IX_event_city_strategy_HasPremiumExpansion",
                table: "event_city_strategy",
                column: "HasPremiumExpansion");

            migrationBuilder.CreateIndex(
                name: "IX_event_city_strategy_InGameEventId",
                table: "event_city_strategy",
                column: "InGameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_event_city_strategy_PlayerId",
                table: "event_city_strategy",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_event_city_strategy_data_EventCityStrategyId",
                table: "event_city_strategy_data",
                column: "EventCityStrategyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_event_city_snapshots_in_game_events_InGameEventId",
                table: "event_city_snapshots",
                column: "InGameEventId",
                principalTable: "in_game_events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_event_city_snapshots_in_game_events_InGameEventId",
                table: "event_city_snapshots");

            migrationBuilder.DropTable(
                name: "event_city_strategy_data");

            migrationBuilder.DropTable(
                name: "event_city_strategy");

            migrationBuilder.DropIndex(
                name: "IX_event_city_snapshots_InGameEventId",
                table: "event_city_snapshots");

            migrationBuilder.DropColumn(
                name: "InGameEventId",
                table: "event_city_snapshots");
        }
    }
}
