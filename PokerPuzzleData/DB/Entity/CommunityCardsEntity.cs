using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.DB.Entity
{
    public class CommunityCardsEntity
    {
        public int GameId { get; set; }

        public string? Flop1 { get; set; }
        public string? Flop2 { get; set; }
        public string? Flop3 { get; set; }
        public string? Turn { get; set; }
        public string? River { get; set; }

        public int FlopPotSize { get; set; }
        public int TurnPotSize { get; set; }
        public int RiverPotSize { get; set; }
        public int ShowdownPotSize { get; set; }
        

        public GameEntity Game { get; set; } = null!;
    }

}
