using System.Collections.Generic;
using WarOfHeroesUsersAPI.Data.Entities;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPI.Processing
{
    public class UserProcessingResult
    {
        public bool IsValid { get; set; }
        public ICollection<string> Errors { get; set; }
        public Data.Entities.User User { get; set; }
    }
}