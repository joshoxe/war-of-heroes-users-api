using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using WarOfHeroesUsersAPI.Users;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPI.Processing
{
    public class GoogleUserProcessor : IUserProcessor<GoogleUser>
    {
        private readonly ILogger<GoogleUserProcessor> _logger;
        private readonly IUserStore _userStore;

        public GoogleUserProcessor(IUserStore userStore, ILogger<GoogleUserProcessor> logger)
        {
            _userStore = userStore;
            _logger = logger;
        }

        public UserProcessingResult Process(GoogleUser googleUser)
        {
            var userProcessingResult = new UserProcessingResult();
            var users = _userStore.Users;

            try
            {
                var user = users.SingleOrDefault(u => u.GoogleID == googleUser.ID);
                if (user == null)
                {
                    user = new DbUser
                    {
                        FirstName = googleUser.FirstName,
                        GoogleID = googleUser.ID
                    };

                    _userStore.Users.Add(user);
                }

                userProcessingResult.IsValid = true;
                userProcessingResult.User = user;

                return userProcessingResult;
            }
            catch (Exception e)
            {
                _logger.LogError("GoogleUserProcessor failed to process user: {googleUser}, exception: {e}", googleUser,
                    e);
                return userProcessingResult;
            }
        }
    }
}