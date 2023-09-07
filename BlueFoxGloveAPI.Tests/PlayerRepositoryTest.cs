namespace BlueFoxGloveAPI.Tests
{
    [TestFixture]
    public class PlayerRepositoryTest
    {
        private IPlayerRepository _playerRepository;
        private Player _player;

        [SetUp]
        public void Setup()
        {
            _playerRepository = Substitute.For<IPlayerRepository>();
            _player = new Player { PlayerId = "64dd1cf27a6922a9502fc8be", PlayerName = "Jane Doe" };
        }

    }
    [Test]
    public async Task GetPlayerByIdAsync_WhenPlayerWithValidIdExists_ReturnsPlayer()
    {
        // Arrange
        var expected = _player;

        _playerRepository.GetPlayerByIdAsync(expected.PlayerId).Returns(expected);

        // Act
        var result = await _playerRepository.GetPlayerByIdAsync(expected.PlayerId);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public async Task GetPlayerByIdAsync_WhenPlayerDoesNotExist_ReturnsNull()
    {
        // Arrange
        string playerId = "nonExistentPlayerId";
        _playerRepository.GetPlayerByIdAsync(playerId).ReturnsNull();

        // Act
        var result = await _playerRepository.GetPlayerByIdAsync(playerId);

        // Assert
        Assert.IsNull(result);
    }
}
}