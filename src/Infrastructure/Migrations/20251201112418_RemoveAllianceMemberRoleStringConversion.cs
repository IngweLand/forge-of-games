using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAllianceMemberRoleStringConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert existing string values to byte values
            migrationBuilder.Sql($@"
                UPDATE alliance_members
                SET Role =
                    CASE Role
                        WHEN '{AllianceMemberRole.AllianceLeader.ToString()}' THEN {(byte)AllianceMemberRole.AllianceLeader}
                        WHEN '{AllianceMemberRole.AllianceMinister.ToString()}' THEN {(byte)AllianceMemberRole.AllianceMinister}
                        WHEN '{AllianceMemberRole.AllianceTrainee.ToString()}' THEN {(byte)AllianceMemberRole.AllianceTrainee}
                        ELSE {(byte)AllianceMemberRole.AllianceTrainee} -- default
                    END
            ");
            
            migrationBuilder.AlterColumn<byte>(
                name: "Role",
                table: "alliance_members",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "alliance_members",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
            
            // Convert existing byte values to string values
            migrationBuilder.Sql($@"
                UPDATE alliance_members
                SET Role =
                    CASE Role
                        WHEN {AllianceMemberRole.AllianceLeader} THEN '{AllianceMemberRole.AllianceLeader.ToString()}'
                        WHEN {AllianceMemberRole.AllianceMinister} THEN '{AllianceMemberRole.AllianceMinister.ToString()}'
                        WHEN {AllianceMemberRole.AllianceTrainee} THEN '{AllianceMemberRole.AllianceTrainee.ToString()}'
                        ELSE '{AllianceMemberRole.AllianceTrainee.ToString()}' -- default
                    END
            ");
        }
    }
}
