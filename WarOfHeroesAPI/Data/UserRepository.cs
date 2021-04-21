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

        public IEnumerable<Entities.User> GetAllUsers()
        {
            return _userContext.Users.Include(u => u.UserHeroInventories).ToList();

        }

        public Entities.User GetUserByGoogleId(string googleId)
        {
            return _userContext.Users.Include(u => u.UserHeroInventories)
                .SingleOrDefault(u => u.GoogleId == googleId);
        }

        public void AddNewUser(Entities.User user)
        {
            _userContext.Users.Add(user);
            _userContext.SaveChanges();
        }

        public Entities.User GetUserById(int userId)
        {
            var firstOrDefault = _userContext.Users.Where(u => u.Id == userId).Include(u => u.UserHeroInventories);
            var userHeroInventories = firstOrDefault.First().UserHeroInventories;
            return firstOrDefault.First();
        }
    }
}