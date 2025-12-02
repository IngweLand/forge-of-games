using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBattleIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_battles_BattleDefinitionId",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_PerformedAt",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_SubmissionId",
                table: "battles");

            migrationBuilder.CreateIndex(
                name: "IX_battles_BattleDefinitionId_Id_Desc",
                table: "battles",
                columns: new[] { "BattleDefinitionId", "Id" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_battles_BattleDefinitionId_ResultStatus_Id_Desc",
                table: "battles",
                columns: new[] { "BattleDefinitionId", "ResultStatus", "Id" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "IX_battles_SubmissionId_BattleType_BattleDefinitionId_ResultStatus_Id_Desc",
                table: "battles",
                columns: new[] { "SubmissionId", "BattleType", "BattleDefinitionId", "ResultStatus", "Id" },
                descending: new[] { false, false, false, false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_battles_BattleDefinitionId_Id_Desc",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_BattleDefinitionId_ResultStatus_Id_Desc",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_SubmissionId_BattleType_BattleDefinitionId_ResultStatus_Id_Desc",
                table: "battles");

            migrationBuilder.CreateIndex(
                name: "IX_battles_BattleDefinitionId",
                table: "battles",
                column: "BattleDefinitionId");

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
    }
}
