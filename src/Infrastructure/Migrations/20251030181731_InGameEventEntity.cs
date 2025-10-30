using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InGameEventEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "in_game_events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefinitionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorldId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_in_game_events", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_in_game_events_DefinitionId",
                table: "in_game_events",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_in_game_events_EventId",
                table: "in_game_events",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_in_game_events_WorldId",
                table: "in_game_events",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_in_game_events_WorldId_DefinitionId_EventId",
                table: "in_game_events",
                columns: new[] { "WorldId", "DefinitionId", "EventId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "in_game_events");
        }
    }
}
