using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WarOfHeroesUsersAPI.Data.Entities;

namespace WarOfHeroesUsersAPI.Data
{
    public class UserContext : DbContext
    {
        private readonly IConfiguration _config;

        public UserContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }
        public virtual DbSet<UserHeroDeck> UserHeroDecks { get; set; }

        public virtual DbSet<UserHeroInventory> UserHeroInventories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_config.GetConnectionString("UserDb"));
        }
    }
}