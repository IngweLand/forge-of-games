using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedIndices2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_players_Age",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_players_WorldId",
                table: "players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_players_Age",
                table: "players",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_players_WorldId",
                table: "players",
                column: "WorldId");
        }
    }
}
