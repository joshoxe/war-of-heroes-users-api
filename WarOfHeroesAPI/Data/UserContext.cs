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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config.GetConnectionString("UserDb"));
        }
    }
}