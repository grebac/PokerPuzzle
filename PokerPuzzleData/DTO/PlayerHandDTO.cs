using PokerPuzzleData.JSON;

namespace PokerPuzzleData.DTO
{
    public class PlayerHandDTO
    {
        public List<CardsEnum> PocketCards { get; set; }
        public int Position { get; set; }
        public int PotSize { get; set; }

        public PlayerHandDTO(PlayerHandJSON json) {
            PocketCards = new List<CardsEnum>();
            if (json.PocketCards.Count == 0)
            {
                PocketCards.Add(CardsEnum.CardBack);
                PocketCards.Add(CardsEnum.CardBack);
            }
            else
            {
                PocketCards.Add(CardHelper.fromCodeToEnum(json.PocketCards[0]));
                PocketCards.Add(CardHelper.fromCodeToEnum(json.PocketCards[1]));
            }
            Position = json.Position;
            PotSize = json.PotSize;
        }
    }
}
