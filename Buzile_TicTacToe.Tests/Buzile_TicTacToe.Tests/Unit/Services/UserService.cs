using Buzile_TicTacToe.Interfaces;
using Buzile_TicTacToe.Models;
using Buzile_TicTacToe.Services;
using Moq;
using Xunit;

namespace Buzile_TicTacToe.Tests.Unit.Services;


public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepoMock.Object);
    }
    
    [Fact]
    public async Task Get_WhenUserCreated_ShouldReturnCorrectUser()
    {
        // Arrange
        var login = "test_user";
        var expectedUserId = Guid.NewGuid();
        var token = CancellationToken.None;
        var createdUser = new User { Id = expectedUserId, Name = login };

        // Настраивание моков для последовательных вызовов
        _userRepoMock.SetupSequence(x => x.UserExists(login, token))
            .ReturnsAsync(false)    // Для Create
            .ReturnsAsync(true);    // Для Get

        _userRepoMock.Setup(x => x.Create(It.IsAny<User>(), token))
            .Callback<User, CancellationToken>((user, _) => user.Id = expectedUserId)
            .Returns(Task.CompletedTask);

        _userRepoMock.Setup(x => x.Get(login, token))
            .ReturnsAsync(createdUser);

        // Act
        var createdId = await _userService.Create(login, token);
        var fetchedUser = await _userService.Get(login, token);

        // Assert
        Assert.Equal(expectedUserId, createdId);
        Assert.Equal(createdUser, fetchedUser);
    }
}
