using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPuzzleData.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExternalGameId = table.Column<string>(type: "TEXT", nullable: true),
                    NumPlayers = table.Column<int>(type: "INTEGER", nullable: false),
                    FinalPot = table.Column<int>(type: "INTEGER", nullable: false),
                    HasShowdown = table.Column<bool>(type: "INTEGER", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Street = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    ActionType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => new { x.GameId, x.OrderIndex });
                    table.ForeignKey(
                        name: "FK_Actions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityCards",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Flop1 = table.Column<string>(type: "TEXT", nullable: true),
                    Flop2 = table.Column<string>(type: "TEXT", nullable: true),
                    Flop3 = table.Column<string>(type: "TEXT", nullable: true),
                    Turn = table.Column<string>(type: "TEXT", nullable: true),
                    River = table.Column<string>(type: "TEXT", nullable: true),
                    FlopPotSize = table.Column<int>(type: "INTEGER", nullable: false),
                    TurnPotSize = table.Column<int>(type: "INTEGER", nullable: false),
                    RiverPotSize = table.Column<int>(type: "INTEGER", nullable: false),
                    ShowdownPotSize = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityCards", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_CommunityCards_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    Card1 = table.Column<string>(type: "TEXT", nullable: true),
                    Card2 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => new { x.GameId, x.Position });
                    table.ForeignKey(
                        name: "FK_Players_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "CommunityCards");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
