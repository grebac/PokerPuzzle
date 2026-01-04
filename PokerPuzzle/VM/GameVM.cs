using PokerPuzzle.IO;
using PokerPuzzle.View;
using PokerPuzzleData.DB;
using PokerPuzzleData.DB.Repository;
using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;
using PokerPuzzleData.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace PokerPuzzle.VM
{
    public class GameVM : INotifyPropertyChanged
    {
        #region attributes
        private ObservableCollection<GameActionDTO> Actions { get; set; }
        private int _cursor;
        private int _gameId;
        private CommunityCardsVM _communityCards;
        private GameRepository _gameRepository;
        #endregion

        #region Properties
        public CommunityCardsVM CommunityCards { 
            get => _communityCards;   
            set { 
                _communityCards = value;
                OnPropertyChanged(nameof(CommunityCards));
            } 
        }
        #region Players
        public ObservableCollection<PlayerHandVM> Players { get; }
        #endregion
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
                if (_cursor >= Actions.Count) {
                    return Actions.Last().Street;
                }
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
            _gameRepository = new GameRepository(new PokerPuzzleContext());

            GetRandomGame();
        }

        #region GetGame
        public void SetupGame(int id)
        {
            var gameEntity = _gameRepository.GetGame(id);
            if (gameEntity == null) {
                return;
            }
            
            GameId = id;
            var pokerGameDTO = PokerGameDTO.FromEntity(gameEntity);

            SetupGame(pokerGameDTO);
        }

        public void SetupGame(PokerGameDTO pokerGameDTO) {
            // Get community card infos (Community cards and pot sizes)
            CommunityCards = new CommunityCardsVM(pokerGameDTO.Community);

            // Get player infos (Cards, pots and position)
            Players.Clear();
            foreach (PlayerHandDTO player in pokerGameDTO.Players.Values.OrderBy(p => p.Position))
            {
                Players.Add(new PlayerHandVM(player.PotSize, player.PocketCards[0], player.PocketCards[1], player.Position, "Unknown Player"));
            }

            // Get game actions (bet/check/fold/.. for each street)
            Actions = new ObservableCollection<GameActionDTO>(pokerGameDTO.GameActions);
            _cursor = 0;
        }

        private void GetRandomGame() {
            var game = _gameRepository.GetRandomGame();
            GameId = game.GameId;
            SetupGame(PokerGameDTO.FromEntity(game));
        }
        #endregion

        #region ControlButtons
        private void NextAction() {
            if (_cursor >= Actions.Count)
                return;

            var action = Actions[_cursor++];

            // StreetEnd action is a broadcasted event
            if (action.Action == ActionTypeEnum.StreetEnd) {
                SendEventNewStreet(action.Street + 1); // Send event for the next street
            }
            else {
                // Other event are player specific
                int playerPosition = action.PlayerPosition;
                Players[playerPosition - 1].ApplyAction(action.Action);
            }
        }

        private void PreviousAction() {
            if (_cursor < 1) {
                return;
            }

            var action = Actions[--_cursor];

            if (action.Action == ActionTypeEnum.StreetEnd)
            {
                SendEventRollbackStreet(action.Street);
            }
            else {
                // Update player if need be
                int playerPosition = action.PlayerPosition;
                Players[playerPosition - 1].UndoAction(action.Action);
            }
        }

        #region ControlButtonHelpers
        private void SendEventRollbackStreet(StreetEnum street) {
            // Show player eliminated at previous street
            foreach (var p in Players)
            {
                p.RollbackStreet(street);
            }

            // Update street
            CommunityCards.SetStreet(street);
        }

        private void SendEventNewStreet(StreetEnum street)
        {
            // Let players update their action based on the new street.
            foreach (var p in Players)
            {
                p.EnterStreet(street);
            }
            // Update the CommunityCards
            CommunityCards.SetStreet(street);
        }

        private void NextStreet() {
            if (CurrentStreet < StreetEnum.Showdown) { 
                GoToStreet(CurrentStreet + 1);
            }
        }

        private void PreviousStreet() {
            if (CurrentStreet > StreetEnum.Preflop) { 
                GoToStreet(CurrentStreet - 1);
            }
        }

        private void GoToStreet(StreetEnum street)
        {
            if (CurrentStreet == street) return;

            if(CurrentStreet < street) // If the street is greater than the current, we go forward.
            {
                // We execute actions until we get to the "street end" action of the previous street
                // If we want to access the Turn, we need to get to the Flops's end + 1 action
                while (_cursor < Actions.Count && !(IsStreetEndOf(street-1))) 
                {
                    NextAction();
                }
                NextAction(); // We execute the "street end" action
            } else
            {
                // We rollback actions until we havefind the "street end" of the previous street
                // If we want to access the Turn, we rollback until the very end of the Flop + 1 action
                while (_cursor > 0 && !(IsStreetEndOf(street-1)))
                {
                    PreviousAction();
                }
                NextAction(); // We re-execute said "street end"
            }
        }

        private bool IsStreetEndOf(StreetEnum street) {
            if (_cursor == Actions.Count) { // Protection in case the cursor is beyond the last action
                return false;
            }
            return Actions.ElementAt(_cursor).Action == ActionTypeEnum.StreetEnd && CurrentStreet == street;
        }
        #endregion
        #endregion

        #region FavoriteGames
        // Open Favorites window
        private void OpenFavoritesMethod()
        {
            var favoriteVM = new FavoriteGamesVM();
            var favoritesWindow = new FavoriteWindow(favoriteVM);

            // Show window
            favoritesWindow.ShowDialog();

            // TODO - What if user closes the window (does not want to select
            var selectedGame = favoriteVM.SelectedGame;
            if (selectedGame != null)
            {
                SetupGame(selectedGame.GameId);
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
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
