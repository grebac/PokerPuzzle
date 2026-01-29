using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.Service
{
    public class ImportProgress
    {

        public int Processed { get; }
        public int Total { get; }
        public ImportPhaseEnum Phase { get; }

        public ImportProgress(int processed, int total, ImportPhaseEnum phase)
        {
            Processed = processed;
            Total = total;
            Phase = phase;
        }
    }
    public enum ImportPhaseEnum {
        ImportGames,
        AnalayseGames
    }
}
