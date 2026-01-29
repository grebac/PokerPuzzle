using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPuzzleData.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlayerStack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Stack",
                table: "Players",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stack",
                table: "Players");
        }
    }
}
