using System;
using System.Collections.Generic;
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

        public IEnumerable<int> GetUserDeck(int userId)
        {
            var deck = GetUserById(userId).UserHeroDecks;

            foreach (var hero in deck)
            {
                yield return hero.HeroId;
            }
        }

        public void AddToUserDeck(int userId, int heroId)
        {
            _userContext.Users.FirstOrDefault(u => u.Id == userId)
                ?.UserHeroDecks.Add(new UserHeroDeck
            {
                HeroId = heroId
            });
        }

        public void RemoveFromUserDeck(int userId, int heroId)
        {
            var userHeroDecks = _userContext.Users.FirstOrDefault(u => u.Id == userId)
                ?.UserHeroDecks;
            var item = userHeroDecks?.FirstOrDefault(d => d.HeroId == heroId);

            if (userHeroDecks == null)
            {
                throw new Exception("User deck was null");
            }

            if (item == null)
            {
                throw new Exception("Hero not found in deck");
            }

            userHeroDecks.Remove(item);
        }

        public void UpdateUserAccessToken(int userId, string accessToken)
        {
            var user = GetUserById(userId);

            if (user == null)
            {
                throw new ArgumentException($"The user with ID {userId} was not found");
            }

            user.AccessToken = accessToken;
            _userContext.SaveChanges();
        }
    }
}