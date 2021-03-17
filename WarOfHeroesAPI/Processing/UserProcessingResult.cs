using System.Collections.Generic;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPI.Processing
{
    public class UserProcessingResult
    {
        public bool IsValid { get; set; }
        public ICollection<string> Errors { get; set; }
        public DbUser User { get; set; }
    }
}