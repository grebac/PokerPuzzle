using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace PokerPuzzle.IO
{
    public static class FavoritesGameHelper
    {
        private static readonly string FavoritesPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PokerPuzzle",
                "favorites.json");

        private static Dictionary<int, string>? _cache;

        static FavoritesGameHelper()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FavoritesPath)!);
        }

        public static Dictionary<int, string> LoadFavorites()
        {
            if (_cache != null)
                return _cache;

            if (!File.Exists(FavoritesPath))
                return _cache = new Dictionary<int, string>();

            try
            {
                var json = File.ReadAllText(FavoritesPath);
                _cache = JsonSerializer.Deserialize<Dictionary<int, string>>(json)
                         ?? new Dictionary<int, string>();
            }
            catch
            {
                _cache = new Dictionary<int, string>();
            }

            return _cache;
        }

        public static void SaveFavorites()
        {
            if (_cache == null) return;

            var json = JsonSerializer.Serialize(_cache, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(FavoritesPath, json);
        }

        public static void AddFavorite(int gameId, string comment = "")
        {
            var favorites = LoadFavorites();
            favorites[gameId] = comment;
            SaveFavorites();
        }

        public static void RemoveFavorite(int gameId)
        {
            var favorites = LoadFavorites();
            if (favorites.Remove(gameId))
                SaveFavorites();
        }

        public static bool IsFavorite(int gameId)
            => LoadFavorites().ContainsKey(gameId);

        public static string? GetComment(int gameId)
            => LoadFavorites().TryGetValue(gameId, out var c) ? c : null;
    }
}
