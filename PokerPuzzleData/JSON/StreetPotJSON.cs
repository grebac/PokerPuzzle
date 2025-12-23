using PokerPuzzleData.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PokerPuzzleData.JSON
{
    public class StreetPotJSON
    {
        [JsonPropertyName("size")]
        public int Size {  get; set; }
        [JsonPropertyName("stage")]
        public string Street { get; set; } = "";
    }
}
