using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBattleUnitSide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_battle_units_UnitId_Level",
                table: "battle_units");

            migrationBuilder.AddColumn<int>(
                name: "Side",
                table: "battle_units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_battle_units_Side",
                table: "battle_units",
                column: "Side");

            migrationBuilder.CreateIndex(
                name: "IX_battle_units_UnitId_Level_Side",
                table: "battle_units",
                columns: new[] { "UnitId", "Level", "Side" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_battle_units_Side",
                table: "battle_units");

            migrationBuilder.DropIndex(
                name: "IX_battle_units_UnitId_Level_Side",
                table: "battle_units");

            migrationBuilder.DropColumn(
                name: "Side",
                table: "battle_units");

            migrationBuilder.CreateIndex(
                name: "IX_battle_units_UnitId_Level",
                table: "battle_units",
                columns: new[] { "UnitId", "Level" },
                unique: true);
        }
    }
}
