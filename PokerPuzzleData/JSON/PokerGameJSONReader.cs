using PokerPuzzleData.JSON;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PokerPuzzleData.Script
{
    public static class PokerGameJSONReader
    {
        public static List<PokerGameJSON> readPokerGameJSON(string path) {
            return JsonSerializer.Deserialize<List<PokerGameJSON>>(
                    File.ReadAllText(path)
            );
        }
    }
}
