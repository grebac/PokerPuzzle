using PokerPuzzle.VM;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour GameSelect.xaml
    /// </summary>
    public partial class GameSelect : Window
    {
        private bool closedBySelection = false;
        public GameSelect(GameSelectionVM vm)
        {
            this.DataContext = vm;
            InitializeComponent();
        }

        private void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            closedBySelection = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!closedBySelection && DataContext is GameSelectionVM vm)
            {
                vm.SelectedGame = null;
            }
        }
    }
}
