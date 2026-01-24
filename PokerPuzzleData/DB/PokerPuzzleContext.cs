using Microsoft.EntityFrameworkCore;
using PokerPuzzleData.DB.Entity;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace PokerPuzzleData.DB
{
    public class PokerPuzzleContext : DbContext
    {
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<ActionEntity> Actions { get; set; }
        public DbSet<CommunityCardsEntity> CommunityCards { get; set; }
        public DbSet<GameCommentEntity> GameComments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appDir = Path.Combine(baseDir, "PokerPuzzle");

            if (!Directory.Exists(appDir))
                Directory.CreateDirectory(appDir);

            var dbPath = Path.Combine(appDir, "PokerPuzzle.db");
            options.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            // Game
            model.Entity<GameEntity>()
                .HasKey(g => g.GameId);

            // Player (composite key)
            model.Entity<PlayerEntity>()
                .HasKey(p => new { p.GameId, p.Position });

            model.Entity<PlayerEntity>()
                .HasOne(p => p.Game)
                .WithMany(g => g.Players)
                .HasForeignKey(p => p.GameId);

            // Actions (composite key)
            model.Entity<ActionEntity>()
                .HasKey(a => new { a.GameId, a.OrderIndex });

            model.Entity<ActionEntity>()
                .HasOne(a => a.Game)
                .WithMany(g => g.Actions)
                .HasForeignKey(a => a.GameId);

            // Community cards (1–1)
            model.Entity<CommunityCardsEntity>()
                .HasKey(c => c.GameId);

            model.Entity<CommunityCardsEntity>()
                .HasOne(c => c.Game)
                .WithOne(g => g.CommunityCards)
                .HasForeignKey<CommunityCardsEntity>(c => c.GameId);

            model.Entity<GameCommentEntity>()
                .HasOne(c => c.Game)
                .WithOne(g => g.GameComment)
                .HasForeignKey<GameCommentEntity>(c => c.GameId);
        }
    }
}
