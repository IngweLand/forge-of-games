using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInGameStatusStringConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert existing string values to int values
            migrationBuilder.Sql(@"
                UPDATE players
                SET Status =
                    CASE Status
                        WHEN 'Missing' THEN 0
                        ELSE 1 -- default
                    END
            ");
            migrationBuilder.Sql(@"
                UPDATE alliances
                SET Status =
                    CASE Status
                        WHEN 'Missing' THEN 0
                        ELSE 1 -- default
                    END
            ");
            
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "players",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "Active");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "alliances",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "Active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "players",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "Active",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "alliances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "Active",
                oldClrType: typeof(int),
                oldType: "int");
            
            // Convert existing int values to string values
            migrationBuilder.Sql(@"
                UPDATE players
                SET Status =
                    CASE Status
                        WHEN 0 THEN 'Missing'
                        ELSE 'Active' -- default
                    END
            ");
            
            migrationBuilder.Sql(@"
                UPDATE alliances
                SET Status =
                    CASE Status
                        WHEN 0 THEN 'Missing'
                        ELSE 'Active' -- default
                    END
            ");
        }
    }
}
