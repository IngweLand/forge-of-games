using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BattleStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "battle_stats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InGameBattleId = table.Column<byte[]>(type: "varbinary(900)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battle_stats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "unit_battle_stats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Attack = table.Column<float>(type: "real", nullable: false),
                    Defense = table.Column<float>(type: "real", nullable: false),
                    Heal = table.Column<float>(type: "real", nullable: false),
                    UnitId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit_battle_stats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "battle_squad_stats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BattleStatsId = table.Column<int>(type: "int", nullable: false),
                    HeroId = table.Column<int>(type: "int", nullable: true),
                    Side = table.Column<int>(type: "int", nullable: false),
                    SupportUnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battle_squad_stats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_battle_squad_stats_battle_stats_BattleStatsId",
                        column: x => x.BattleStatsId,
                        principalTable: "battle_stats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_battle_squad_stats_unit_battle_stats_HeroId",
                        column: x => x.HeroId,
                        principalTable: "unit_battle_stats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_battle_squad_stats_unit_battle_stats_SupportUnitId",
                        column: x => x.SupportUnitId,
                        principalTable: "unit_battle_stats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_battle_squad_stats_BattleStatsId",
                table: "battle_squad_stats",
                column: "BattleStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_battle_squad_stats_HeroId",
                table: "battle_squad_stats",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "IX_battle_squad_stats_SupportUnitId",
                table: "battle_squad_stats",
                column: "SupportUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_battle_stats_InGameBattleId",
                table: "battle_stats",
                column: "InGameBattleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "battle_squad_stats");

            migrationBuilder.DropTable(
                name: "battle_stats");

            migrationBuilder.DropTable(
                name: "unit_battle_stats");
        }
    }
}
