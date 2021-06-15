using System;
using System.Linq;
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
        private readonly IUserRepository _repository;
        private readonly IUserProcessor<GoogleUser> _userProcessor;
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

        [Route("refresh")]
        [HttpPost]
        public ActionResult Refresh([FromBody] string accessToken)
        {

        }

        [Route("/user/{id}/inventory")]
        [HttpGet]
        public ActionResult GetInventory([FromRoute] int id)
        {
            try
            {
                var inventory = _repository.GetUserInventory(id);

                if (inventory == null || !inventory.Any())
                {
                    return NotFound($"User inventory with ID {id} not found");
                }

                return Ok(inventory);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred getting inventory for ID {id}", id);
                return BadRequest();
            }
        }

        [Route("/user/{id}/deck")]
        [HttpGet]
        public ActionResult GetDeck([FromRoute] int id)
        {
            try
            {
                var deck = _repository.GetUserDeck(id);

                if (deck == null)
                {
                    return NotFound($"User deck with ID {id} not found");
                }

                if (deck.Count() > 5)
                {
                    _logger.LogError($"More than 5 heroes found in user deck with ID {id}");
                    return BadRequest("More than 5 heroes found in user deck");
                }

                return Ok(deck);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred getting deck for ID {id}", id);
                return BadRequest();
            }
        }

        [Route("/user/{userId}/deck/add/{heroId}")]
        [HttpGet]
        public ActionResult AddToDeck([FromRoute] int userId, [FromRoute] int heroId)
        {
            try
            {
                var deck = _repository.GetUserDeck(userId);

                if (deck.Count() >= 5)
                {
                    return BadRequest($"Deck for user {userId} is full, cannot add more heroes");
                }

                _repository.AddToUserDeck(userId, heroId);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred adding hero {heroId} to user deck {userId}", heroId, userId);
                return BadRequest("An error occurred adding hero to deck");
            }
        }

        [Route("/user/{userId}/deck/add/{heroId}")]
        [HttpGet]
        public ActionResult RemoveFromDeck([FromRoute] int userId, [FromRoute] int heroId) {
            try {
                var deck = _repository.GetUserDeck(userId);

                if(deck.All(i => i != heroId)) {
                    return BadRequest($"Deck for user {userId} does not contain hero with ID {heroId}");
                }

                _repository.RemoveFromUserDeck(userId, heroId);

                return Ok();
            } catch(Exception e) {
                _logger.LogError(e, "Error occurred removing hero {heroId} from user deck {userId}", heroId, userId);
                return BadRequest("An error occurred removing hero from deck");
            }
        }
    }
}