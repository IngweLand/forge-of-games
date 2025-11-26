using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PvpBattleTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pvp_battle_teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoserTeam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PvpBattleId = table.Column<int>(type: "int", nullable: false),
                    WinnerTeam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pvp_battle_teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pvp_battle_teams_pvp_battles_PvpBattleId",
                        column: x => x.PvpBattleId,
                        principalTable: "pvp_battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pvp_battle_teams_PvpBattleId",
                table: "pvp_battle_teams",
                column: "PvpBattleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pvp_battle_teams");
        }
    }
}
