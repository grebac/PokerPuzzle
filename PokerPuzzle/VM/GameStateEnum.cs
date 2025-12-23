using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerPuzzle.VM
{
    public enum GameStateEnum
    {
        Preflop,
        Flop,
        Turn,
        River,
        Showdown
    }

    public enum GameActionEnum
    {
        Check,
        Call,
        Bet,
        Raise,
        Blind,
        Fold
    }
}
