using Buzile_TicTacToe.Interfaces;
using Buzile_TicTacToe.Models;
using Buzile_TicTacToe.Services;
using Moq;
using Xunit;

namespace Buzile_TicTacToe.Tests.Unit.Services;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _gameRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IGameStepRepository> _gameStepRepoMock;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        // Создаем моки для всех зависимостей
        _gameRepoMock = new Mock<IGameRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _gameStepRepoMock = new Mock<IGameStepRepository>();
        
        // Создаем экземпляр тестируемого сервиса
        _gameService = new GameService(
            _gameRepoMock.Object,
            _userRepoMock.Object,
            _gameStepRepoMock.Object);
    }

    [Fact]
    public async Task Create_Should_ReturnGuid()
    {
        // Arrange
        var token = CancellationToken.None;
        
        // Act
        var result = await _gameService.Create(token);

        // Assert
        Assert.IsType<Guid>(result);
    }

    [Fact]
    public async Task SetStep_WhenGameNotExists_ShouldThrowException()
    {
        // Arrange
        var token = CancellationToken.None;
        var gameId = Guid.NewGuid(); //сгенерируется несуществующая в бд игра
        var playerId = Guid.NewGuid();
        var row = 0;
        var column = 0;
        
        // Act & Assert

        var ex = await Assert.ThrowsAsync<Exception>(() =>
            _gameService.SetStep(gameId, playerId, column, row, token));

        Assert.Equal("Данной игры не существует!", ex.Message);
    }
    
    
    [Fact]
    public async Task SetStep_WhenGameIsEnded_ShouldThrowException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var game = new Game { IsEnd = true };
        var row = 0;
        var column = 0;
        var token = CancellationToken.None;

        
        _gameRepoMock.Setup(x => x.GameExists(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _gameRepoMock.Setup(x => x.GetGame(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => 
            _gameService.SetStep(gameId, playerId, column, row, token));
        Assert.Equal("Ваша игра уже закончена!", ex.Message);
    }
    
    [Fact]
    public async Task IsCorrectUserForSetStep_WhenUserNotInGame_ShouldThrowException()
    {
        // Arrange
        var row = 0;
        var column = 0;
        var gameId = Guid.NewGuid();
        var token = CancellationToken.None;

        
        var game = new Game
        {
            Id = gameId,
            Player1Id = Guid.NewGuid(),
            Player2Id = Guid.NewGuid()
        };
        var wrongPlayerId = Guid.NewGuid();
        
        _gameRepoMock.Setup(x => x.GameExists(gameId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _gameRepoMock.Setup(x => x.GetGame(gameId, It.IsAny<CancellationToken>())).ReturnsAsync(game);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => 
            _gameService.SetStep(gameId, wrongPlayerId, column, row, token));
        Assert.Equal("Вы не являетесь игроком данной партии!", ex.Message);
    }
    
    [Fact]
    public async Task SetStep_WhenUserNotFound_ThrowsException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var game = new Game { Id = gameId };

        _gameRepoMock.Setup(x => x.GameExists(gameId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _gameRepoMock.Setup(x => x.GetGame(gameId, It.IsAny<CancellationToken>())).ReturnsAsync(game);
        _userRepoMock.Setup(x => x.Get(playerId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => 
            _gameService.SetStep(gameId, playerId, 0, 0, CancellationToken.None));
        Assert.Contains($"Игрок с идентификатором {playerId} не найден!", ex.Message);
    }
    
    [Fact]
    public async Task SetStep_WhenPlayerTriesDoubleMove_ThrowsException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var game = new Game 
        { 
            Id = gameId,
            Player1Id = playerId,
            Steps = new List<GameStep> { new GameStep { PlayerId = playerId } }
        };

        _gameRepoMock.Setup(x => x.GameExists(gameId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _gameRepoMock.Setup(x => x.GetGame(gameId, It.IsAny<CancellationToken>())).ReturnsAsync(game);
        _userRepoMock.Setup(x => x.Get(playerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = playerId });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => 
            _gameService.SetStep(gameId, playerId, 0, 0, CancellationToken.None));
        Assert.Equal("Сейчас не ваш ход!", ex.Message);
    }
}