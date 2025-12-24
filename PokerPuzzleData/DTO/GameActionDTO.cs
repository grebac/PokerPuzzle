using PokerPuzzleData.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PokerPuzzleData.DTO
{
    // TODO - PlayerName may need to be changed to PlayerPosition => It would anonymise the data
    public class GameActionDTO
    {
        public int PlayerPosition { get; init; }
        public StreetEnum Street { get; init; }
        public ActionTypeEnum Action { get; init; }
        public int OrderIndex { get; init; }

        public GameActionDTO(int playerPosition, StreetEnum street, ActionTypeEnum action, int orderIndex) {
            PlayerPosition = playerPosition;
            this.Street = street;
            this.Action = action;
            this.OrderIndex = orderIndex;
        }
    }
}
