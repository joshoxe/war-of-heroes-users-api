using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPI.Processing
{
    public interface IUserProcessor<in T>
    {
        UserProcessingResult Process(T googleUser);
    }
}