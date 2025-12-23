using System;
using System.Reflection;
using System.ComponentModel;

// TODO - This CardsEnum file is copied from the main projet. There could technically be a split between theses.
// There's also a UI component with the image path, while this is a library project.
namespace PokerPuzzleData.DTO
{
    // Custom Attribute to match the data's code to C#'s enum
    [AttributeUsage(AttributeTargets.Field)]
    public class CardCodeAttribute : Attribute
    {
        public string Code { get; }
        public CardCodeAttribute(string code)
        {
            Code = code;
        }
    }

    public enum CardsEnum
    {
        // Clubs
        [Description("pack://application:,,,/Ressources/Cards/ace_of_clubs.png")]
        [CardCode("Ac")]
        AceOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/2_of_clubs.png")]
        [CardCode("2c")]
        TwoOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/3_of_clubs.png")]
        [CardCode("3c")]
        ThreeOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/4_of_clubs.png")]
        [CardCode("4c")]
        FourOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/5_of_clubs.png")]
        [CardCode("5c")]
        FiveOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/6_of_clubs.png")]
        [CardCode("6c")]
        SixOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/7_of_clubs.png")]
        [CardCode("7c")]
        SevenOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/8_of_clubs.png")]
        [CardCode("8c")]
        EightOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/9_of_clubs.png")]
        [CardCode("9c")]
        NineOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/10_of_clubs.png")]
        [CardCode("10c")]
        TenOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/jack_of_clubs.png")]
        [CardCode("Jc")]
        JackOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/queen_of_clubs.png")]
        [CardCode("Qc")]
        QueenOfClubs,
        [Description("pack://application:,,,/Ressources/Cards/king_of_clubs.png")]
        [CardCode("Kc")]
        KingOfClubs,

        // Diamonds
        [Description("pack://application:,,,/Ressources/Cards/ace_of_diamonds.png")]
        [CardCode("Ad")]
        AceOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/2_of_diamonds.png")]
        [CardCode("2d")]
        TwoOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/3_of_diamonds.png")]
        [CardCode("3d")]
        ThreeOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/4_of_diamonds.png")]
        [CardCode("4d")]
        FourOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/5_of_diamonds.png")]
        [CardCode("5d")]
        FiveOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/6_of_diamonds.png")]
        [CardCode("6d")]
        SixOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/7_of_diamonds.png")]
        [CardCode("7d")]
        SevenOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/8_of_diamonds.png")]
        [CardCode("8d")]
        EightOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/9_of_diamonds.png")]
        [CardCode("9d")]
        NineOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/10_of_diamonds.png")]
        [CardCode("10d")]
        TenOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/jack_of_diamonds.png")]
        [CardCode("Jd")]
        JackOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/queen_of_diamonds.png")]
        [CardCode("Qd")]
        QueenOfDiamonds,
        [Description("pack://application:,,,/Ressources/Cards/king_of_diamonds.png")]
        [CardCode("Kd")]
        KingOfDiamonds,

        // Hearts
        [Description("pack://application:,,,/Ressources/Cards/ace_of_hearts.png")]
        [CardCode("Ah")]
        AceOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/2_of_hearts.png")]
        [CardCode("2h")]
        TwoOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/3_of_hearts.png")]
        [CardCode("3h")]
        ThreeOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/4_of_hearts.png")]
        [CardCode("4h")]
        FourOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/5_of_hearts.png")]
        [CardCode("5h")]
        FiveOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/6_of_hearts.png")]
        [CardCode("6h")]
        SixOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/7_of_hearts.png")]
        [CardCode("7h")]
        SevenOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/8_of_hearts.png")]
        [CardCode("8h")]
        EightOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/9_of_hearts.png")]
        [CardCode("9h")]
        NineOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/10_of_hearts.png")]
        [CardCode("10h")]
        TenOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/jack_of_hearts.png")]
        [CardCode("Jh")]
        JackOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/queen_of_hearts.png")]
        [CardCode("Qh")]
        QueenOfHearts,
        [Description("pack://application:,,,/Ressources/Cards/king_of_hearts.png")]
        [CardCode("Kh")]
        KingOfHearts,

        // Spades
        [Description("pack://application:,,,/Ressources/Cards/ace_of_spades.png")]
        [CardCode("As")]
        AceOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/2_of_spades.png")]
        [CardCode("2s")]
        TwoOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/3_of_spades.png")]
        [CardCode("3s")]
        ThreeOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/4_of_spades.png")]
        [CardCode("4s")]
        FourOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/5_of_spades.png")]
        [CardCode("5s")]
        FiveOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/6_of_spades.png")]
        [CardCode("6s")]
        SixOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/7_of_spades.png")]
        [CardCode("7s")]
        SevenOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/8_of_spades.png")]
        [CardCode("8s")]
        EightOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/9_of_spades.png")]
        [CardCode("9s")]
        NineOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/10_of_spades.png")]
        [CardCode("10s")]
        TenOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/jack_of_spades.png")]
        [CardCode("Js")]
        JackOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/queen_of_spades.png")]
        [CardCode("Qs")]
        QueenOfSpades,
        [Description("pack://application:,,,/Ressources/Cards/king_of_spades.png")]
        [CardCode("Ks")]
        KingOfSpades,

        [Description("pack://application:,,,/Ressources/Cards/card_back.png")]
        CardBack
    }
    public static class CardHelper
    {
        public static string ToImagePath(this CardsEnum card)
        {
            var field = card.GetType().GetField(card.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? "pack://application:,,,/Ressources/card_back.png";
        }

        // Converts a card code to a card enum using the CardCode attribute
        public static CardsEnum fromCodeToEnum(string code) {
            foreach (CardsEnum card in System.Enum.GetValues(typeof(CardsEnum))) {
                var attribute = card.GetType().GetField(card.ToString())?.GetCustomAttribute<CardCodeAttribute>();
                if (attribute?.Code == code)
                    return card;
            }
            return CardsEnum.CardBack;
        }
    }
}
