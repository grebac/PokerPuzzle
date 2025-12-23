using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;
using PokerPuzzleData.Script;
using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;

namespace PokerPuzzle.VM
{
    public class GameVM : INotifyPropertyChanged
    {
        #region attributes
        private IReadOnlyList<GameActionDTO> Actions { get; set; }
        private int _cursor = 0;
        private StreetEnum _currentStreet = StreetEnum.Preflop;
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
        public ICommand PreviousActionCommand { get; }
        #endregion

        public GameVM() {
            // Commands Databinding
            PreflopCommand = new RelayCommand(SetPreflop);
            FlopCommand = new RelayCommand(SetFlop);
            TurnCommand = new RelayCommand(SetTurn);
            RiverCommand = new RelayCommand(SetRiver);
            ShowdownCommand = new RelayCommand(SetShowdown);
            NextActionCommand = new RelayCommand(NextAction);
            PreviousActionCommand = new RelayCommand(PreviousAction);
            Players = new ObservableCollection<PlayerHandVM>();

            // Setup game
            SetupGame();
        }

        #region logic
        public void SetupGame()
        {
            // Read data from file or DB
            var path = Path.Combine(
                AppContext.BaseDirectory,
                "Ressources",
                "Data",
                "data.json");
            var pokerGames = PokerGameJSONReader.readPokerGameJSON(path);
            PokerGameDTO pokerGameDTO = new PokerGameDTO(pokerGames[0]);

            // Get community card infos (Community cards and pot sizes)
            CommunityCards = new CommunityCardsVM(pokerGameDTO.Community.CommunityCards[0], pokerGameDTO.Community.CommunityCards[1], pokerGameDTO.Community.CommunityCards[2], pokerGameDTO.Community.CommunityCards[3], pokerGameDTO.Community.CommunityCards[4]);
            
            // Get player infos (Cards, pots and position)
            Players.Clear();
            foreach(PlayerHandDTO player in pokerGameDTO.Players.Values)
            {
                Players.Add(new PlayerHandVM(player.PotSize, player.PocketCards[0], player.PocketCards[1], player.Position, "Unknown Player"));
            }

            // Get game actions (bet/check/fold/.. for each street)
            Actions = pokerGameDTO.GameActions;
        }
        #endregion

        #region ControlButtons
        public void NextAction() {
            if (_cursor + 1 >= Actions.Count)
                return;

            var action = Actions[_cursor++];

            // Update Community if need be
            if (action.Street != _currentStreet) {
                // Get rid of folded players
                foreach(var p in Players)
                {
                    if(p.CurrentAction == ActionTypeEnum.Fold) {
                        p.HidePlayer(action.Street);
                    }
                }

                // Update street
                _currentStreet = action.Street;
                CommunityCards.SetStreet(action.Street);
            }

            // Update player if need be
            int playerPosition = action.PlayerPosition;
            Players[playerPosition - 1].ApplyAction(action.Action);
        }

        public void PreviousAction() {
            if (_cursor < 1)
                return;

            var action = Actions[--_cursor];

            if (action.Street != _currentStreet) {
                // Show player eliminated at previous street
                foreach (var p in Players) {
                    p.ShowPlayer(_currentStreet);
                }

                // Update street
                _currentStreet = action.Street;
                CommunityCards.SetStreet(action.Street);
            }

            // Update player if need be
            int playerPosition = action.PlayerPosition;
            Players[playerPosition-1].UndoAction();
        }

        public void SetPreflop()
        {
            //CommunityCards.SetPreflop();
            //foreach (var player in Players)
            //{
            //    player.SetPreflop();
            //}
        }

        public void SetFlop()
        {
            //CommunityCards.SetFlop();
            //foreach (var player in Players)
            //{
            //    player.SetFlop();
            //}
        }

        public void SetTurn()
        {
            //CommunityCards.SetTurn();
            //foreach (var player in Players)
            //{
            //    player.SetTurn();
            //}
        }

        public void SetRiver()
        {
            //CommunityCards.SetRiver();
            //foreach (var player in Players)
            //{
            //    player.SetRiver();
            //}
        }

        public void SetShowdown()
        {
            //CommunityCards.SetShowdown();
            //foreach (var player in Players)
            //{
            //    player.SetShowdown();
            //}
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
