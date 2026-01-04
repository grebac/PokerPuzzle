using PokerPuzzleData.DB.Entity;
using PokerPuzzleData.JSON;

namespace PokerPuzzleData.DTO
{
    public class PokerGameDTO
    {
        public int GameId { get; set; }
        public Dictionary<string, PlayerHandDTO> Players { get; set; } // TODO - Remove the string it is an int. Currently I quick fixed it by using GameId.toString()
        public CommunityDTO Community { get; set; }
        public List<GameActionDTO> GameActions { get; set; }
        public PokerGameDTO(int gameId, CommunityDTO community, Dictionary<string, PlayerHandDTO> players, List<GameActionDTO> actions) {
            GameId = gameId;
            Community = community;
            Players = players;
            GameActions = actions;
        }
        public PokerGameDTO(PokerGameJSON json) {
            Community = new CommunityDTO(json);

            Players = new Dictionary<string, PlayerHandDTO>();
            foreach (var player in json.Players) {
                Players.Add(player.Key, new PlayerHandDTO(player.Value));
            }
            GameActions = json.BuildGameActions();
        }
        public static PokerGameDTO FromEntity(GameEntity entity)
        {
            return new PokerGameDTO(entity.GameId, 
                CommunityDTO.FromEntity(entity.CommunityCards),
                   entity.Players
                    .ToDictionary(
                        p => p.Position.ToString(),
                        PlayerHandDTO.FromEntity
                    ),
                    entity.Actions
                    .OrderBy(a => a.OrderIndex)
                    .Select(GameActionDTO.FromEntity)
                    .ToList());
        }
    }
}
