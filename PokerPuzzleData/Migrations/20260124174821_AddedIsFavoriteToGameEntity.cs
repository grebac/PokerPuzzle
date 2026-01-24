using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPuzzleData.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsFavoriteToGameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFavorite",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFavorite",
                table: "Games");
        }
    }
}
