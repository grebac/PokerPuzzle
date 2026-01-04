using Microsoft.EntityFrameworkCore;
using PokerPuzzleData.DB.Entity;
using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.DB.Repository
{
    public class GameRepository
    {
        private readonly PokerPuzzleContext _db;

        public GameRepository(PokerPuzzleContext db)
        {
            _db = db;
        }

        private IQueryable<GameEntity> GetGames() {
            return _db.Games
                .Include(g => g.Players)
                .Include(g => g.Actions)
                .Include(g => g.CommunityCards)
                .Where(g => g.HasShowdown && g.Players.Count <= 10);
        }

        public GameEntity GetGame(int gameId)
        {
            return GetGames().FirstOrDefault(g => g.GameId == gameId);
        }

        public GameEntity GetRandomGame()
        {
            var games = GetGames();

            return games
                .Include(g => g.Players)
                .Include(g => g.Actions)
                .Include(g => g.CommunityCards)
                .OrderBy(x => EF.Functions.Random())
                .FirstOrDefault();
        }

        private IEnumerable<GameSummaryDTO> _filterGameSummary(IEnumerable<GameEntity> games) {
            return games.Select(g => new GameSummaryDTO(
                    gameId: g.GameId,
                    playerCount: g.Players.Count,
                    finalPotSize: g.CommunityCards.ShowdownPotSize,
                    reachedFlop: g.Actions.Any(a => a.Street == StreetEnum.Flop),
                    reachedTurn: g.Actions.Any(a => a.Street == StreetEnum.Turn),
                    reachedRiver: g.Actions.Any(a => a.Street == StreetEnum.River),
                    communityCards: new List<CardsEnum>
                    {
                        CardHelper.fromCodeToEnum(g.CommunityCards.Flop1),
                        CardHelper.fromCodeToEnum(g.CommunityCards.Flop2),
                        CardHelper.fromCodeToEnum(g.CommunityCards.Flop3),
                        CardHelper.fromCodeToEnum(g.CommunityCards.Turn),
                        CardHelper.fromCodeToEnum(g.CommunityCards.River)
                    },
                    isConnectedFlop: false, // TODO - Implement helper function
                    isPairedFlop: false // TODO
                ));
        }

        public IEnumerable<GameSummaryDTO> GetGameSummaries(List<int> ids) {
            var favoriteGames = _db.Games
                .Include(g => g.CommunityCards)
                .Include(g => g.Players)
                .Include(g => g.Actions)
                .Where(g => ids.Contains(g.GameId));
            return _filterGameSummary(favoriteGames);
        }

        public GameSummaryDTO GetGameSummary(int id) {
            var game = _db.Games
                .Include(g => g.CommunityCards)
                .Include(g => g.Players)
                .Include(g => g.Actions)
                .Where(g => g.GameId == id);
            return _filterGameSummary(game).FirstOrDefault();
        }
    }
}
