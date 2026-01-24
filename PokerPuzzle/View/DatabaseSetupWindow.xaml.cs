using System.Windows;

namespace PokerPuzzle.View
{
    /// <summary>
    /// Logique d'interaction pour DatabaseSetup.xaml
    /// </summary>
    public partial class DatabaseSetupWindow : Window
    {
        public DatabaseSetupWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
            "Database was setup. Please restart app.",
            "Database setup",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
            this.Close();
        }
    }
}
