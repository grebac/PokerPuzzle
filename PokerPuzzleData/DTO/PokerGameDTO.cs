using PokerPuzzleData.DB.Entity;
using PokerPuzzleData.JSON;

namespace PokerPuzzleData.DTO
{
    public class PokerGameDTO
    {
        public int GameId { get; set; }
        public bool IsFavorite { get; set; }
        public Dictionary<int, PlayerHandDTO> Players { get; set; }
        public CommunityDTO Community { get; set; }
        public List<GameActionDTO> GameActions { get; set; }
        public PokerGameDTO(int gameId, bool isFavorite, CommunityDTO community, Dictionary<int, PlayerHandDTO> players, List<GameActionDTO> actions) {
            GameId = gameId;
            IsFavorite = isFavorite;
            Community = community;
            Players = players;
            GameActions = actions;
        }
        public static PokerGameDTO FromEntity(GameEntity entity)
        {
            return new PokerGameDTO(entity.GameId,
                entity.isFavorite,
                CommunityDTO.FromEntity(entity.CommunityCards),
                   entity.Players
                    .ToDictionary(
                        p => p.Position,
                        PlayerHandDTO.FromEntity
                    ),
                    entity.Actions
                    .OrderBy(a => a.OrderIndex)
                    .Select(GameActionDTO.FromEntity)
                    .Skip(2) // Skips SB and BB
                    .ToList());
        }
    }
}
