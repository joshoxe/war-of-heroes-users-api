using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using WarOfHeroesUsersAPI.Processing;
using WarOfHeroesUsersAPI.Users;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPITests.Processing
{
    [TestFixture]
    public class GoogleUserProcessorTests
    {
        [SetUp]
        public void SetUp()
        {
            _userStore = A.Fake<IUserStore>();
            _processor = new GoogleUserProcessor(_userStore, A.Fake<ILogger<GoogleUserProcessor>>());
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

        private IUserStore _userStore;
        private GoogleUserProcessor _processor;
        private GoogleUser _validUser;

        [Test]
        public void TestProcessNewUserReturnsValidResult()
        {
            var dbUser = new DbUser
            {
                FirstName = "test",
                GoogleID = "test"
            };
            var result = _processor.Process(_validUser);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(dbUser.FirstName, result.User.FirstName);
            Assert.AreEqual(dbUser.GoogleID, result.User.GoogleID);
        }
    }
}