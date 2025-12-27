using PokerPuzzleData.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PokerPuzzle.View
{
    /// <summary>
    /// Logique d'interaction pour FavoriteWindow.xaml
    /// </summary>
    public partial class FavoriteWindow : Window
    {
        public int? SelectedGameIndex { get; private set; }
        private ObservableCollection<FavoriteEntry> _favoritesList;

        public FavoriteWindow(Dictionary<int, string> favorites)
        {
            InitializeComponent();

            // Convert to list of mutable objects
            _favoritesList = new ObservableCollection<FavoriteEntry>(
                favorites
                    .OrderBy(f => f.Key)
                    .Select(f => new FavoriteEntry(f.Key, f.Value))
            );

            FavoritesGrid.ItemsSource = _favoritesList;
        }

        public Dictionary<int, string> GetUpdatedFavorites()
        {
            // Reconstruct dictionary from grid
            return _favoritesList.ToDictionary(
                entry => entry.GameIndex,
                entry => entry.Comment
            );
        }

        private void FavoritesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FavoritesGrid.SelectedItem is FavoriteEntry entry)
            {
                SelectedGameIndex = entry.GameIndex;
                DialogResult = true;
                Close();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (FavoritesGrid.SelectedItem is FavoriteEntry entry)
            {
                _favoritesList.Remove(entry);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

public class FavoriteEntry
{
    public int GameIndex { get; set; }
    public string Comment { get; set; }

    public FavoriteEntry(int gameIndex, string comment)
    {
        GameIndex = gameIndex;
        Comment = comment;
    }
}
