using Microsoft.Win32;
using PokerPuzzleData.DB;
using PokerPuzzleData.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PokerPuzzle.VM
{
    public class DatabaseSetupVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand OpenDownloadPageCommand { get; }
        public ICommand SelectJsonFileCommand { get; }

        private Visibility _isImporting = Visibility.Hidden;
        public Visibility IsImporting
        {
            get => _isImporting;
            set { _isImporting = value; OnPropertyChanged(); }
        }

        private int _progressValue;
        public int ProgressValue
        {
            get => _progressValue;
            set { _progressValue = value; OnPropertyChanged(); }
        }

        private int _progressMaximum;
        public int ProgressMaximum
        {
            get => _progressMaximum;
            set { _progressMaximum = value; OnPropertyChanged(); }
        }


        private string _statusMessage = "Waiting for JSON file…";
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        private bool _canContinue;
        public bool CanContinue
        {
            get => _canContinue;
            set { _canContinue = value; OnPropertyChanged(); }
        }

        public DatabaseSetupVM()
        {
            OpenDownloadPageCommand = new RelayCommand(OpenDownloadPage);
            SelectJsonFileCommand = new RelayCommand(SelectJsonFile);
        }

        private void OpenDownloadPage()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/grebac/PokerPuzzle/releases/tag/data",
                UseShellExecute = true
            });
        }

        private async void SelectJsonFile()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Select poker hand history file"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                IsImporting = Visibility.Visible;
                StatusMessage = "Importing data, this may take a few minutes…";


                var filepath = dialog.FileName;
                var importService = new GameImportService(filepath);

                var progress = new Progress<ImportProgress>(p =>
                {
                    ProgressMaximum = p.Total;
                    ProgressValue = p.Processed;
                    
                    string phaseText = p.Phase switch
                    {
                        ImportPhaseEnum.ImportGames => "Importing hands",
                        ImportPhaseEnum.AnalayseGames => "Analyzing hands",
                        _ => "Working…"
                    };

                    StatusMessage = $"{phaseText}: {p.Processed:N0} / {p.Total:N0}";
                });

                await Task.Run(() =>
                    importService.EnsureDatabaseReady(progress));

                StatusMessage = "Database ready ✔";
                CanContinue = true;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsImporting = Visibility.Hidden;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
