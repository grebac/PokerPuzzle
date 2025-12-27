using PokerPuzzle.IO;
using PokerPuzzle.View;
using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;
using PokerPuzzleData.JSON;
using PokerPuzzleData.Script;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;

namespace PokerPuzzle.VM
{
    public class GameVM : INotifyPropertyChanged
    {
        #region attributes
        private List<PokerGameJSON> _games;
        private IReadOnlyList<GameActionDTO> Actions { get; set; }
        private int _cursor;
        private int _gameId;
        private CommunityCardsVM _communityCards;
        #endregion

        #region Properties
        public CommunityCardsVM CommunityCards { 
            get => _communityCards;   
            set { 
                _communityCards = value;
                OnPropertyChanged(nameof(CommunityCards));
            } 
        }
        public ObservableCollection<PlayerHandVM> Players { get; }

        public ICommand PreflopCommand { get; }
        public ICommand FlopCommand { get; }
        public ICommand TurnCommand { get; }
        public ICommand RiverCommand { get; }
        public ICommand ShowdownCommand { get; }
        public ICommand NextActionCommand { get; }
        public ICommand NextStreetCommand { get; }
        public ICommand PreviousActionCommand { get; }
        public ICommand PreviousStreetCommand { get; }
        public ICommand RandomGame { get; }
        public StreetEnum CurrentStreet { // TODO: Better fix. This property exists only for the "GoToStreet" function. 
            get
            {
                if (_cursor >= Actions.Count)
                    return Actions.Last().Street;
                return Actions[_cursor].Street;
            } 
        }
        public int GameId { get { return _gameId; } set { _gameId = value; OnPropertyChanged(nameof(GameId)); } }
        public ICommand OpenFavorites {  get; }
        public ICommand AddToFavorties { get; }
        #endregion

        public GameVM() {
            // Commands Databinding
            PreflopCommand = new RelayCommand<StreetEnum>(GoToStreet);
            FlopCommand = new RelayCommand<StreetEnum>(GoToStreet);
            TurnCommand = new RelayCommand<StreetEnum>(GoToStreet);
            RiverCommand = new RelayCommand<StreetEnum>(GoToStreet);
            ShowdownCommand = new RelayCommand<StreetEnum>(GoToStreet);
            NextActionCommand = new RelayCommand(NextAction);
            NextStreetCommand = new RelayCommand(NextStreet);
            PreviousActionCommand = new RelayCommand(PreviousAction);
            PreviousStreetCommand = new RelayCommand(PreviousStreet);
            RandomGame = new RelayCommand(GetRandomGame);
            OpenFavorites = new RelayCommand(OpenFavoritesMethod);
            AddToFavorties = new RelayCommand(AddToFavorites);
            Players = new ObservableCollection<PlayerHandVM>();

            // Setup game
            GetRandomGame();
        }

        #region logic
        public void SetupGame(int id)
        {
            // Read data from file or DB
            if (_games == null)
                LoadGames();

            // Choose random element
            GameId = id;
            var pokerGameDTO = new PokerGameDTO(_games[GameId]);

            // Get community card infos (Community cards and pot sizes)
            CommunityCards = new CommunityCardsVM(pokerGameDTO.Community);

            // Get player infos (Cards, pots and position)
            Players.Clear();
            foreach (PlayerHandDTO player in pokerGameDTO.Players.Values.OrderBy(p => p.Position))
            {
                Players.Add(new PlayerHandVM(player.PotSize, player.PocketCards[0], player.PocketCards[1], player.Position, "Unknown Player"));
            }

            // Get game actions (bet/check/fold/.. for each street)
            Actions = pokerGameDTO.GameActions;
            _cursor = 0;
        }

        private void GetRandomGame() {
            // Read data from file or DB
            if (_games == null)
                LoadGames();

            var id = RandomNumberGenerator.GetInt32(_games.Count);
            SetupGame(id);
        }
        #endregion

        #region LoadData
        private void LoadGames()
        {
            var path = System.IO.Path.Combine(
                AppContext.BaseDirectory,
                "Ressources",
                "Data",
                "hands.json");
            _games = PokerGameJSONReader.readPokerGameJSON(path).Where(game => AtLeastTwoPlayersRevealed(game)).ToList();
        }

        private static bool AtLeastTwoPlayersRevealed(PokerGameJSON game)
        {
            int revealedCount = game.Players.Values
                .Count(player => player.PocketCards != null &&
                                player.PocketCards.Count == 2);

            return revealedCount >= 2;
        }
        #endregion

        #region ControlButtons
        public void NextAction() {
            if (_cursor >= Actions.Count)
                return;

            var action = Actions[_cursor++];

            // Special update in case of StreetEnd
            if (action.Action == ActionTypeEnum.StreetEnd) {
                // Let players update their action based on the new street.
                foreach (var p in Players)
                {
                    p.EnterStreet(action.Street);
                }
                // Update the CommunityCards
                CommunityCards.SetStreet(action.Street);
            }
            else
            {
                // Update player if need be
                int playerPosition = action.PlayerPosition;
                Players[playerPosition - 1].ApplyAction(action.Action);
            }                
        }

        public void PreviousAction() {
            if (_cursor < 1)
                return;

            var action = Actions[--_cursor];

            if (action.Action == ActionTypeEnum.StreetEnd)
            {
                // Show player eliminated at previous street
                foreach (var p in Players)
                {
                    p.RollbackStreet(action.Street);
                }

                // Update street
                CommunityCards.SetStreet(action.Street-1);
            }
            else {
                // Update player if need be
                int playerPosition = action.PlayerPosition;
                Players[playerPosition - 1].UndoAction(action.Action);
            }
        }

        private void NextStreet() {
            if (CurrentStreet < StreetEnum.Showdown)
                GoToStreet(CurrentStreet + 1);
        }

        private void PreviousStreet() {
            if(CurrentStreet > StreetEnum.Preflop)
                GoToStreet(CurrentStreet - 1);
        }

        private void GoToStreet(StreetEnum street)
        {
            if (CurrentStreet == street) return;

            if(CurrentStreet < street) // If the street is greater than the current, we go forward.
            {
                // We execute actions until we get to the "street end" action
                while (_cursor <= Actions.Count && !(Actions[_cursor].Action == ActionTypeEnum.StreetEnd && CurrentStreet == street)) 
                {
                    NextAction();
                }
                NextAction(); // We execute the "street end" action
            } else
            {
                // We rollback actions until we have executed "street end"
                while (_cursor > 0 && !(CurrentStreet == street && Actions[_cursor].Action == ActionTypeEnum.StreetEnd))
                {
                    PreviousAction();
                }
                NextAction(); // We re-execute said "street end"
            }
        }
        #endregion

        #region FavoriteGames
        // Ouvrir la fenêtre des favoris
        private void OpenFavoritesMethod()
        {
            var favorites = FavoritesGameHelper.LoadFavorites();
            var favoritesWindow = new FavoriteWindow(favorites);

            if (favoritesWindow.ShowDialog() == true)
            {
                // L'utilisateur a double-cliqué sur une partie
                var selectedIndex = favoritesWindow.SelectedGameIndex;
                if (selectedIndex.HasValue)
                {
                    SetupGame(selectedIndex.Value);
                }

                // Sauvegarder les changements (suppressions/éditions)
                var updatedFavorites = favoritesWindow.GetUpdatedFavorites();
                FavoritesGameHelper.SaveFavorites(updatedFavorites);
            }
        }

        private void AddToFavorites()
        {
            if (GameId < 0)
            {
                MessageBox.Show("No game loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            FavoritesGameHelper.AddFavorite(GameId, "No Comment yet");
            MessageBox.Show("Game added to favorites!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Notify
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
