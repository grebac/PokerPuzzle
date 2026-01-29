using PokerPuzzleData.DB.Entity;
using PokerPuzzleData.JSON;

namespace PokerPuzzleData.DTO
{
    public class PlayerHandDTO
    {
        public List<CardsEnum> PocketCards { get; set; }
        public int Position { get; set; }
        public double PotSize { get; set; }
        public PlayerHandDTO(int position, double potSize, List<CardsEnum> pocketCards) { 
            Position = position;
            PocketCards = pocketCards;
            PotSize = potSize;
        }
        public static PlayerHandDTO FromEntity(PlayerEntity e)
        {
            return new PlayerHandDTO(e.Position, e.Stack
                , [CardHelper.fromCodeToEnum(e.Card1), CardHelper.fromCodeToEnum(e.Card2)]);
        }

    }
}
