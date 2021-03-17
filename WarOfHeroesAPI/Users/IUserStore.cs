using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPI.Users
{
    public interface IUserStore
    {
        ICollection<DbUser> Users { get; }
    }
}
