using PokerPuzzleData.JSON;

namespace PokerPuzzleData.DTO
{
    // TODO - Split this class with a "CommunityDTO"
    // "CommunityDTO" will hold community cards and pot size at each street
    public class PokerGameDTO
    {
        public string Id { get; set; }
        public Dictionary<string, PlayerHandDTO> Players { get; set; }
        public CommunityDTO Community { get; set; }
        public List<GameActionDTO> GameActions { get; set; }
        public PokerGameDTO(PokerGameJSON json) {
            Community = new CommunityDTO(json);

            Players = new Dictionary<string, PlayerHandDTO>();
            foreach (var player in json.Players) {
                Players.Add(player.Key, new PlayerHandDTO(player.Value));
            }
            GameActions = json.BuildGameActions();
        }
    }
}
