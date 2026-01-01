using Microsoft.EntityFrameworkCore;
using PokerPuzzleData.DB;
using PokerPuzzleData.DB.Entity;
using PokerPuzzleData.Enum;
using PokerPuzzleData.JSON;
using PokerPuzzleData.Script;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.Import
{
    public class GameImportService
    {
        private readonly PokerPuzzleContext _db;

        public GameImportService(PokerPuzzleContext db)
        {
            _db = db;
        }

        public void EnsureDatabaseReady()
        {
            using var context = new PokerPuzzleContext();

            // 1. Ensure schema
            context.Database.Migrate();

            // 2. Fill only once
            if (context.Games.Any())
                return;

            // 3. Load JSON data
            var jsonGames = LoadJSONGames();

            // 4. Get JSON into WITH BULK OPTIMISATION
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            using var transaction = context.Database.BeginTransaction();

            int count = 0;

            foreach (var game in jsonGames)
            {
                context.Games.Add(BuildGameEntity(game));
                count++;

                if (count % 1000 == 0)
                {
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                }
            }

            context.SaveChanges();
            transaction.Commit();
        }

        private List<PokerGameJSON> LoadJSONGames()
        {
            var path = Path.Combine(
                AppContext.BaseDirectory,
                "Ressources",
                "Data",
                "hands.json");
            return PokerGameJSONReader.readPokerGameJSON(path).ToList(); // To keep showdown games: .Where(game => AtLeastTwoPlayersRevealed(game))
        }

        private GameEntity BuildGameEntity(PokerGameJSON json)
        {
            return new GameEntity
            {
                ExternalGameId = json.Id,
                NumPlayers = json.Players.Count,
                FinalPot = json.StreetPotJSONs.LastOrDefault()?.Size ?? 0,
                HasShowdown = json.StreetPotJSONs.Any(p => p.Street == "s"),
                Source = "JSON",

                CommunityCards = BuildCommunityCards(json),
                Players = BuildPlayers(json),
                Actions = BuildActions(json)
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
    }
}
