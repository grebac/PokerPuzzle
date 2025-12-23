using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PokerPuzzleData.Enum
{
    public enum StreetEnum {
        Preflop,
        Flop,
        Turn,
        River,
        Showdown
    }
    public static class StreetHelper
    {
        public static StreetEnum ParseStreet(string stage)
        {
            return stage switch
            {
                "p" => StreetEnum.Preflop,
                "f" => StreetEnum.Flop,
                "t" => StreetEnum.Turn,
                "r" => StreetEnum.River,
                _ => StreetEnum.Showdown
            };
        }
    }
}
