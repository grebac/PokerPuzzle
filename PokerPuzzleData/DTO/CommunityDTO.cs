using PokerPuzzleData.Enum;
using PokerPuzzleData.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.DTO
{
    public class CommunityDTO
    {
        public List<CardsEnum> CommunityCards { get; set; }
        public Dictionary<StreetEnum, int> StreetPots { get; set; }
        public CommunityDTO(PokerGameJSON pokerGame) {
            CommunityCards = pokerGame.ParseBoard();
            StreetPots = pokerGame.ParseStreetPots();
        }
    }
}
