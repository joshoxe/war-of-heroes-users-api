using System.Collections.Generic;

namespace WarOfHeroesUsersAPI.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string GoogleId { get; set; }
        public string FirstName { get; set; }
        public ICollection<UserHeroInventory> Inventory { get; set; }
        public ICollection<UserHeroDeck> Deck { get; set; }
    }
}