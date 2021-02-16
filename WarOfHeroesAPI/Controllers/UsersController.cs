using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WarOfHeroesAPI.Validation;
using FluentValidation;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WarOfHeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly AbstractValidator<User> _validator;
        private readonly FakeDatabase _database;

        public UsersController(ILogger<UsersController> logger, AbstractValidator<User> validator, FakeDatabase database)
        {
            _logger = logger;
            _validator = validator;
            _database = database;
        }

        // GET api/<UsersController>/5
        [HttpPost("login")]
        public IActionResult Login([FromBody] object body)
        {
            var userFromBody = JsonConvert.DeserializeObject<User>(body.ToString());
            var validationResult = _validator.Validate(userFromBody);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid login request {body}", body);
                return BadRequest("Invalid login request");
            }


        }
    }
}
