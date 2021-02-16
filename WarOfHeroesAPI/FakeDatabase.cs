using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarOfHeroesAPI
{
    // Interim database before implementing Entity Framework
    public class FakeDatabase
    {
        public List<User> Users { get; set; }

        public FakeDatabase()
        {
            Users = new List<User>();
            Users.Add(new User("test", "test"));
        }
    }
}
