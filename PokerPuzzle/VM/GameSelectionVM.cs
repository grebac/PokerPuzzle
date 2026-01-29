using PokerPuzzleData.DB.Repository;
using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace PokerPuzzle.VM
{
    public class GameSelectionVM
    {
        #region Properties
        public ObservableCollection<GameSummaryVM> FilteredGames { get; } = new();
        public GameSummaryVM? SelectedGame { get; set; }

        // -- Handle game pagination
        private const int _pageSize = 8;
        private int _pageNumber = 0;
        public int PageNumber { get => _pageNumber; 
            set 
            {
                _pageNumber = value;
                OnPropertyChanged(nameof(PageNumber));
            } 
        }

        public int MaxPage { get => (int)Math.Ceiling((double)MatchCount / _pageSize); }

        // -- Match Count  TODO - Fix the binding
        private int _matchCount = 0;
        public int MatchCount { 
            get => _matchCount; 
            set { 
                _matchCount = value; 
                OnPropertyChanged(nameof(MatchCount));
                OnPropertyChanged(nameof(MaxPage));
            } 
        }

        // -- CRITERIAS
        private BoardTexture _selectedFilter;
        public BoardTexture SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
            }
        }
        // -- RANK CRITERIA
        public bool IsHighFlopSelected { get; set; }
        public bool IsMiddleFlopSelected { get; set; }
        public bool IsLowFlopSelected { get; set; }

        public bool IsConnectedFlopSelected { get; set; }

        // -- WET / DRY
        public bool IsWetFlopSelected { get; set; }
        public bool IsDryFlopSelected { get; set; }

        // -- FLUSH
        public bool IsRainbowFlopSelected { get; set; }
        public bool IsDuoFlopSelected { get; set; }
        public bool IsMonoFlopSelected { get; set; }

        // -- FULL BOARD CRITERIA
        public bool IsFlushThreatSelected { get; set; }
        public bool IsStraightThreatSelected { get; set; }
        public bool IsSetThreatSelected { get; set; }

        // -- ICommands
        public ICommand ApplyFiltersCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        #endregion

        public GameSelectionVM()
        {
            ApplyFiltersCommand = new RelayCommand(ApplyFilters);
            NextPageCommand = new RelayCommand(
                () => ApplyFilters(),
                () => nextPage());
            PreviousPageCommand = new RelayCommand(
                () => ApplyFilters(),
                () => previousPage());
        }

        private void ApplyFilters() {
            buildFilter();
            GameRepository repo = new GameRepository();

            var summaries = repo.GetGameSummaryPage(_pageSize, _pageNumber, SelectedFilter);
            fillGameList(summaries);

            MatchCount = repo.GetGameSummaryCount(SelectedFilter);
        }

        private void fillGameList(IList<GameSummaryDTO> games) {
            FilteredGames.Clear();
            foreach (var summary in games)
            {
                FilteredGames.Add(new GameSummaryVM(summary));
            }
        }

        private void buildFilter() {
            BoardTexture filter = BoardTexture.None;

            // Rank
            if (IsHighFlopSelected) filter |= BoardTexture.HighFlop;
            if (IsMiddleFlopSelected) filter |= BoardTexture.MiddleFlop;
            if (IsLowFlopSelected) filter |= BoardTexture.LowFlop;

            // Connected
            if (IsConnectedFlopSelected)
                filter |= BoardTexture.ConnectedFlop;

            // Wet / Dry
            if (IsWetFlopSelected) filter |= BoardTexture.WetFlop;
            if (IsDryFlopSelected) filter |= BoardTexture.DryFlop;

            // Flush texture
            if (IsRainbowFlopSelected) filter |= BoardTexture.RainbowFlop;
            if (IsDuoFlopSelected) filter |= BoardTexture.TwoToneFlop;
            if (IsMonoFlopSelected) filter |= BoardTexture.MonoFlop;

            // Threats
            if (IsFlushThreatSelected) filter |= BoardTexture.FlushPossible;
            if (IsStraightThreatSelected) filter |= BoardTexture.StraightPossible;
            if (IsSetThreatSelected) filter |= BoardTexture.FullHousePossible;

            SelectedFilter = filter;
        }

        private bool nextPage() {
            if(_pageNumber + 1 <= MaxPage) {
                _pageNumber++;
                return true;
            }
            return false;
        }

        private bool previousPage()
        {
            if (_pageNumber - 1 >= 0)
            {
                _pageNumber--;
                return true;
            }
            return false;
        }

        #region Notify
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
