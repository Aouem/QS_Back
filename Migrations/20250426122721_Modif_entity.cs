using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QS.Migrations
{
    /// <inheritdoc />
    public partial class Modif_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    RepondantId = table.Column<int>(type: "int", nullable: false),
                    Reponses = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormResults_Repondants_RepondantId",
                        column: x => x.RepondantId,
                        principalTable: "Repondants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormResults_RepondantId",
                table: "FormResults",
                column: "RepondantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormResults");
        }
    }
}
