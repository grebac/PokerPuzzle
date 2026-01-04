using PokerPuzzleData.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PokerPuzzle.VM
{
    public class GameSummaryVM
    {
        public int GameId { get; }

        public int PlayerCount { get; }
        public int FinalPotSize { get; }

        public string StreetsReached =>
            ReachedRiver ? "River" :
            ReachedTurn ? "Turn" :
            ReachedFlop ? "Flop" :
            "Preflop";

        public string FlopUnicode { get; }
        public ObservableCollection<CardsEnum> FlopCards { get; }

        public GameSummaryVM(GameSummaryDTO dto)
        {
            GameId = dto.GameId;
            PlayerCount = dto.PlayerCount;
            FinalPotSize = dto.FinalPotSize;

            ReachedFlop = dto.ReachedFlop;
            ReachedTurn = dto.ReachedTurn;
            ReachedRiver = dto.ReachedRiver;

            FlopCards = new(dto.CommunityCards);

            FlopUnicode = dto.CommunityCards?.Count == 3
            ? string.Join(" ", dto.CommunityCards.Select(CardHelper.ToUnicode))
            : "-";
        }

        private bool ReachedFlop;
        private bool ReachedTurn;
        private bool ReachedRiver;
    }

}
