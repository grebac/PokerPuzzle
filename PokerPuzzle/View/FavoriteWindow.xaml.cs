using PokerPuzzle.VM;
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
        private bool closedBySelection = false;
        public FavoriteWindow(FavoriteGamesVM vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            closedBySelection = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!closedBySelection && DataContext is FavoriteGamesVM vm)
            {
                vm.SelectedGame = null;
            }
        }
    }
}