using PokerPuzzleData.DB.Entity;
using PokerPuzzleData.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PokerPuzzleData.DTO
{
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
        public static GameActionDTO FromEntity(ActionEntity e)
        {
            return new GameActionDTO(
                playerPosition: e.PlayerPosition,
                street: e.Street,
                action: e.ActionType,
                orderIndex: e.OrderIndex
            );
        }

    }
}
