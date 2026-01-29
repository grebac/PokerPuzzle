using PokerPuzzleData.DB.Repository;
using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PokerPuzzle.VM
{
    public class GameSummaryVM
    {
        static private GameRepository rep = new GameRepository();
        private string _comment;
        public int GameId { get; }

        public int PlayerCount { get; }
        public int FinalPotSize { get; }
        public string Comment { 
            get => _comment; 
            set {
                if (_comment == value) return;

                _comment = value;
                
                rep.updateComment(GameId, _comment);
            } 
        }

        public string StreetsReached =>
            ReachedRiver ? "River" :
            ReachedTurn ? "Turn" :
            ReachedFlop ? "Flop" :
            "Preflop";
        public BoardTexture BoardTexture { get; }
        
        public ObservableCollection<CardsEnum> FlopCards { get; }

        public GameSummaryVM(GameSummaryDTO dto)
        {
            GameId = dto.GameId;
            PlayerCount = dto.PlayerCount;
            FinalPotSize = dto.FinalPotSize;
            Comment = dto.Comment ?? "";

            ReachedFlop = dto.ReachedFlop;
            ReachedTurn = dto.ReachedTurn;
            ReachedRiver = dto.ReachedRiver;

            FlopCards = new(dto.CommunityCards);

            BoardTexture = dto.BoardTexture;
        }

        private bool ReachedFlop;
        private bool ReachedTurn;
        private bool ReachedRiver;
    }

}
