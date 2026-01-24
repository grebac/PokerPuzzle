using Microsoft.Data.Sqlite;
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
        public GameRepository()
        {
        }

        public GameEntity? GetGame(int gameId)
        {
            using (var _db = new PokerPuzzleContext())
            {
                return _db.Games
                    .Include(g => g.Players)
                    .Include(g => g.Actions)
                    .Include(g => g.CommunityCards)
                    .Where(g => g.HasShowdown && g.Players.Count <= 10)
                    .FirstOrDefault(g => g.GameId == gameId);
            }
        }

        public GameEntity? GetRandomGame()
        {
            using (var _db = new PokerPuzzleContext())
            {
                return _db.Games
                    .Include(g => g.Players)
                    .Include(g => g.Actions)
                    .Include(g => g.CommunityCards)
                    .Where(g => g.HasShowdown && g.Players.Count <= 10)
                    .Include(g => g.Players)
                    .Include(g => g.Actions)
                    .Include(g => g.CommunityCards)
                    .OrderBy(x => EF.Functions.Random())
                    .FirstOrDefault();
            }
        }

        public List<GameEntity> GetFavoriteGames() {
            using (var _db = new PokerPuzzleContext())
            {
                return _db.Games
                    .Include(g => g.Players)
                    .Include(g => g.Actions)
                    .Include(g => g.CommunityCards)
                    .Where(g => g.isFavorite).ToList();
            }
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
                    isPairedFlop: false, // TODO
                    g.GameComment?.Text
                ));
        }

        public IList<GameSummaryDTO> GetGameSummaries(List<int> ids) {
            using (var _db = new PokerPuzzleContext())
            {
                var favoriteGames = _db.Games
                    .Include(g => g.CommunityCards)
                    .Include(g => g.Players)
                    .Include(g => g.Actions)
                    .Include(g => g.GameComment)
                    .Where(g => ids.Contains(g.GameId));
                return _filterGameSummary(favoriteGames).ToList();
            }
        }

        public GameSummaryDTO GetGameSummary(int id) {
            using (var _db = new PokerPuzzleContext())
            {
                var game = _db.Games
                    .Include(g => g.CommunityCards)
                    .Include(g => g.Players)
                    .Include(g => g.Actions)
                    .Where(g => g.GameId == id);
                return _filterGameSummary(game).FirstOrDefault();
            }
        }

        public void updateComment(int gameId, string content) {
            using (var _db = new PokerPuzzleContext())
            {
                var comment = _db.GameComments.Where(c => c.GameId == gameId).FirstOrDefault();
                if (comment == null)
                {
                    comment = new GameCommentEntity() { GameId = gameId, CreatedAt = DateTime.Now, Text = content };
                    _db.GameComments.Add(comment);
                } else {
                    comment.Text = content;
                    comment.CreatedAt = DateTime.Now;
                }
                _db.SaveChanges();
            }
        }

        public void setFavorite(int gameId, bool isFavorite)
        {
            using (var _db = new PokerPuzzleContext()) {
                var game = _db.Games.Where(g => g.GameId == gameId).FirstOrDefault();
                if (game == null) {
                    return;
                }
                game.isFavorite = isFavorite;
                _db.SaveChanges();
            }
        }

        public void DeleteDatabase()
        {
            using var context = new PokerPuzzleContext();
            var dbPath = context.Database.GetDbConnection().DataSource;

            context.Dispose();
            SqliteConnection.ClearAllPools();


            if (File.Exists(dbPath))
                File.Delete(dbPath);
        }
    }
}
