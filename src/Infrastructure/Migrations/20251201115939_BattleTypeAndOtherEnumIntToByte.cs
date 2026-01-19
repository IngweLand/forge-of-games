using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BattleTypeAndOtherEnumIntToByte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert existing string values to byte values
            migrationBuilder.Sql($@"
                UPDATE battles
                SET BattleType =
                    CASE BattleType
                        WHEN '{Models.Hoh.Enums.BattleType.Campaign.ToString()}' THEN {(byte)Models.Hoh.Enums.BattleType.Campaign}
                        WHEN '{Models.Hoh.Enums.BattleType.HistoricBattle.ToString()}' THEN {(byte)Models.Hoh.Enums.BattleType.HistoricBattle}
                        WHEN '{Models.Hoh.Enums.BattleType.Pvp.ToString()}' THEN {(byte)Models.Hoh.Enums.BattleType.Pvp}
                        WHEN '{Models.Hoh.Enums.BattleType.TeslaStorm.ToString()}' THEN {(byte)Models.Hoh.Enums.BattleType.TeslaStorm}
                        WHEN '{Models.Hoh.Enums.BattleType.TreasureHunt.ToString()}' THEN {(byte)Models.Hoh.Enums.BattleType.TreasureHunt}
                        WHEN '{Models.Hoh.Enums.BattleType.AncientEgypt.ToString()}' THEN {(byte)Models.Hoh.Enums.BattleType.AncientEgypt}
                        ELSE {(byte)Models.Hoh.Enums.BattleType.Undefined} -- default
                    END
            ");
            
            
            migrationBuilder.AlterColumn<byte>(
                name: "ResultStatus",
                table: "battles",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Difficulty",
                table: "battles",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "BattleType",
                table: "battles",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "Undefined");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ResultStatus",
                table: "battles",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "Difficulty",
                table: "battles",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "BattleType",
                table: "battles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "Undefined",
                oldClrType: typeof(byte),
                oldType: "tinyint");
            
            // Convert existing byte values to string values
            migrationBuilder.Sql($@"
                UPDATE battles
                SET BattleType =
                    CASE BattleType
                        WHEN {(byte)Models.Hoh.Enums.BattleType.Campaign} THEN '{Models.Hoh.Enums.BattleType.Campaign.ToString()}'
                        WHEN {(byte)Models.Hoh.Enums.BattleType.HistoricBattle} THEN '{Models.Hoh.Enums.BattleType.HistoricBattle.ToString()}'
                        WHEN {(byte)Models.Hoh.Enums.BattleType.Pvp} THEN '{Models.Hoh.Enums.BattleType.Pvp.ToString()}'
                        WHEN {(byte)Models.Hoh.Enums.BattleType.TeslaStorm} THEN '{Models.Hoh.Enums.BattleType.TeslaStorm.ToString()}'
                        WHEN {(byte)Models.Hoh.Enums.BattleType.TreasureHunt} THEN '{Models.Hoh.Enums.BattleType.TreasureHunt.ToString()}'
                        WHEN {(byte)Models.Hoh.Enums.BattleType.AncientEgypt} THEN '{Models.Hoh.Enums.BattleType.AncientEgypt.ToString()}'
                        ELSE '{Models.Hoh.Enums.BattleType.Undefined.ToString()}' -- default
                    END
            ");
        }
    }
}
