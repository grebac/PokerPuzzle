using PokerPuzzle.View;
using PokerPuzzleData.DB;
using PokerPuzzleData.Service;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace PokerPuzzle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        override protected void OnStartup(StartupEventArgs e){
            base.OnStartup(e);

            try {
                GameImportService service = new GameImportService();
                service.EnsureDatabaseReady();
            }
            catch (FileNotFoundException) {
                // If an error occured, the user probably does not have the JSON data file
                // A window pops-up to guide the user toward downloading the JSON data file
                DatabaseSetupWindow databaseSetupWindow = new DatabaseSetupWindow();
                databaseSetupWindow.ShowDialog();
            }
            catch (Exception ex) {
                // Any other occasionnal exception
                MessageBox.Show($"Unexpected error during Database loading: {ex.Message}");
                Application.Current.Shutdown();
            }
        }
    }
}
