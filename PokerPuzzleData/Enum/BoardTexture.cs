using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.Enum
{
    [Flags]
    public enum BoardTexture
    {
        None = 0,

        // ─── FLOP STRUCTURE ─────────────────────────────
        HighFlop = 1 << 0,
        MiddleFlop = 1 << 1,
        LowFlop = 1 << 2,

        WetFlop = 1 << 3,
        DryFlop = 1 << 4,

        ConnectedFlop = 1 << 5,
        DisconnectedFlop = 1 << 6,

        RainbowFlop = 1 << 7,
        TwoToneFlop = 1 << 8,
        MonoFlop = 1 << 9,

        // ─── BOARD (SHOWDOWN) THREATS ───────────────────
        FlushPossible = 1 << 10, // 3 same suit
        BackdoorFlush = 1 << 11, // 4 same suit

        StraightPossible = 1 << 12,
        FullHousePossible = 1 << 13
    }
}
