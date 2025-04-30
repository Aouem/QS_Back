using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QS.Migrations
{
    /// <inheritdoc />
    public partial class repondantdto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormResults_Repondants_RepondantId",
                table: "FormResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormResults",
                table: "FormResults");

            migrationBuilder.RenameTable(
                name: "FormResults",
                newName: "FormResultEntities");

            migrationBuilder.RenameIndex(
                name: "IX_FormResults_RepondantId",
                table: "FormResultEntities",
                newName: "IX_FormResultEntities_RepondantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormResultEntities",
                table: "FormResultEntities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormResultEntities_Repondants_RepondantId",
                table: "FormResultEntities",
                column: "RepondantId",
                principalTable: "Repondants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormResultEntities_Repondants_RepondantId",
                table: "FormResultEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormResultEntities",
                table: "FormResultEntities");

            migrationBuilder.RenameTable(
                name: "FormResultEntities",
                newName: "FormResults");

            migrationBuilder.RenameIndex(
                name: "IX_FormResultEntities_RepondantId",
                table: "FormResults",
                newName: "IX_FormResults_RepondantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormResults",
                table: "FormResults",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormResults_Repondants_RepondantId",
                table: "FormResults",
                column: "RepondantId",
                principalTable: "Repondants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
