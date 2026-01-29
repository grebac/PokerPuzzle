using Microsoft.EntityFrameworkCore;
using PokerPuzzleData.DB;
using PokerPuzzleData.DTO;
using PokerPuzzleData.Enum;
using PokerPuzzleData.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerPuzzleData.Service
{
    public static class GameCaracteristicsAnalyzer
    {
        public static void AnalyzeAllMissing(IProgress<ImportProgress>? progress = null)
        {
            using (PokerPuzzleContext context = new PokerPuzzleContext())
            {
                var games = context.Games
                    .Where(g => g.BoardTexture == BoardTexture.None)
                    .Include(g => g.CommunityCards)
                    .ToList();

                int processed = 0;
                int total = games.Count;

                foreach (var game in games)
                {
                    game.BoardTexture = Analyze(game.CommunityCards.ToList());
                    processed++;
                    
                    progress?.Report(new ImportProgress(
                        processed,
                        total,
                        ImportPhaseEnum.AnalayseGames));
                }

                context.SaveChanges();
            }
        }

        private static BoardTexture Analyze(IReadOnlyList<CardRankSuit> board)
        {
            // TODO - Is this an ok default value?
            if (board == null || board.Count == 0)
                return BoardTexture.None;

            BoardTexture texture = AnalyzeFlop(board.Take(3).ToList());
            texture |= AnalyzeFinalBoard(board);
            return texture;
        }

        private static BoardTexture AnalyzeFlop(IReadOnlyList<CardRankSuit> flop)
        {
            var texture = BoardTexture.None;

            texture |= AnalyzeFlopHeight(flop);
            texture |= AnalyzeFlopConnectivity(flop);
            texture |= AnalyzeFlopWetness(flop);
            texture |= AnalyzeFlopSuits(flop);

            return texture;
        }

        private static BoardTexture AnalyzeFinalBoard(IReadOnlyList<CardRankSuit> board)
        {
            var texture = BoardTexture.None;

            texture |= AnalyzeFlushThreat(board);
            texture |= AnalyzeStraightThreat(board);
            texture |= AnalyzeSetThreat(board);

            return texture;
        }

        // ───────────────────────────────────────────────
        // FLOP ANALYSIS
        // ───────────────────────────────────────────────

        private static BoardTexture AnalyzeFlopHeight(IReadOnlyList<CardRankSuit> flop)
        {
            var maxRank = flop.Max(c => c.Rank);

            return maxRank switch
            {
                >= Rank.Jack => BoardTexture.HighFlop,
                >= Rank.Eight => BoardTexture.MiddleFlop,
                _ => BoardTexture.LowFlop
            };
        }

        // TODO - Is 3 rank distance to strict? (test -> <= 4)
        private static BoardTexture AnalyzeFlopConnectivity(IReadOnlyList<CardRankSuit> flop)
        {
            var ranks = flop.Select(c => (int)c.Rank).OrderBy(r => r).ToArray();
            return (ranks[2] - ranks[0] <= 3)
                ? BoardTexture.ConnectedFlop
                : BoardTexture.DisconnectedFlop;
        }

        // TODO - Are these caracteristics too lenient?
        private static BoardTexture AnalyzeFlopWetness(IReadOnlyList<CardRankSuit> flop)
        {
            var connected = AnalyzeFlopConnectivity(flop) == BoardTexture.ConnectedFlop;
            var suited = AnalyzeFlopSuits(flop) != BoardTexture.RainbowFlop;

            return (connected || suited)
                ? BoardTexture.WetFlop
                : BoardTexture.DryFlop;
        }

        private static BoardTexture AnalyzeFlopSuits(IReadOnlyList<CardRankSuit> flop)
        {
            var suitCount = flop.GroupBy(c => c.Suit).Count();

            return suitCount switch
            {
                1 => BoardTexture.MonoFlop,
                2 => BoardTexture.TwoToneFlop,
                _ => BoardTexture.RainbowFlop
            };
        }

        // ───────────────────────────────────────────────
        // FINAL BOARD (SHOWDOWN)
        // ───────────────────────────────────────────────

        private static BoardTexture AnalyzeFlushThreat(IReadOnlyList<CardRankSuit> board)
        {
            var maxSuit = board
                .GroupBy(c => c.Suit)
                .Max(g => g.Count());

            return maxSuit switch
            {
                >= 4 => BoardTexture.BackdoorFlush,
                3 => BoardTexture.FlushPossible,
                _ => BoardTexture.None
            };
        }

        private static BoardTexture AnalyzeStraightThreat(IReadOnlyList<CardRankSuit> board)
        {
            var ranks = board
                .Select(c => (int)c.Rank)
                .Distinct()
                .OrderBy(r => r)
                .ToArray();

            for (int i = 0; i <= ranks.Length - 4; i++)
            {
                if (ranks[i + 3] - ranks[i] <= 4)
                    return BoardTexture.StraightPossible;
            }

            return BoardTexture.None;
        }

        private static BoardTexture AnalyzeSetThreat(IReadOnlyList<CardRankSuit> board)
        {
            var groups = board.GroupBy(c => c.Rank).Select(g => g.Count()).ToList();

            return (groups.Contains(2))
                ? BoardTexture.FullHousePossible
                : BoardTexture.None;
        }
    }
}