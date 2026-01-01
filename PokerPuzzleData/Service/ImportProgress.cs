using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.Service
{
    public class ImportProgress
    {
        public int Processed {  get; }
        public int Total { get; }

        public ImportProgress(int processed, int total) 
        { 
            Processed = processed; 
            Total = total; 
        }
    }
}
