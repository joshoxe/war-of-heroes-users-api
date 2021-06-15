using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using WarOfHeroesUsersAPI.Data;
using WarOfHeroesUsersAPI.Data.Entities;
using WarOfHeroesUsersAPI.Users;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPI.Processing
{
    public class GoogleUserProcessor : IUserProcessor<GoogleUser>
    {
        private readonly ILogger<GoogleUserProcessor> _logger;
        private readonly IUserRepository _repository;

        public GoogleUserProcessor(ILogger<GoogleUserProcessor> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public UserProcessingResult Process(GoogleUser googleUser)
        {
            var userProcessingResult = new UserProcessingResult();

            try
            {
                var user = _repository.GetUserByGoogleId(googleUser.ID) ?? AddNewUser(googleUser);
                user.AccessToken = AccessTokenGenerator.GenerateAccessToken();

                userProcessingResult.IsValid = true;
                userProcessingResult.User = user;

                return userProcessingResult;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GoogleUserProcessor failed to process user: {googleUser}", googleUser,
                    e);
                return userProcessingResult;
            }
        }

        private Data.Entities.User AddNewUser(GoogleUser googleUser)
        {
            var user = new Data.Entities.User
            {
                FirstName = googleUser.FirstName,
                GoogleId = googleUser.ID,
                UserHeroInventories = new List<UserHeroInventory>
                {
                    new UserHeroInventory
                    {
                        HeroId = 1
                    },
                    new UserHeroInventory
                    {
                        HeroId = 2
                    },
                    new UserHeroInventory
                    {
                        HeroId = 3
                    }
                },
                AccessToken = AccessTokenGenerator.GenerateAccessToken()
            };

            _repository.AddNewUser(user);
            return user;
        }
    }
}