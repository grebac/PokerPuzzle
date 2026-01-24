using PokerPuzzleData.DB;
using PokerPuzzleData.DB.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace PokerPuzzle.VM
{
    public class FavoriteGamesVM
    {
        public ObservableCollection<GameSummaryVM> Games { get; } = new();
        public GameSummaryVM? SelectedGame { get; set; }

        public ICommand RemoveFavoriteCommand { get; }

        public FavoriteGamesVM()
        {
            RemoveFavoriteCommand = new RelayCommand<GameSummaryVM>(RemoveGame);

            GameRepository repo = new GameRepository();
            var games = repo.GetFavoriteGames();
            var summaries = repo.GetGameSummaries(games.Select(g => g.GameId).ToList());

            foreach (var summary in summaries)
                Games.Add(new GameSummaryVM(summary));
        }

        private void RemoveGame(GameSummaryVM game) {
            GameRepository repo = new GameRepository();
            repo.setFavorite(game.GameId, false);
        }
    }

}
