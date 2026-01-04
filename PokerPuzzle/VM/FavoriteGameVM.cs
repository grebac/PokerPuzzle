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
        public GameSummaryVM SelectedGame { get; set; }

        public ICommand LoadGameCommand { get; }
        public ICommand RemoveFavoriteCommand { get; }

        public FavoriteGamesVM()
        {
            LoadGameCommand = new RelayCommand<GameSummaryVM>(LoadGame);
            RemoveFavoriteCommand = new RelayCommand<GameSummaryVM>(RemoveGame);

            var favorites = FavoritesGameHelper.LoadFavorites();
            var favoritesGameIds = favorites.Keys.ToList();

            GameRepository repo = new GameRepository(new PokerPuzzleContext());
            var summaries = repo.GetGameSummaries(favoritesGameIds);

            foreach (var summary in summaries)
                Games.Add(new GameSummaryVM(summary));
        }

        private void LoadGame(GameSummaryVM game) {
            // TODO - If double click on game, you select it and close the window
        }

        private void RemoveGame(GameSummaryVM game)
        {
            FavoritesGameHelper.RemoveFavorite(game.GameId);
        }
    }

}
