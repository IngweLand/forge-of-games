using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alliance_name_history_entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alliance_name_history_entries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "alliance_rankings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    CollectedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alliance_rankings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlliancePlayer",
                columns: table => new
                {
                    AllianceHistoryId = table.Column<int>(type: "int", nullable: false),
                    MemberHistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlliancePlayer", x => new { x.AllianceHistoryId, x.MemberHistoryId });
                });

            migrationBuilder.CreateTable(
                name: "alliances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvatarBackgroundId = table.Column<int>(type: "int", nullable: false),
                    AvatarIconId = table.Column<int>(type: "int", nullable: false),
                    InGameAllianceId = table.Column<int>(type: "int", nullable: false),
                    LeaderId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    RankingPoints = table.Column<int>(type: "int", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    WorldId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alliances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AllianceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AvatarId = table.Column<int>(type: "int", nullable: false),
                    InGamePlayerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: true),
                    RankingPoints = table.Column<int>(type: "int", nullable: true),
                    CurrentAllianceId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    WorldId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_players_alliances_CurrentAllianceId",
                        column: x => x.CurrentAllianceId,
                        principalTable: "alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "player_age_history_entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Age = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_age_history_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_player_age_history_entries_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "player_name_history_entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_name_history_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_player_name_history_entries_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_rankings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_rankings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_player_rankings_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pvp_rankings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pvp_rankings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pvp_rankings_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alliance_name_history_entries_AllianceId_Name_ChangedAt",
                table: "alliance_name_history_entries",
                columns: new[] { "AllianceId", "Name", "ChangedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_alliance_name_history_entries_Name",
                table: "alliance_name_history_entries",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_alliance_rankings_AllianceId",
                table: "alliance_rankings",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_alliance_rankings_CollectedAt",
                table: "alliance_rankings",
                column: "CollectedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_AlliancePlayer_MemberHistoryId",
                table: "AlliancePlayer",
                column: "MemberHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_alliances_InGameAllianceId",
                table: "alliances",
                column: "InGameAllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_alliances_LeaderId",
                table: "alliances",
                column: "LeaderId",
                unique: true,
                filter: "[LeaderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_alliances_Name",
                table: "alliances",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_alliances_RankingPoints",
                table: "alliances",
                column: "RankingPoints",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_alliances_WorldId",
                table: "alliances",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_alliances_WorldId_InGameAllianceId",
                table: "alliances",
                columns: new[] { "WorldId", "InGameAllianceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_age_history_entries_PlayerId_Age_ChangedAt",
                table: "player_age_history_entries",
                columns: new[] { "PlayerId", "Age", "ChangedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_alliance_name_history_entries_AllianceName",
                table: "player_alliance_name_history_entries",
                column: "AllianceName");

            migrationBuilder.CreateIndex(
                name: "IX_player_alliance_name_history_entries_PlayerId_AllianceName",
                table: "player_alliance_name_history_entries",
                columns: new[] { "PlayerId", "AllianceName" });

            migrationBuilder.CreateIndex(
                name: "IX_player_name_history_entries_Name",
                table: "player_name_history_entries",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_player_name_history_entries_PlayerId_Name",
                table: "player_name_history_entries",
                columns: new[] { "PlayerId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_player_rankings_CollectedAt",
                table: "player_rankings",
                column: "CollectedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_player_rankings_PlayerId",
                table: "player_rankings",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_players_Age",
                table: "players",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_players_CurrentAllianceId",
                table: "players",
                column: "CurrentAllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_players_InGamePlayerId",
                table: "players",
                column: "InGamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_players_Name",
                table: "players",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_players_RankingPoints",
                table: "players",
                column: "RankingPoints",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_players_WorldId",
                table: "players",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_players_WorldId_InGamePlayerId",
                table: "players",
                columns: new[] { "WorldId", "InGamePlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pvp_rankings_CollectedAt",
                table: "pvp_rankings",
                column: "CollectedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_pvp_rankings_PlayerId",
                table: "pvp_rankings",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_alliance_name_history_entries_alliances_AllianceId",
                table: "alliance_name_history_entries",
                column: "AllianceId",
                principalTable: "alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_alliance_rankings_alliances_AllianceId",
                table: "alliance_rankings",
                column: "AllianceId",
                principalTable: "alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlliancePlayer_alliances_AllianceHistoryId",
                table: "AlliancePlayer",
                column: "AllianceHistoryId",
                principalTable: "alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlliancePlayer_players_MemberHistoryId",
                table: "AlliancePlayer",
                column: "MemberHistoryId",
                principalTable: "players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_alliances_players_LeaderId",
                table: "alliances",
                column: "LeaderId",
                principalTable: "players",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_players_alliances_CurrentAllianceId",
                table: "players");

            migrationBuilder.DropTable(
                name: "alliance_name_history_entries");

            migrationBuilder.DropTable(
                name: "alliance_rankings");

            migrationBuilder.DropTable(
                name: "AlliancePlayer");

            migrationBuilder.DropTable(
                name: "player_age_history_entries");

            migrationBuilder.DropTable(
                name: "player_alliance_name_history_entries");

            migrationBuilder.DropTable(
                name: "player_name_history_entries");

            migrationBuilder.DropTable(
                name: "player_rankings");

            migrationBuilder.DropTable(
                name: "pvp_rankings");

            migrationBuilder.DropTable(
                name: "alliances");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
