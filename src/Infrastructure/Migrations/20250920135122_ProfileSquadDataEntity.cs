using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingweland.Fog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProfileSquadDataEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "profile_squad_data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileSquadId = table.Column<int>(type: "int", nullable: false),
                    SerializedHero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerializedSupportUnit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_squad_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_profile_squad_data_profile_squads_ProfileSquadId",
                        column: x => x.ProfileSquadId,
                        principalTable: "profile_squads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_profile_squad_data_ProfileSquadId",
                table: "profile_squad_data",
                column: "ProfileSquadId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profile_squad_data");
        }
    }
}
