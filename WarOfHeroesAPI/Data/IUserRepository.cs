using System.Collections.Generic;
using WarOfHeroesUsersAPI.Data.Entities;

namespace WarOfHeroesUsersAPI.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserByGoogleId(string googleId);
        void AddNewUser(User user);
        User GetUserById(int userId);
        IEnumerable<int> GetUserInventory(int userId);
        IEnumerable<int> GetUserDeck(int userId);
        void AddToUserDeck(int userId, int heroId);
        void AddToUserInventory(int userId, int heroId);
        void RemoveFromUserDeck(int userId, int heroId);
        bool InventoryContainsHero(int userId, int heroId);
        bool DeckContainsHero(int userId, int heroId);
        void UpdateDeck(int userId, int[] ids);
        void UpdateInventory(int userId, int[] ids);
    }
}