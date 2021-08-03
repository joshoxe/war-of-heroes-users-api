using NUnit.Framework;
using WarOfHeroesUsersAPI.Users.Models;
using WarOfHeroesUsersAPI.Validation;

namespace WarOfHeroesUsersAPITests.Validation
{
    [TestFixture()]
    public class GoogleUserValidatorTests
    {
        private GoogleUser _validUser;
        private GoogleUserValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new GoogleUserValidator();
            _validUser = new GoogleUser {
                FirstName = "test",
                ID = "test",
                Email = "test",
                Name = "test",
                PhotoUrl = "test",
                IdToken = "test",
                AuthorizationCode = "test",
                Response = "test",
                AuthToken = "test",
                LastName = "test",
                Provider = "test"
            };
        }

        [Test()]
        public void TestValidationPassesWithValidUser()
        {
           var result = _validator.Validate(_validUser);

           Assert.IsTrue(result.IsValid);
           Assert.IsEmpty(result.Errors);
        }

        [TestCase(null, "A", "A", "A", "A", "A", "A", "A", "A", "A", "A")]
        [TestCase("A", null, "A", "A", "A", "A", "A", "A", "A", "A", "A")]
        [TestCase("A", "A", null, "A", "A", "A", "A", "A", "A", "A", "A")]
        [TestCase("A", "A", "A", null, "A", "A", "A", "A", "A", "A", "A")]
        [TestCase("A", "A", "A", "A", "A", null, "A", "A", "A", "A", "A")]
        [TestCase("A", "A", "A", "A", "A", "A", "A", null, "A", "A", "A")]
        [TestCase("A", "A", "A", "A", "A", "A", "A", "A", null, "A", "A")]
        [TestCase("A", "A", "A", "A", "A", "A", "A", "A", "A", "A", null)]
        public void TestValidationFailsWhenIncorrectValuesSupplied(string firstName, string id, string email,
            string name, string photoUrl, string idToken, string authorizationCode, string response, string authToken,
            string lastName, string provider)
        {
            var invalidUser = new GoogleUser
            {
                FirstName = firstName,
                ID = id,
                Email = email,
                Name = name,
                PhotoUrl = photoUrl,
                IdToken = idToken,
                AuthorizationCode = authorizationCode,
                Response = response,
                AuthToken = authToken,
                LastName = lastName,
                Provider = provider
            };

            var result = _validator.Validate(invalidUser);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
        }
    }
}