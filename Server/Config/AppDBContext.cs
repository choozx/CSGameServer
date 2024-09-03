using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Server.Game.Spec;

namespace Server.Config
{
    
    public class GameDBContext : DbContext
    {
        private const string connectionString = @"Server=localhost\SQLEXPRESS;Database=inspector-game;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        
        public static void InitializeDB(bool forceInit = false)
        {
            using (GameDBContext db = new GameDBContext())
            {
                if (!forceInit && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return;

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
    
    public class SpecDBContext : DbContext
    {
        // Spec table
        public DbSet<Tile> Tile { get; set; }
        
        private const string connectionString = @"Server=localhost\SQLEXPRESS;Database=inspector-spec;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        
        public static void InitializeDB(bool forceInit = false)
        {
            using (SpecDBContext db = new SpecDBContext())
            {
                if (!forceInit && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return;

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}