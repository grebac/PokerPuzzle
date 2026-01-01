using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.DB.Entity
{
    public class PlayerEntity
    {
        public int GameId { get; set; }
        public int Position { get; set; } // seat / order

        public string? Card1 { get; set; }
        public string? Card2 { get; set; }

        public GameEntity Game { get; set; } = null!;
    }

}
