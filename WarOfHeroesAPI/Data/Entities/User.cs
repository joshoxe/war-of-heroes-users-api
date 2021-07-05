using System.Collections.Generic;

namespace WarOfHeroesUsersAPI.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string GoogleId { get; set; }
        public string FirstName { get; set; }
        public string AccessToken { get; set; }
        public virtual List<UserHeroInventory> UserHeroInventories { get; set; }
        public virtual List<UserHeroDeck> UserHeroDecks { get; set; }

    }
}