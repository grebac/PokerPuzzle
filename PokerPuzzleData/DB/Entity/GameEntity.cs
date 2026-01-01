using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.DB.Entity
{
    public class GameEntity
    {
        public int GameId { get; set; }
        public string? ExternalGameId { get; set; }

        public int NumPlayers { get; set; }
        public int FinalPot { get; set; }
        public bool HasShowdown { get; set; }
        public required string Source { get; set; }

        public ICollection<PlayerEntity> Players { get; set; } = new List<PlayerEntity>();
        public ICollection<ActionEntity> Actions { get; set; } = new List<ActionEntity>();
        public CommunityCardsEntity CommunityCards { get; set; } = null!;
    }

}
