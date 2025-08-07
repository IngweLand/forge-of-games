using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BattleSubmissionIdAndPerformedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "PerformedAt",
                table: "battles",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<Guid>(
                name: "SubmissionId",
                table: "battles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_battles_PerformedAt",
                table: "battles",
                column: "PerformedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_battles_SubmissionId",
                table: "battles",
                column: "SubmissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_battles_PerformedAt",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_SubmissionId",
                table: "battles");

            migrationBuilder.DropColumn(
                name: "PerformedAt",
                table: "battles");

            migrationBuilder.DropColumn(
                name: "SubmissionId",
                table: "battles");
        }
    }
}
