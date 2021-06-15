using System;

namespace WarOfHeroesUsersAPI.Users
{
    public static class AccessTokenGenerator
    {
        public static string GenerateAccessToken()
        {
            var guid = Guid.NewGuid();
            var currentDate = $"{DateTime.UtcNow:yyyyMMdd}";

            return $"WoH-{currentDate}-{guid}";
        }
    }
}