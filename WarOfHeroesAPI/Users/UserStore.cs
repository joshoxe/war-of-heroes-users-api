using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPI.Users
{
    [ExcludeFromCodeCoverage]
    public class UserStore : IUserStore
    {
        public ICollection<DbUser> Users { get; } = new List<DbUser>();
    }
}
