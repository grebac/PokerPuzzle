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
        public GameRepository() {}

        #region Games
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
        #endregion

        #region GameSummaries
        public IList<GameSummaryDTO> GetGameSummaryPage(int pageSize, int pageIndex, BoardTexture filterCriteria)
        {
            using (var _db = new PokerPuzzleContext())
            {
                var GameSummariesQuery = _getFilteredGameSummary(filterCriteria, _db);
                return GameSummariesQuery.Skip(pageSize * pageIndex).Take(pageSize).ToList();
            }
        }

        public int GetGameSummaryCount(BoardTexture filterCriteria) {
            using (var _db = new PokerPuzzleContext())
            {
                return _getFilteredGameSummary(filterCriteria, _db).Count();
            }
        }

        private IQueryable<GameSummaryDTO> _getFilteredGameSummary(BoardTexture filterCriteria, PokerPuzzleContext _db) {
            var GameQuery = _db.Games.AsNoTracking().Where(g => (g.BoardTexture & filterCriteria) == filterCriteria);
            var GameSummariesQuery = _filterGameSummary(GameQuery);
            return GameSummariesQuery;
        }

        public IList<GameSummaryDTO> GetGameSummaries(List<int> ids)
        {
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

        private GameSummaryDTO GetGameSummary(int id)
        {
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
        private IQueryable<GameSummaryDTO> _filterGameSummary(IQueryable<GameEntity> games) {
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
                    boardTexture: g.BoardTexture,
                    comment: g.GameComment != null ? g.GameComment.Text : string.Empty
                ));
        }
        #endregion

        #region Comment
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
        #endregion

        #region FavoriteGames
        public List<GameEntity> GetFavoriteGames()
        {
            using (var _db = new PokerPuzzleContext())
            {
                return _db.Games
                    .Include(g => g.Players)
                    .Include(g => g.Actions)
                    .Include(g => g.CommunityCards)
                    .Where(g => g.isFavorite).ToList();
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
        #endregion

        #region Other
        public void DeleteDatabase()
        {
            using var context = new PokerPuzzleContext();
            var dbPath = context.Database.GetDbConnection().DataSource;

            context.Dispose();
            SqliteConnection.ClearAllPools();


            if (File.Exists(dbPath))
                File.Delete(dbPath);
        }
        #endregion
    }
}
