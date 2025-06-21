using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Battles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "battle_units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battle_units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BattleDefinitionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnemySquads = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InGameBattleId = table.Column<byte[]>(type: "varbinary(900)", nullable: false),
                    PlayerSquads = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultStatus = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    WorldId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "battles_to_units",
                columns: table => new
                {
                    BattlesId = table.Column<int>(type: "int", nullable: false),
                    UnitsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battles_to_units", x => new { x.BattlesId, x.UnitsId });
                    table.ForeignKey(
                        name: "FK_battles_to_units_battle_units_UnitsId",
                        column: x => x.UnitsId,
                        principalTable: "battle_units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_battles_to_units_battles_BattlesId",
                        column: x => x.BattlesId,
                        principalTable: "battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_battle_units_Level",
                table: "battle_units",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_battle_units_UnitId",
                table: "battle_units",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_battle_units_UnitId_Level",
                table: "battle_units",
                columns: new[] { "UnitId", "Level" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_battles_BattleDefinitionId",
                table: "battles",
                column: "BattleDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_battles_Difficulty",
                table: "battles",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_battles_InGameBattleId",
                table: "battles",
                column: "InGameBattleId");

            migrationBuilder.CreateIndex(
                name: "IX_battles_ResultStatus",
                table: "battles",
                column: "ResultStatus");

            migrationBuilder.CreateIndex(
                name: "IX_battles_WorldId",
                table: "battles",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_battles_WorldId_InGameBattleId",
                table: "battles",
                columns: new[] { "WorldId", "InGameBattleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_battles_to_units_UnitsId",
                table: "battles_to_units",
                column: "UnitsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "battles_to_units");

            migrationBuilder.DropTable(
                name: "battle_units");

            migrationBuilder.DropTable(
                name: "battles");
        }
    }
}
