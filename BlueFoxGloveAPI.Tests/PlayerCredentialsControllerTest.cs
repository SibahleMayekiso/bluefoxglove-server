using BlueFoxGloveAPI.Controllers;
using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class PlayerCredentialsControllerTest
    {
        private PlayerCredentialController _playerCredentialController;
        private IPlayerCredentialsRepository _playerCredentialsRepository;

        [SetUp]
        public void Setup()
        {
            _playerCredentialsRepository = Substitute.For<IPlayerCredentialsRepository>();
            _playerCredentialController = new PlayerCredentialController(_playerCredentialsRepository);
        }
        [Test]

        public async Task CreatePlayer_WhenANewPlayerIsCreated_ReturnsSuccessStatusCode()
        {
            //Arrange
            var expected = 201;
            var newPlayerCredentials = new PlayerCredentials
            {
                PlayerId = "64dd1cf27a6922a9502fc90a",
                PlayerName = "Jane Doe"
            };

            _playerCredentialsRepository
           .CreatePlayer(Arg.Any<PlayerCredentials>())
           .Returns(Task.FromResult(newPlayerCredentials));

            //Act 
            var result = await _playerCredentialController.CreatePlayer(newPlayerCredentials) as CreatedAtActionResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [Test]

        public async Task CreatePlayer_WhenPlayerWithAnEmptyNameIsCreated_ReturnsABadRequestError()
        {
            //Arrange
            var expected = 400;
            var newPlayerCredentials = new PlayerCredentials
            {
                PlayerId = "",
                PlayerName = ""
            };

            _playerCredentialsRepository
           .CreatePlayer(Arg.Any<PlayerCredentials>())
           .Returns(Task.FromResult(newPlayerCredentials));

            //Act 
            var result = await _playerCredentialController.CreatePlayer(newPlayerCredentials) as ObjectResult;
            var actual = result?.StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}