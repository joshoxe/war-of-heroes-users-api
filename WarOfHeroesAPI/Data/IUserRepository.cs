using System.Collections.Generic;
using System.Linq;
using WarOfHeroesUsersAPI.Data.Entities;

namespace WarOfHeroesUsersAPI.Data
{
    public interface IUserRepository
    {
        IEnumerable<Entities.User> GetAllUsers();
        Entities.User GetUserByGoogleId(string googleId);
        void AddNewUser(Entities.User user);
        Entities.User GetUserById(int userId);
    }
}