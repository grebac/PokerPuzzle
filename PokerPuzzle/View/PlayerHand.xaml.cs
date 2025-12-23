using PokerPuzzle.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokerPuzzle.View
{
    /// <summary>
    /// Logique d'interaction pour PlayerHand.xaml
    /// </summary>
    public partial class PlayerHand : UserControl
    {
        public PlayerHandVM ViewModel { get; set; }
        public PlayerHand()
        {
            InitializeComponent();
        }
    }
}
