using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BattleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BattleType",
                table: "battles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "Undefined");

            migrationBuilder.CreateIndex(
                name: "IX_battles_BattleType",
                table: "battles",
                column: "BattleType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_battles_BattleType",
                table: "battles");

            migrationBuilder.DropColumn(
                name: "BattleType",
                table: "battles");
        }
    }
}
