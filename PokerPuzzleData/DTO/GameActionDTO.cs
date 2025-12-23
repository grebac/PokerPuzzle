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
        public string PlayerName { get; init; } = "";
        public int PlayerPosition { get; init; }
        public StreetEnum Street { get; init; }
        public ActionTypeEnum Action { get; init; }
        public int OrderIndex { get; init; }

        public GameActionDTO(int playerPosition, string playerName, StreetEnum street, ActionTypeEnum action, int orderIndex) {
            PlayerPosition = playerPosition;
            PlayerName = playerName;
            this.Street = street;
            this.Action = action;
            this.OrderIndex = orderIndex;
        }
    }
}
