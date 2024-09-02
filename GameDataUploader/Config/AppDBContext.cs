using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameDataUploader
{
    public class AppDBContext : DbContext
    {
        public DbSet<Wall> map { get; set; }

        private const string connectionString = @"Server=localhost\SQLEXPRESS;Database=inspector-game;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        
        public static void InitializeDB(bool forceInit = false)
        {
            using (AppDBContext db = new AppDBContext())
            {
                if (!forceInit && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return;

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Console.WriteLine("Init DB");
            }
        }
    }
}