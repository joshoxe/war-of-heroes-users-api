using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WarOfHeroesUsersAPI.Data;
using WarOfHeroesUsersAPI.Processing;
using WarOfHeroesUsersAPI.Users;
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

        [Route("/user/{userId}/logout")]
        [HttpPost]
        public ActionResult Logout([FromRoute] int userId, [FromBody] SessionAccessToken accessToken)
        {
            var user = _repository.GetUserByAccessToken(accessToken.AccessToken);

            if(user == null) {
                _logger.LogError($"Logout endpoint failed to find a user with access token: {accessToken.AccessToken}");
                return BadRequest("No user found for provided access token");
            }

            _repository.UpdateUserAccessToken(userId, "");

            return Ok();
        }

        [Route("/user/refresh")]
        [HttpPost]
        public ActionResult Refresh([FromBody] SessionAccessToken accessToken)
         {
            var user = _repository.GetUserByAccessToken(accessToken.AccessToken);

            if (user == null)
            {
                _logger.LogError($"Refresh endpoint failed to find a user with access token: {accessToken.AccessToken}");
                return BadRequest("No user found for provided access token");
            }

            return Ok(user);
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

        [Route("/user/{userId}/inventory/add")]
        [HttpPut]
        public ActionResult AddToInventory([FromBody] int[] ids, [FromRoute] int userId)
            {
            try
            {
                foreach (var id in ids)
                {
                    _repository.AddToUserInventory(userId, id);
                }
                return Ok();

            } catch (Exception e) {
                _logger.LogError(e, "Error occurred adding heroes");
                return BadRequest();
            }
        }


        [Route("/user/{userId}/deck/add/{heroId}")]
        [HttpGet]
        public ActionResult RemoveFromDeck([FromRoute] int userId, [FromRoute] int heroId)
        {
            try
            {
                if (!_repository.DeckContainsHero(userId, heroId))
                {
                    return BadRequest($"Deck for user {userId} does not contain hero with ID {heroId}");
                }

                _repository.RemoveFromUserDeck(userId, heroId);

                return Ok();
            }
            catch (Exception e)
            {

                _logger.LogError(e, "Error occurred removing hero {heroId} from user deck {userId}", heroId, userId);
                return BadRequest("An error occurred removing hero from deck");
            }
        }

        [Route("/user/{userId}/coins/add")]
        [HttpPut]
        public ActionResult RemoveFromCoins([FromBody] int coins, [FromRoute] int userId)
        {
            try
            {
                _repository.RemoveFromCoins(userId, coins);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }



        [Route("/user/{userId}/deck/update")]
        [HttpPost]
        public ActionResult UpdateDeck([FromRoute] int userId, [FromBody] int[] ids)
        {
            try
            {
                var userInventory = _repository.GetUserInventory(userId);
                var userDeck = new List<int>();
                userDeck.AddRange(_repository.GetUserDeck(userId));

                foreach (var heroId in ids)
                {
                    if (!userInventory.Contains(heroId))
                    {
                        // Hero isn't in the user's inventory, but check if it's already in their deck

                        if (userDeck.Contains(heroId))
                        {
                            // If it is in the deck, remove it from the next check
                            userDeck.Remove(heroId);
                        }
                        else
                        {
                            return BadRequest($"[ERROR] User with ID {userId} does not own hero with ID {heroId}");
                        }
                    }
                    else
                    {
                        // The user has the card in the inventory, so remove it
                        _repository.RemoveFromUserInventory(userId, heroId);
                    }
                }

                _repository.UpdateDeck(userId, ids);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred updating deck for user {userId} with deck {ids}", userId,
                    ids.ToString());
                return BadRequest(
                    $"[ERROR] An unknown error occurred while updating deck for user {userId} with deck {ids}");
            }
        }

        [Route("/user/{userId}/inventory/update")]
        [HttpPost]
        public ActionResult UpdateInventory([FromRoute] int userId, [FromBody] int[] ids)
        {
            try
            {
                // No secure check to ensure the request to change inventory is legit
                _repository.UpdateInventory(userId, ids);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred updating inventory for user {userId} with inventory {ids}",
                    userId,
                    ids.ToString());
                return BadRequest(
                    $"[ERROR] An unknown error occurred while updating inventory for user {userId} with inventory {ids}");
            }
        }

        [Route("/user/{userId}/win")]
        [HttpPut]
        public ActionResult AddUserWin([FromRoute] int userId, [FromHeader] string accessToken, [FromBody] int coins)
        {
            try
            {
                var user = _repository.GetUserByAccessToken(accessToken);

                if (user.Id != userId)
                {
                    return Unauthorized("You do not have access to this resource");
                }

                _repository.IncreaseUserWins(accessToken);
                _repository.GiveUserCoins(accessToken, coins);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while recording win for user {user}", userId);
                return BadRequest($"[ERROR] An unknown error occurred recording a win for user with ID {userId}");
            }
        }

        [Route("/user/{userId}/loss")]
        [HttpPut]
        public ActionResult AddUserLoss([FromRoute] int userId, [FromHeader] string accessToken, [FromBody] int coins)
        {

            try {
                var user = _repository.GetUserByAccessToken(accessToken);

                if(user.Id != userId) {
                    return Unauthorized("You do not have access to this resource");
                }

                _repository.IncreaseUserLosses(accessToken);
                _repository.GiveUserCoins(accessToken, coins);

                return Ok();
            } catch(Exception e) {
                _logger.LogError(e, "An error occurred while recording loss for user {user}", userId);
                return BadRequest($"[ERROR] An unknown error occurred recording a loss for user with ID {userId}");
            }
        }
    }
}