using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QS.Migrations
{
    /// <inheritdoc />
    public partial class GenreTracheAge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Repondants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrancheAge",
                table: "Repondants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Repondants");

            migrationBuilder.DropColumn(
                name: "TrancheAge",
                table: "Repondants");
        }
    }
}
