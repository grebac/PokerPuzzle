using PokerPuzzleData.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.DTO
{
    public class GameSummaryDTO
    {
        public int GameId { get; }

        public int PlayerCount { get; }
        public int FinalPotSize { get; }

        public bool ReachedFlop { get; }
        public bool ReachedTurn { get; }
        public bool ReachedRiver { get; }

        public List<CardsEnum> CommunityCards { get; }

        public BoardTexture BoardTexture { get; init; }

        //public bool IsHighFlop => BoardTexture.HasFlag(BoardTexture.HighFlop);
        //public bool IsWet => BoardTexture.HasFlag(BoardTexture.WetFlop);
        //public bool IsConnected => BoardTexture.HasFlag(BoardTexture.ConnectedFlop);

        public string? Comment { get; set; }

        public GameSummaryDTO(
            int gameId,
            int playerCount,
            int finalPotSize,
            bool reachedFlop,
            bool reachedTurn,
            bool reachedRiver,
            List<CardsEnum> communityCards,
            BoardTexture boardTexture,
            string? comment)
        {
            GameId = gameId;
            PlayerCount = playerCount;
            FinalPotSize = finalPotSize;
            ReachedFlop = reachedFlop;
            ReachedTurn = reachedTurn;
            ReachedRiver = reachedRiver;
            CommunityCards = communityCards;
            BoardTexture = boardTexture;
            Comment = comment;
        }
    }

}
