using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentInsights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "equipment_insights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromLevel = table.Column<int>(type: "int", nullable: false),
                    ToLevel = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentSlotType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentSets = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainAttributes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubAttributesLevel4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubAttributesLevel8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubAttributesLevel12 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubAttributesLevel16 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment_insights", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_equipment_insights_EquipmentSlotType",
                table: "equipment_insights",
                column: "EquipmentSlotType");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_insights_FromLevel",
                table: "equipment_insights",
                column: "FromLevel");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_insights_ToLevel",
                table: "equipment_insights",
                column: "ToLevel");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_insights_UnitId",
                table: "equipment_insights",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_insights_UnitId_EquipmentSlotType_FromLevel_ToLevel",
                table: "equipment_insights",
                columns: new[] { "UnitId", "EquipmentSlotType", "FromLevel", "ToLevel" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipment_insights");
        }
    }
}
