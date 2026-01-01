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

        public GameEntity GetGame(int gameId)
        {
            return _db.Games
                .Include(g => g.Players)
                .Include(g => g.Actions)
                .Include(g => g.CommunityCards)
                .FirstOrDefault(g => g.GameId == gameId);
        }

        public GameEntity GetRandomGame()
        {
            var count = _db.Games.Count();

            var randomIndex = Random.Shared.Next(count);

            return _db.Games
                .Include(g => g.Players)
                .Include(g => g.Actions)
                .Include(g => g.CommunityCards)
                .Skip(randomIndex)
                .First();
        }
    }
}
