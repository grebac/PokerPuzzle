using PokerPuzzleData.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.DB.Entity
{
    public class ActionEntity
    {
        public int GameId { get; set; }
        public int OrderIndex { get; set; }
        public StreetEnum Street { get; set; }
        public int PlayerPosition { get; set; }
        public ActionTypeEnum ActionType { get; set; }

        public GameEntity Game { get; set; } = null!;
    }

}
