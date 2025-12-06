using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PlayerCitySnapshotAdditionalStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_AgeId",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_CityId",
                table: "player_city_snapshots");

            migrationBuilder.AddColumn<int>(
                name: "Coins1H",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Coins1HPerArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Coins24H",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Coins24HPerArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoinsPerArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Food1H",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Food1HPerArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Food24H",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Food24HPerArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FoodPerArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Goods1H",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Goods1HPerArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Goods24H",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Goods24HPerArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoodsPerArea",
                table: "player_city_snapshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_CityId_AgeId",
                table: "player_city_snapshots",
                columns: new[] { "CityId", "AgeId" });

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Coins1H",
                table: "player_city_snapshots",
                column: "Coins1H",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Coins1HPerArea",
                table: "player_city_snapshots",
                column: "Coins1HPerArea",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Coins24H",
                table: "player_city_snapshots",
                column: "Coins24H",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Coins24HPerArea",
                table: "player_city_snapshots",
                column: "Coins24HPerArea",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_CoinsPerArea",
                table: "player_city_snapshots",
                column: "CoinsPerArea",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Food1H",
                table: "player_city_snapshots",
                column: "Food1H",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Food1HPerArea",
                table: "player_city_snapshots",
                column: "Food1HPerArea",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Food24H",
                table: "player_city_snapshots",
                column: "Food24H",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Food24HPerArea",
                table: "player_city_snapshots",
                column: "Food24HPerArea",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_FoodPerArea",
                table: "player_city_snapshots",
                column: "FoodPerArea",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Goods1H",
                table: "player_city_snapshots",
                column: "Goods1H",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Goods1HPerArea",
                table: "player_city_snapshots",
                column: "Goods1HPerArea",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Goods24H",
                table: "player_city_snapshots",
                column: "Goods24H",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_Goods24HPerArea",
                table: "player_city_snapshots",
                column: "Goods24HPerArea",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_GoodsPerArea",
                table: "player_city_snapshots",
                column: "GoodsPerArea",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_CityId_AgeId",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Coins1H",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Coins1HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Coins24H",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Coins24HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_CoinsPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Food1H",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Food1HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Food24H",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Food24HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_FoodPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Goods1H",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Goods1HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Goods24H",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_Goods24HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_city_snapshots_GoodsPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Coins1H",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Coins1HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Coins24H",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Coins24HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "CoinsPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Food1H",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Food1HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Food24H",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Food24HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "FoodPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Goods1H",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Goods1HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Goods24H",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "Goods24HPerArea",
                table: "player_city_snapshots");

            migrationBuilder.DropColumn(
                name: "GoodsPerArea",
                table: "player_city_snapshots");

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_AgeId",
                table: "player_city_snapshots",
                column: "AgeId");

            migrationBuilder.CreateIndex(
                name: "IX_player_city_snapshots_CityId",
                table: "player_city_snapshots",
                column: "CityId");
        }
    }
}
