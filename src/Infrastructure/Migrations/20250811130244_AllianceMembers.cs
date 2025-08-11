using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AllianceMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_alliances_players_LeaderId",
                table: "alliances");

            migrationBuilder.DropForeignKey(
                name: "FK_players_alliances_CurrentAllianceId",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_players_CurrentAllianceId",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_alliances_LeaderId",
                table: "alliances");

            migrationBuilder.DropColumn(
                name: "CurrentAllianceId",
                table: "players");

            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "alliances");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeenOnline",
                table: "players",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ProfileUpdatedAt",
                table: "players",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateTime>(
                name: "MembersUpdatedAt",
                table: "alliances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "alliances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "Active");

            migrationBuilder.CreateTable(
                name: "alliance_members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alliance_members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_alliance_members_alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalTable: "alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_alliance_members_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_players_ProfileUpdatedAt",
                table: "players",
                column: "ProfileUpdatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_players_UpdatedAt",
                table: "players",
                column: "UpdatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_alliances_MembersUpdatedAt",
                table: "alliances",
                column: "MembersUpdatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_alliances_Status",
                table: "alliances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_alliances_UpdatedAt",
                table: "alliances",
                column: "UpdatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_alliance_members_AllianceId",
                table: "alliance_members",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_alliance_members_PlayerId",
                table: "alliance_members",
                column: "PlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_alliance_members_Role",
                table: "alliance_members",
                column: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alliance_members");

            migrationBuilder.DropIndex(
                name: "IX_players_ProfileUpdatedAt",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_players_UpdatedAt",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_alliances_MembersUpdatedAt",
                table: "alliances");

            migrationBuilder.DropIndex(
                name: "IX_alliances_Status",
                table: "alliances");

            migrationBuilder.DropIndex(
                name: "IX_alliances_UpdatedAt",
                table: "alliances");

            migrationBuilder.DropColumn(
                name: "LastSeenOnline",
                table: "players");

            migrationBuilder.DropColumn(
                name: "ProfileUpdatedAt",
                table: "players");

            migrationBuilder.DropColumn(
                name: "MembersUpdatedAt",
                table: "alliances");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "alliances");

            migrationBuilder.AddColumn<int>(
                name: "CurrentAllianceId",
                table: "players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeaderId",
                table: "alliances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_players_CurrentAllianceId",
                table: "players",
                column: "CurrentAllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_alliances_LeaderId",
                table: "alliances",
                column: "LeaderId",
                unique: true,
                filter: "[LeaderId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_alliances_players_LeaderId",
                table: "alliances",
                column: "LeaderId",
                principalTable: "players",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_players_alliances_CurrentAllianceId",
                table: "players",
                column: "CurrentAllianceId",
                principalTable: "alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
