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
        public FavoriteWindow(FavoriteGamesVM vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}