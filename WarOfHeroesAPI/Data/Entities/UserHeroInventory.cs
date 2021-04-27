using System.ComponentModel.DataAnnotations.Schema;

namespace WarOfHeroesUsersAPI.Data.Entities
{
    public class UserHeroInventory
    {
        public int Id { get; set; }
        public int HeroId { get; set; }
    }
}