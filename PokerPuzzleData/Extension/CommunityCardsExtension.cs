using PokerPuzzleData.DB.Entity;
using PokerPuzzleData.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.Extension
{
    public static class CommunityCardsExtension
    {
        public static IReadOnlyList<CardRankSuit> ToList(this CommunityCardsEntity entity)
        {
            var cards = new List<CardRankSuit>(5);

            TryAdd(entity.Flop1);
            TryAdd(entity.Flop2);
            TryAdd(entity.Flop3);
            TryAdd(entity.Turn);
            TryAdd(entity.River);

            return cards;

            void TryAdd(string? code)
            {
                if (code != null)
                    cards.Add(CardRankSuit.Parse(code));
            }
        }
    }
}
