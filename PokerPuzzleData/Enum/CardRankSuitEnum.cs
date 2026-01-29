using PokerPuzzleData.DB.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.Enum
{
    public enum Rank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public readonly struct CardRankSuit
    {
        public Rank Rank { get; }
        public Suit Suit { get; }
        public CardRankSuit(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public static CardRankSuit Parse(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length < 2)
                throw new ArgumentException($"Invalid card code: '{code}'");

            Rank rank = code[0] switch
            {
                '2' => Rank.Two,
                '3' => Rank.Three,
                '4' => Rank.Four,
                '5' => Rank.Five,
                '6' => Rank.Six,
                '7' => Rank.Seven,
                '8' => Rank.Eight,
                '9' => Rank.Nine,
                'T' => Rank.Ten,
                'J' => Rank.Jack,
                'Q' => Rank.Queen,
                'K' => Rank.King,
                'A' => Rank.Ace,
                _ => throw new ArgumentException($"Unknown rank '{code[0]}'")
            };

            Suit suit = code[1] switch
            {
                'c' => Suit.Clubs,
                'd' => Suit.Diamonds,
                'h' => Suit.Hearts,
                's' => Suit.Spades,
                _ => throw new ArgumentException($"Unknown suit '{code[1]}'")
            };

            return new CardRankSuit(rank, suit);
        }
        public static IReadOnlyList<CardRankSuit> ParseMany(IEnumerable<string> codes)
        => codes.Select(Parse).ToList();
    }
}
