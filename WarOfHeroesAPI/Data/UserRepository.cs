using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using WarOfHeroesUsersAPI.Data.Entities;

namespace WarOfHeroesUsersAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;
        private readonly ILogger<UserContext> _logger;

        public UserRepository(UserContext userContext, ILogger<UserContext> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userContext.Users.Include(u => u.UserHeroInventories).ToList();

        }

        public User GetUserByGoogleId(string googleId)
        {
            var userByGoogleId = _userContext.Users.Include(u => u.UserHeroInventories)
                .Select(u => u.GoogleId);
            _logger.LogDebug(String.Join(" ", userByGoogleId.Select(u => u)));
           return _userContext.Users.Include(u => u.UserHeroInventories).FirstOrDefault<User>(u => googleId == u.GoogleId);
        }

        public void AddNewUser(User user)
        {
            _userContext.Users.Add(user);
            _userContext.SaveChanges();
        }

        public User GetUserById(int userId)
        {
            return _userContext.Users.Where(u => u.Id == userId).Include(u => u.UserHeroInventories).First();
        }

        public IEnumerable<int> GetUserInventory(int userId)
        {
            var inventory = GetUserById(userId).UserHeroInventories;

            foreach (var hero in inventory)
            {
                yield return hero.HeroId;
            }
        }
    }
}