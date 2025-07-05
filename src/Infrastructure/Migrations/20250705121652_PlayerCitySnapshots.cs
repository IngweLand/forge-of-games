using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerCitySnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPresentInGame",
                table: "players",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "player_city_snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Coins = table.Column<int>(type: "int", nullable: false),
                    CollectedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    CompressedData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Food = table.Column<int>(type: "int", nullable: false),
                    Goods = table.Column<int>(type: "int", nullable: false),
                    HasPremiumBuildings = table.Column<bool>(type: "bit", nullable: false),
                    OpenedExpansionsHash = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_city_snapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_player_city_snapshots_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_players_IsPresentInGame",
                table: "players",
                column: "IsPresentInGame");

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_CityId",
                table: "player_city_snapshots",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Coins",
                table: "player_city_snapshots",
                column: "Coins",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Food",
                table: "player_city_snapshots",
                column: "Food",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Goods",
                table: "player_city_snapshots",
                column: "Goods",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_HasPremiumBuildings",
                table: "player_city_snapshots",
                column: "HasPremiumBuildings");

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_OpenedExpansionsHash",
                table: "player_city_snapshots",
                column: "OpenedExpansionsHash");

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_PlayerId_CityId_CollectedAt",
                table: "player_city_snapshots",
                columns: new[] { "PlayerId", "CityId", "CollectedAt" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_players_IsPresentInGame",
                table: "players");

            migrationBuilder.DropColumn(
                name: "IsPresentInGame",
                table: "players");
        }
    }
}
