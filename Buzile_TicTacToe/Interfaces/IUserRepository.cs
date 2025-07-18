using Buzile_TicTacToe.Models;

namespace Buzile_TicTacToe.Interfaces;

public interface IUserRepository
{
    Task Create(User user, CancellationToken token = default);
    Task<User> Get(string login, CancellationToken token = default);
    Task<User> Get(Guid id, CancellationToken token = default);
    Task<bool> UserExists(string login, CancellationToken token);
}