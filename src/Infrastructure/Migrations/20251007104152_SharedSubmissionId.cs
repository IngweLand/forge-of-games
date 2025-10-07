using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SharedSubmissionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shared_submission_ids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SharedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shared_submission_ids", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shared_submission_ids_ExpiresAt",
                table: "shared_submission_ids",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_shared_submission_ids_SharedId",
                table: "shared_submission_ids",
                column: "SharedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shared_submission_ids_SubmissionId",
                table: "shared_submission_ids",
                column: "SubmissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shared_submission_ids");
        }
    }
}
