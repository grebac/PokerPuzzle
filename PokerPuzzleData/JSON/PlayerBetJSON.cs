using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PokerPuzzleData.JSON
{
    public class PlayerBetJSON
    {
        [JsonPropertyName("actions")]
        public string Actions { get; set; } = "";

        [JsonPropertyName("stage")]
        public string Stage { get; set; } = "";
    }
}
