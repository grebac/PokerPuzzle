using Microsoft.EntityFrameworkCore;
using PokerPuzzleData.DB.Entity;
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
                .Where(g => g.HasShowdown);
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
    }
}
