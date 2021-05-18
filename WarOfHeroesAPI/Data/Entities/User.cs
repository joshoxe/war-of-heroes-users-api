using System.Collections.Generic;

namespace WarOfHeroesUsersAPI.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string GoogleId { get; set; }
        public string FirstName { get; set; }
        public virtual ICollection<UserHeroInventory> UserHeroInventories { get; set; }
        public virtual List<UserHeroDeck> UserHeroDecks { get; set; }
    }
}