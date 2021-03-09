﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WarOfHeroesUsersAPI.Processing;
using WarOfHeroesUsersAPI.Users.Models;
using WarOfHeroesUsersAPI.Validation;


namespace WarOfHeroesUsersAPI.Controllers
{
    [Route("user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserProcessor<GoogleUser> _userProcessor;
        private readonly IUserValidator _userValidator;

        public UserController(ILogger<UserController> logger, IUserValidator userValidator,
            IUserProcessor<GoogleUser> userProcessor)
        {
            _logger = logger;
            _userValidator = userValidator;
            _userProcessor = userProcessor;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] GoogleUser user)
        {
            _logger.LogInformation("Login endpoint called");

            var userValidationResult = _userValidator.Validate(user);

            if (!userValidationResult.IsValid)
            {
                _logger.LogError("Login failed to validate user: {user}, validation errors: {errors}", user,
                    userValidationResult.Errors);

                return BadRequest(userValidationResult.Errors);
            }

            var userProcessResult = _userProcessor.Process(user);

            if (!userProcessResult.IsValid)
            {
                _logger.LogError("Login failed to process user, validation errors: {errors}", userProcessResult.Errors);
                return BadRequest(userProcessResult.Errors);
            }

            return Ok(userProcessResult.User);
        }
    }
}