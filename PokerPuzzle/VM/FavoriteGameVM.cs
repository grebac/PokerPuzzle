using PokerPuzzle.IO;
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

            var favorites = FavoritesGameHelper.LoadFavorites();
            var favoritesGameIds = favorites.Keys.ToList();

            GameRepository repo = new GameRepository();
            var summaries = repo.GetGameSummaries(favoritesGameIds);

            foreach (var summary in summaries)
                Games.Add(new GameSummaryVM(summary));
        }

        private void RemoveGame(GameSummaryVM game) {
            FavoritesGameHelper.RemoveFavorite(game.GameId);
        }
    }

}
