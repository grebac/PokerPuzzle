using PokerPuzzleData.DB.Entity;
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
        public CommunityDTO(List<CardsEnum> communityCards, Dictionary<StreetEnum, int> streetPots) { 
            CommunityCards = communityCards;
            StreetPots = streetPots;
        }
        public CommunityDTO(PokerGameJSON pokerGame) {
            CommunityCards = pokerGame.ParseBoard();
            StreetPots = pokerGame.ParseStreetPots();
        }
        public static CommunityDTO FromEntity(CommunityCardsEntity e)
        {
            var streetPots = new Dictionary<StreetEnum, int> {
                { StreetEnum.Preflop, 0 },
                { StreetEnum.Flop,    e.FlopPotSize },
                { StreetEnum.Turn,    e.TurnPotSize },
                { StreetEnum.River,   e.RiverPotSize },
                { StreetEnum.Showdown, e.ShowdownPotSize }
            };

            var cards = new[] {
                CardHelper.fromCodeToEnum(e.Flop1),
                CardHelper.fromCodeToEnum(e.Flop2),
                CardHelper.fromCodeToEnum(e.Flop3),
                CardHelper.fromCodeToEnum(e.Turn),
                CardHelper.fromCodeToEnum(e.River)
            }.Where(c => c != CardsEnum.CardBack)
            .ToList();

            return new CommunityDTO(cards, streetPots);
        }

    }
}
