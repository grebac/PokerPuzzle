using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPuzzleData.Migrations
{
    /// <inheritdoc />
    public partial class AddedGameAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardTexture",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoardTexture",
                table: "Games");
        }
    }
}
