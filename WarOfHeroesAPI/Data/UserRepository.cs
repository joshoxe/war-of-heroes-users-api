﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WarOfHeroesUsersAPI.Data.Entities;

namespace WarOfHeroesUsersAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userContext.Users.Include(u => u.UserHeroInventories).Include(u => u.UserHeroDecks).ToList();
        }

        public User GetUserByGoogleId(string googleId)
        {
            return _userContext.Users.Include(u => u.UserHeroInventories).Include(u => u.UserHeroDecks)
                .FirstOrDefault(u => u.GoogleId == googleId);
        }

        public void AddNewUser(User user)
        {
            _userContext.Users.Add(user);
            _userContext.SaveChanges();
        }

        public User GetUserById(int userId)
        {
            return _userContext.Users.Where(u => u.Id == userId).Include(u => u.UserHeroInventories)
                .Include(u => u.UserHeroDecks).First();
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