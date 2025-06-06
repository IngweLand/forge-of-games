using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PvpBattles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pvp_battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InGameBattleId = table.Column<byte[]>(type: "varbinary(900)", nullable: false),
                    LoserUnits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WinnerUnits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorldId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    WinnerId = table.Column<int>(type: "int", nullable: false),
                    LoserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pvp_battles", x => x.Id);
                    table.CheckConstraint("CK_pvp_battles_WinnerId_LoserId_Different", "[WinnerId] <> [LoserId]");
                    table.ForeignKey(
                        name: "FK_pvp_battles_players_LoserId",
                        column: x => x.LoserId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pvp_battles_players_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pvp_battles_InGameBattleId",
                table: "pvp_battles",
                column: "InGameBattleId");

            migrationBuilder.CreateIndex(
                name: "IX_pvp_battles_LoserId",
                table: "pvp_battles",
                column: "LoserId");

            migrationBuilder.CreateIndex(
                name: "IX_pvp_battles_PerformedAt",
                table: "pvp_battles",
                column: "PerformedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_pvp_battles_WinnerId",
                table: "pvp_battles",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_pvp_battles_WorldId",
                table: "pvp_battles",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_pvp_battles_WorldId_InGameBattleId",
                table: "pvp_battles",
                columns: new[] { "WorldId", "InGameBattleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pvp_battles");
        }
    }
}
