using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WarOfHeroesUsersAPI.Data;
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
        private readonly IUserRepository _repository;
        private readonly IUserValidator _userValidator;

        public UserController(ILogger<UserController> logger, IUserValidator userValidator,
            IUserProcessor<GoogleUser> userProcessor, IUserRepository repository)
        {
            _logger = logger;
            _userValidator = userValidator;
            _userProcessor = userProcessor;
            _repository = repository;
        }

        [Route("login")]
        [HttpPost]
        public ActionResult Login([FromBody] GoogleUser user)
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


        [AllowAnonymous] // Turn off
        [Route("{userId}")]
        [HttpGet]
        public ActionResult Get([FromRoute] int userId)
        {
            try
            {
                var user = _repository.GetUserById(userId);
                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to find user with ID {userId}", userId);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public ActionResult Get()
        {
            try {
                return Ok(_repository.GetAllUsers());
            } catch(Exception e) {
                _logger.LogError(e, "Unable to find users");
                return BadRequest();
            }
        }
    }
}