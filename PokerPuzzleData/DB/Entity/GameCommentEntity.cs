using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.DB.Entity
{
    public class GameCommentEntity
    {
        public int Id { get; set; }

        public int GameId { get; set; }
        public GameEntity Game { get; set; } = null!;

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
