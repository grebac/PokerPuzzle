using Microsoft.EntityFrameworkCore;
using PokerPuzzleData.DB;
using PokerPuzzleData.DB.Entity;
using PokerPuzzleData.Enum;
using PokerPuzzleData.JSON;
using PokerPuzzleData.Script;
using System.Diagnostics;

namespace PokerPuzzleData.Service
{
    public class GameImportService
    {
        private readonly string _jsonPath;

        public GameImportService(string ?jsonPath = null)
        {
            _jsonPath = jsonPath ?? GetDefaultJsonPath();
        }

        public void EnsureDatabaseReady(IProgress<ImportProgress>? progress = null)
        {
            using (var _db = new PokerPuzzleContext())
            {
                // 1. Ensure schema
                _db.Database.Migrate();

                // 2. Fill only once
                if (_db.Games.Any())
                {
                    return;
                }

                // 3. Ensure JSON file exists
                if (!Path.Exists(_jsonPath))
                {
                    throw new FileNotFoundException("Poker hand JSON file not found. Can't initialize the Database.", _jsonPath);
                }

                // 4. Import JSON data
                ImportJSON(_db, progress);
            }
        }

        private void ImportJSON(PokerPuzzleContext context, IProgress<ImportProgress>? progress = null) {
            // Load JSON data
            var jsonGames = LoadJSONGames(_jsonPath);

            // Get JSON into WITH BULK OPTIMISATION
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            using var transaction = context.Database.BeginTransaction();

            int processed = 0;
            int total = jsonGames.Count;

            foreach (var game in jsonGames)
            {
                context.Games.Add(BuildGameEntity(game));
                processed++;

                if (processed % 10000 == 0)
                {
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                }
                progress?.Report(new ImportProgress(processed, total));
            }

            context.SaveChanges();
            transaction.Commit();
        }

        private List<PokerGameJSON> LoadJSONGames(string path)
        {
            return PokerGameJSONReader.readPokerGameJSON(path).ToList();
        }

        private static string GetDefaultJsonPath() => Path.Combine(AppContext.BaseDirectory, "Ressources", "Data", "hands.json");

        #region ConvertJsonToEntityFramework
        private GameEntity BuildGameEntity(PokerGameJSON json)
        {
            var actions = BuildActions(json);
            return new GameEntity
            {
                ExternalGameId = json.Id,
                NumPlayers = json.Players.Count,
                FinalPot = json.StreetPotJSONs.LastOrDefault()?.Size ?? 0,
                HasFlop = actions.Any(a => a.Street == StreetEnum.Preflop && a.ActionType == ActionTypeEnum.StreetEnd), // If Preflop was ended, there is a flop
                HasShowdown = actions.Any(a => a.Street == StreetEnum.River && a.ActionType == ActionTypeEnum.StreetEnd), // If river was ended, there is a showndown
                Source = "JSON",

                CommunityCards = BuildCommunityCards(json),
                Players = BuildPlayers(json),
                Actions = actions
            };
        }

        private CommunityCardsEntity BuildCommunityCards(PokerGameJSON json)
        {
            var streetPots = json.ParseStreetPots();
            return new CommunityCardsEntity
            {
                Flop1 = json.Board.ElementAtOrDefault(0),
                Flop2 = json.Board.ElementAtOrDefault(1),
                Flop3 = json.Board.ElementAtOrDefault(2),
                Turn = json.Board.ElementAtOrDefault(3),
                River = json.Board.ElementAtOrDefault(4),
                FlopPotSize = streetPots.GetValueOrDefault(StreetEnum.Flop),
                TurnPotSize = streetPots.GetValueOrDefault(StreetEnum.Turn),
                RiverPotSize = streetPots.GetValueOrDefault(StreetEnum.River),
                ShowdownPotSize = streetPots.GetValueOrDefault(StreetEnum.Showdown)
            };
        }

        private List<PlayerEntity> BuildPlayers(PokerGameJSON json)
        {
            var players = new List<PlayerEntity>();

            foreach (var (_, playerJson) in json.Players)
            {
                players.Add(new PlayerEntity
                {
                    Position = playerJson.Position,

                    Card1 = playerJson.PocketCards.ElementAtOrDefault(0),
                    Card2 = playerJson.PocketCards.ElementAtOrDefault(1)
                });
            }

            return players;
        }

        private List<ActionEntity> BuildActions(PokerGameJSON json)
        {
            var actions = new List<ActionEntity>();

            var dtoActions = json.BuildGameActions();

            foreach (var dto in dtoActions)
            {
                actions.Add(new ActionEntity
                {
                    OrderIndex = dto.OrderIndex,
                    Street = dto.Street,
                    PlayerPosition = dto.PlayerPosition,
                    ActionType = dto.Action
                });
            }

            return actions;
        }
        #endregion
    }
}
