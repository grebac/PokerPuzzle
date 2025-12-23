using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PokerPuzzleData.JSON
{
    public class PlayerHandJSON
    {
        [JsonPropertyName("pocket_cards")]
        public List<string> PocketCards { get; set; }
        [JsonPropertyName("position")]
        public int Position { get; set; }
        [JsonPropertyName("bets")]
        public List<PlayerBetJSON> Bets { get; set; } = [];
        [JsonPropertyName("bankroll")]
        public int PotSize { get; set; }
    }
}
