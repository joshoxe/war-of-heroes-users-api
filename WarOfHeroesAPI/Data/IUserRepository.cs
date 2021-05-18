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
    }
}