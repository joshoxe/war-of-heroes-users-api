using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var decks = user?.UserHeroDecks;
            decks?.Add(new UserHeroDeck
            {
                HeroId = heroId
            });
            _userContext.SaveChanges();
        }

        public void AddToUserInventory(int userId, int heroId)
        {
            _userContext.Users.FirstOrDefault(u => u.Id == userId)?.UserHeroInventories.Add(new UserHeroInventory
            {
                HeroId = heroId
            });
            _userContext.SaveChanges();
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
            _userContext.SaveChanges();
        }

        public void RemoveFromUserInventory(int userId, int heroId)
        {
            var userHeroInventory = _userContext.Users.Include(u => u.UserHeroInventories).FirstOrDefault(u => u.Id == userId)
                ?.UserHeroInventories;
            var item = userHeroInventory?.FirstOrDefault(d => d.HeroId == heroId);

            if(userHeroInventory == null) {
                throw new Exception("User inventory was null");
            }

            if(item == null) {
                throw new Exception("Hero not found in inventory");
            }

            userHeroInventory.Remove(item);
            _userContext.SaveChanges();
        }

        /// <summary>
        /// Checks if a user's deck contains a hero
        /// </summary>
        /// <param name="userId">The ID of the user to check</param>
        /// <param name="heroId">The ID of the hero to check</param>
        /// <returns>True if the user's deck contains the hero</returns>
        public bool DeckContainsHero(int userId, int heroId)
        {
            var userDeck = _userContext.Users.Include(u => u.UserHeroDecks).FirstOrDefault(u => u.Id == userId)?.UserHeroDecks;

            if (userDeck == null)
            {
                return false;
            }

            var heroCard = userDeck.Where(h => h.HeroId == heroId);

            return heroCard.Any();
        }

        /// <summary>
        /// Replaces the entire deck of a user with the provided list of `ids`
        /// </summary>
        /// <param name="userId">User to replace deck of</param>
        /// <param name="ids">Hero IDs to replace with</param>
        public void UpdateDeck(int userId, int[] ids)
        {
            var newDeck = ids.Select(i => new UserHeroDeck() {HeroId = i});

            _userContext.Users.FirstOrDefault(u => u.Id == userId).UserHeroDecks.RemoveAll(h => true);

            _userContext.Users.Include(u => u.UserHeroDecks).FirstOrDefault(u => u.Id == userId).UserHeroDecks = newDeck.ToList();

            _userContext.SaveChanges();
        }

        /// <summary>
        /// Replaces the entire inventory of a user with the provided list of `ids`
        /// </summary>
        /// <param name="userId">User to replace inventory of</param>
        /// <param name="ids">Hero IDs to replace with</param>
        public void UpdateInventory(int userId, int[] ids) {
            var newInventory = ids.Select(i => new UserHeroInventory() { HeroId = i });
            _userContext.Users.Include(u => u.UserHeroInventories).FirstOrDefault(u => u.Id == userId).UserHeroInventories.RemoveAll(h => true);

            _userContext.Users.Include(u => u.UserHeroInventories).FirstOrDefault(u => u.Id == userId).UserHeroInventories = newInventory.ToList();
            _userContext.SaveChanges();
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

        public User GetUserByAccessToken(string accessToken)
        {
            return _userContext.Users.Include(u => u.UserHeroDecks).Include(u => u.UserHeroInventories)
                .FirstOrDefault(u => u.AccessToken == accessToken);
        }

        public void GiveUserCoins(string accessToken, int coins)
        {
            var user = GetUserByAccessToken(accessToken);

            user.Coins += coins;
            _userContext.SaveChanges();
        }

        public void IncreaseUserWins(string accessToken)
        {
            var user = GetUserByAccessToken(accessToken);

            user.Wins += 1;
            _userContext.SaveChanges();
        }

        public void IncreaseUserLosses(string accessToken)
        {
            var user = GetUserByAccessToken(accessToken);

            user.Losses += 1;
            _userContext.SaveChanges();
        }
    }
}