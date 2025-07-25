using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlayerAllianceNameHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_alliance_name_history_entries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "player_alliance_name_history_entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllianceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_alliance_name_history_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_player_alliance_name_history_entries_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_player_alliance_name_history_entries_AllianceName",
                table: "player_alliance_name_history_entries",
                column: "AllianceName");

            migrationBuilder.CreateIndex(
                name: "IX_player_alliance_name_history_entries_PlayerId_AllianceName",
                table: "player_alliance_name_history_entries",
                columns: new[] { "PlayerId", "AllianceName" });
        }
    }
}
