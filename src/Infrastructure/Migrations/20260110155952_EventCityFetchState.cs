using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EventCityFetchState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "event_city_fetch_states",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    FailuresCount = table.Column<int>(type: "int", nullable: false),
                    FetchTimestamps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameWorldId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InGamePlayerId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_city_fetch_states", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_city_fetch_states_EventId",
                table: "event_city_fetch_states",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_event_city_fetch_states_EventId_PlayerId",
                table: "event_city_fetch_states",
                columns: new[] { "EventId", "PlayerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_city_fetch_states");
        }
    }
}
