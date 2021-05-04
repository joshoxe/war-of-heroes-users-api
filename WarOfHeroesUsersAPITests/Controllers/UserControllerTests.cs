using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using WarOfHeroesUsersAPI.Controllers;
using WarOfHeroesUsersAPI.Data;
using WarOfHeroesUsersAPI.Data.Entities;
using WarOfHeroesUsersAPI.Processing;
using WarOfHeroesUsersAPI.Users.Models;
using WarOfHeroesUsersAPI.Validation;

namespace WarOfHeroesUsersAPITests.Controllers
{
    public class UserControllerTests
    {
        private UserController _controller;
        private IUserRepository _fakeRepository;
        private IUserProcessor<GoogleUser> _processor;
        private IUserValidator _validator;
        private GoogleUser _validUser;

        [SetUp]
        public void Setup()
        {
            _fakeRepository = A.Fake<IUserRepository>();
            _validator = A.Fake<IUserValidator>();
            _processor = A.Fake<IUserProcessor<GoogleUser>>();
            _controller = new UserController(A.Fake<ILogger<UserController>>(), _validator, _processor,
                _fakeRepository);
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

        [Test]
        public void TestSuccessfulLoginReturnsOkAndUserObject()
        {
            var dbUser = new User
            {
                FirstName = "test",
                GoogleId = "test"
            };

            var userProcessingResult = new UserProcessingResult
            {
                IsValid = true,
                User = dbUser
            };

            A.CallTo(() => _validator.Validate(_validUser)).Returns(new ValidationResult());
            A.CallTo(() => _processor.Process(_validUser)).Returns(userProcessingResult);

            var result = (OkObjectResult) _controller.Login(_validUser);

            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(dbUser, result.Value);
        }

        [Test]
        public void TestInvalidUserReturnsBadRequest()
        {
            var dbUser = new User
            {
                FirstName = "test",
                GoogleId = "test"
            };

            var userProcessingResult = new UserProcessingResult
            {
                IsValid = true,
                User = dbUser
            };

            var invalidUser = _validUser;
            invalidUser.FirstName = "";

            A.CallTo(() => _validator.Validate(invalidUser)).Returns(new ValidationResult
                {Errors = {new ValidationFailure("firstName", "error")}});
            A.CallTo(() => _processor.Process(invalidUser)).Returns(userProcessingResult);

            var result = (BadRequestObjectResult) _controller.Login(invalidUser);

            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void TestBadProcessingReturnsBadRequest()
        {
            var userProcessingResult = new UserProcessingResult
            {
                IsValid = false,
            };

            A.CallTo(() => _validator.Validate(_validUser)).Returns(new ValidationResult());
            A.CallTo(() => _processor.Process(_validUser)).Returns(userProcessingResult);

            var result = (BadRequestObjectResult) _controller.Login(_validUser);

            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void TestGetUserInventoryReturnsOk()
        {
            var id = 1;
            A.CallTo(() => _fakeRepository.GetUserInventory(id)).Returns(new List<int> {1, 2, 3});

            var result = (ObjectResult) _controller.GetInventory(id);
            var inventory = (IEnumerable<int>) result.Value;

            Assert.AreEqual(200, result.StatusCode);
            CollectionAssert.AreEqual(new[] {1,2,3}, inventory);
        }

        [Test]
        public void TestGetUserInventoryReturnsNotFoundWhenListIsEmpty()
        {
            var id = 1;
            A.CallTo(() => _fakeRepository.GetUserInventory(id)).Returns(new List<int>());

            var emptyResult = (ObjectResult)_controller.GetInventory(id);

            Assert.AreEqual(404, emptyResult.StatusCode);
        }

        [Test]
        public void TestGetUserInventoryReturnsNotFoundWhenListIsNull()
        {
            var id = 1;
            A.CallTo(() => _fakeRepository.GetUserInventory(id)).Returns(null);

            var nullResult = (ObjectResult)_controller.GetInventory(id);

            Assert.AreEqual(404, nullResult.StatusCode);
        }

        [Test]
        public void TestGetUserInventoryReturnsBadRequestWhenExceptionThrown() {
            var id = 1;
            A.CallTo(() => _fakeRepository.GetUserInventory(id)).Throws<Exception>();

            var nullResult = (BadRequestResult)_controller.GetInventory(id);

            Assert.AreEqual(400, nullResult.StatusCode);
        }
    }
}