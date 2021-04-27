using System;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using WarOfHeroesUsersAPI.Data;
using WarOfHeroesUsersAPI.Data.Entities;
using WarOfHeroesUsersAPI.Processing;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPITests.Processing
{
    [TestFixture]
    public class GoogleUserProcessorTests
    {
        [SetUp]
        public void SetUp()
        {
            _repository = A.Fake<IUserRepository>();
            _processor = new GoogleUserProcessor(A.Fake<ILogger<GoogleUserProcessor>>(), _repository);
            _validUser = new GoogleUser
            {
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

        private IUserRepository _repository;
        private GoogleUserProcessor _processor;
        private GoogleUser _validUser;

        [Test]
        public void TestProcessNewUserReturnsValidResult()
        {
            var dbUser = new User
            {
                FirstName = "test",
                GoogleId = "test"
            };
            A.CallTo(() => _repository.GetUserByGoogleId(_validUser.ID)).Returns(null);
            var result = _processor.Process(_validUser);;

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(dbUser.FirstName, result.User.FirstName);
            Assert.AreEqual(dbUser.GoogleId, result.User.GoogleId);
        }

        [Test]
        public void TestProcessReturnsInvalidWhenExceptionThrown()
        {
            A.CallTo(() => _repository.GetUserByGoogleId("test")).Throws<Exception>();

            var result = _processor.Process(_validUser);

            Assert.IsFalse(result.IsValid);
            Assert.IsNull(result.User);
        }
    }
}