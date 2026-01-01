using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace PokerPuzzle.IO
{
    public static class FavoritesGameHelper
    {
        private static readonly string FavoritesPath = Path.Combine(
            AppContext.BaseDirectory,
            "favorites.json");

        public static Dictionary<int, string> LoadFavorites()
        {
            if (!File.Exists(FavoritesPath)) {
                return new Dictionary<int, string>();
            }

            try
            {
                var json = File.ReadAllText(FavoritesPath);
                return JsonSerializer.Deserialize<Dictionary<int, string>>(json)
                       ?? new Dictionary<int, string>();
            }
            catch
            {
                // Si le fichier est corrompu, on repart de zéro
                return new Dictionary<int, string>();
            }
        }

        public static void SaveFavorites(Dictionary<int, string> favorites)
        {
            var json = JsonSerializer.Serialize(favorites, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(FavoritesPath, json);
        }

        public static void AddFavorite(int gameIndex, string comment)
        {
            var favorites = LoadFavorites();
            favorites[gameIndex] = comment;
            SaveFavorites(favorites);
        }

        public static void RemoveFavorite(int gameIndex)
        {
            var favorites = LoadFavorites();
            favorites.Remove(gameIndex);
            SaveFavorites(favorites);
        }

        public static bool IsFavorite(int gameIndex)
        {
            var favorites = LoadFavorites();
            return favorites.ContainsKey(gameIndex);
        }
    }
}
