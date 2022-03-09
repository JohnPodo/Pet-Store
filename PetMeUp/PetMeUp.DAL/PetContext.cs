using Microsoft.EntityFrameworkCore;
using PetMeUp.Models;
using PetMeUp.Models.Models;

namespace PetMeUp.DAL
{
    public class PetContext : DbContext
    {
        public PetContext(string conString, DatabaseType type) : base(GetOptions(conString, type))
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<LogMeUp> Logs { get; set; }
        public DbSet<PetSpecie> Species { get; set; }
        public DbSet<PetGroup> Groups { get; set; }
        public DbSet<Pic> Pics { get; set; }
        public DbSet<PetFamily> Families { get; set; }
        public DbSet<Pet> Pets { get; set; }
        private static DbContextOptions GetOptions(string connectionString, DatabaseType type)
        {
            if (type == DatabaseType.MSSQL)
                return new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;
            if (type == DatabaseType.SQLite)
                return new DbContextOptionsBuilder().UseSqlite(connectionString).Options;
            return null;
        }

        ///// <summary>
        ///// Only For Migrations for SqlLite
        ///// </summary>
        ///// <param name="options"></param>
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //   => options.UseSqlite($"Data Source={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mailmeup.db")}");

        //// <summary>
        //// Only For Migrations for SqlServer
        //// </summary>
        //// <param name = "options" ></ param >
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //   => options.UseSqlServer("");
    }
} 