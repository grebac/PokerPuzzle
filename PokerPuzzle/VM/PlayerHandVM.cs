using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;
using System;
using System.ComponentModel;
using System.Windows;

namespace PokerPuzzle.VM
{
    public class PlayerHandVM : INotifyPropertyChanged
    {
        #region attributes
        // Datas
        private string _playerName = "default";
        private decimal _chipCount;
        private bool _areCardsVisible;
        private CardsEnum _card1 = CardsEnum.CardBack;
        private CardsEnum _card2 = CardsEnum.CardBack;
        private Visibility _isHidden = Visibility.Visible;
        private Stack<ActionTypeEnum> _actionHistory;
        private ActionTypeEnum _currentAction = ActionTypeEnum.Nothing;
        private StreetEnum? _streetHidden = null; // Street at which the player is eliminated (Code Smell)
        #endregion

        #region Properties
        // Card Image Path Binding
        public string Card1Path { 
            get 
            {
                if (_areCardsVisible) {
                    return _card1.ToImagePath();
                }
                return CardsEnum.CardBack.ToImagePath();
            }
        }

        public string Card2Path
        {
            get
            {
                if (_areCardsVisible) {
                    return _card2.ToImagePath();
                }
                return CardsEnum.CardBack.ToImagePath();

            }
        }

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                OnPropertyChanged(nameof(PlayerName));
            }
        }

        public decimal ChipCount
        {
            get => _chipCount;
            set
            {
                _chipCount = value;
                OnPropertyChanged(nameof(ChipCount));
            }
        }

        public bool AreCardsVisible
        {
            get => _areCardsVisible;
            set
            {
                _areCardsVisible = value;
                OnPropertyChanged(nameof(Card1Path));
                OnPropertyChanged(nameof(Card2Path));
                OnPropertyChanged(nameof(AreCardsVisible));
            }
        }

        public Visibility IsHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value;
                OnPropertyChanged(nameof(IsHidden));
            }
        }

        public Visibility IsRevealable
        {
            get => (_card1 != CardsEnum.CardBack && _card2 != CardsEnum.CardBack) ? Visibility.Visible : Visibility.Hidden;
        }

        public ActionTypeEnum CurrentAction
        {
            get => _currentAction;
            set
            {
                _currentAction = value;
                OnPropertyChanged(nameof(CurrentAction));
                OnPropertyChanged(nameof(ActionPath));
            }
        }

        public int PlayerPosition { get; private set; } = -1;

        public string? ActionPath { get => _currentAction.ToImagePath(); }
        #endregion

        #region constructors
        public PlayerHandVM() : this(100, CardsEnum.AceOfClubs, CardsEnum.AceOfDiamonds, -1, "default") { }

        public PlayerHandVM(decimal chipCount, CardsEnum card1, CardsEnum card2, int position, string playerName, bool areCardsVisible = false)
        {
            ChipCount = chipCount;
            AreCardsVisible = areCardsVisible;
            PlayerPosition = position;
            PlayerName = playerName ?? "";
            SetCards(card1, card2);
            _actionHistory = new(new[] { ActionTypeEnum.Nothing });
        }
        #endregion

        #region Logique
        public void SetCards(CardsEnum card1, CardsEnum card2)
        {
            _card1 = card1;
            _card2 = card2;
            OnPropertyChanged(nameof(Card1Path));
            OnPropertyChanged(nameof(Card2Path));
            OnPropertyChanged(nameof(IsRevealable));
        }

        public void SetCardVisibility(bool visibility)
        {
            AreCardsVisible = visibility;
        }
        #endregion

        #region GameAction
        public void ApplyAction(ActionTypeEnum action)
        {
            _actionHistory.Push(_currentAction);
            CurrentAction = action;
        }

        public void UndoAction(ActionTypeEnum action)
        {
            CurrentAction = _actionHistory.Pop();
        }

        public void EnterStreet(StreetEnum street) {
            if(IsHidden == Visibility.Visible && CurrentAction == ActionTypeEnum.Fold) { // If we were folded and not eliminated yet, we get eliminated this street.
                IsHidden = Visibility.Hidden; // We set ourselves to invisible.
                _streetHidden = street - 1; // We remember which street we got eliminated on (for rollback purpose). If we enter the River, it means we got eliminated on the previous street: the Turn (street - 1)
            } else { // Otherwise, we reset our action.
                _actionHistory.Push(_currentAction);
                CurrentAction = ActionTypeEnum.Nothing;
            }
        }

        public void RollbackStreet(StreetEnum street) {
            if(_streetHidden == street) { // If we rollback to the street we got eliminated on, we should set out visibility back on. CurrentAction should still be at "fold".
                IsHidden = Visibility.Visible; 
            } else { // Otherwise, we need to rollback to our last action
                CurrentAction = _actionHistory.Pop();
            }
        }

        public void HidePlayer(StreetEnum streetHidden) {
            IsHidden = Visibility.Hidden;
            _streetHidden = streetHidden;
        }

        public void ShowPlayer(StreetEnum streetHidden) {
            if(_streetHidden == streetHidden) {
                IsHidden = Visibility.Visible;
            }
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
