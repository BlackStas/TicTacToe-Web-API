using Buzile_TicTacToe.DBContext;
using Buzile_TicTacToe.Repositories;
using Buzile_TicTacToe.Services;
using Buzile_TicTacToe.Models;

using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Buzile_TicTacToe.Tests.Integration;

public class UserServiceIntegrationTests : IDisposable
{
    private readonly TestsApplicationContext _dbContext;
    private readonly UserService _userService;

    public UserServiceIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TestsApplicationContext(options);

        // 2. Инициализация сервиса с реальным репозиторием
        var userRepository = new UserRepository(_dbContext);
        _userService = new UserService(userRepository);
    }

    [Fact]
    public async Task CreateUser_Should_SaveToInMemoryDatabase()
    {
        // Arrange
        var login = "test_user";

        // Act
        var userId = await _userService.Create(login, CancellationToken.None);
        var userFromDb = await _dbContext.Users.FindAsync(userId);

        // Assert
        Assert.NotNull(userFromDb);
        Assert.Equal(login, userFromDb.Name);
    }

    [Fact]
    public async Task GetUser_Should_ReturnUser_FromInMemoryDatabase()
    {
        // Arrange
        var login = "existing_user";
        var user = new User { Id = Guid.NewGuid(), Name = login };
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _userService.Get(login, CancellationToken.None);

        // Assert
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(login, result.Name);
    }

    [Fact]
    public async Task CreateUser_WhenLoginExists_ShouldThrowException()
    {
        // Arrange
        var login = "duplicate_user";
        await _userService.Create(login, CancellationToken.None);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            _userService.Create(login, CancellationToken.None));
    }

    [Fact]
    public async Task GetUser_WhenNotExists_ShouldThrowException()
    {
        // Arrange
        var login = "non_existing_user";

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() =>
            _userService.Get(login, CancellationToken.None));
        Assert.Equal($"Пользователь с логином {login} не зарегистрирован!", ex.Message);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}