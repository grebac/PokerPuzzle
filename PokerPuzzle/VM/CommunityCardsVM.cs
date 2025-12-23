using System.Windows;
using System.ComponentModel;
using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;

namespace PokerPuzzle.VM
{
    public class CommunityCardsVM : INotifyPropertyChanged
    {
        #region attributes
        private CardsEnum _flop1 = CardsEnum.CardBack;
        private CardsEnum _flop2 = CardsEnum.CardBack;
        private CardsEnum _flop3 = CardsEnum.CardBack;
        private CardsEnum _turn = CardsEnum.CardBack;
        private CardsEnum _river = CardsEnum.CardBack;
        private Visibility _isFlopVisible;
        private Visibility _isTurnVisible;
        private Visibility _isRiverVisible;
        #endregion

        #region Properties
        public Visibility IsFlopVisible { get => _isFlopVisible;
            set { 
                _isFlopVisible = value;
                OnPropertyChanged(nameof(IsFlopVisible));
            } 
        }
        public Visibility IsTurnVisible
        {
            get => _isTurnVisible;
            set
            {
                _isTurnVisible = value;
                OnPropertyChanged(nameof(IsTurnVisible));
            }
        }
        public Visibility IsRiverVisible
        {
            get => _isRiverVisible;
            set
            {
                _isRiverVisible = value;
                OnPropertyChanged(nameof(IsRiverVisible));
            }
        }

        public string Flop1Path {
            get => _flop1.ToImagePath();
        }

        public string Flop2Path
        {
            get => _flop2.ToImagePath();
        }

        public string Flop3Path
        {
            get => _flop3.ToImagePath();
        }

        public string TurnPath
        {
            get => _turn.ToImagePath();
        }

        public string RiverPath
        {
            get => _river.ToImagePath();
        }
        #endregion

        #region Constructors
        public CommunityCardsVM(): this(CardsEnum.ThreeOfClubs, CardsEnum.EightOfHearts, CardsEnum.FourOfDiamonds, CardsEnum.FiveOfClubs, CardsEnum.JackOfSpades) { }

        public CommunityCardsVM(CardsEnum flop1, CardsEnum flop2, CardsEnum flop3, CardsEnum turn, CardsEnum river, Visibility isFlopVisible = Visibility.Hidden, Visibility isTurnVisible = Visibility.Hidden, Visibility isRiverVisible = Visibility.Hidden)
        {
            IsFlopVisible = isFlopVisible;
            IsTurnVisible = isTurnVisible;
            IsRiverVisible = isRiverVisible;
            setCards(flop1, flop2, flop3, turn, river);
        }
        #endregion

        #region logic
        public void setCards(CardsEnum flop1, CardsEnum flop2, CardsEnum flop3, CardsEnum turn, CardsEnum river) {
            _flop1 = flop1;
            _flop2 = flop2;
            _flop3 = flop3;
            _turn = turn;
            _river = river;
            OnPropertyChanged(nameof(Flop1Path));
            OnPropertyChanged(nameof(Flop2Path));
            OnPropertyChanged(nameof(Flop3Path));
            OnPropertyChanged(nameof(TurnPath));
            OnPropertyChanged(nameof(RiverPath));
        }
        #endregion

        #region PokerGameStage
        public void SetStreet(StreetEnum street) {
            IsFlopVisible = street >= StreetEnum.Flop ? Visibility.Visible : Visibility.Hidden;
            IsTurnVisible = street >= StreetEnum.Turn ? Visibility.Visible : Visibility.Hidden;
            IsRiverVisible = street >= StreetEnum.River ? Visibility.Visible : Visibility.Hidden;
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
