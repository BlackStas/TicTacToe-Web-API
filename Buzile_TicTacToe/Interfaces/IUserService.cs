using Buzile_TicTacToe.Models;

namespace Buzile_TicTacToe.Interfaces;

public interface IUserService
{
    Task<Guid> Create(string login, CancellationToken token = default);
    Task<User> Get(string login, CancellationToken token = default);
    
}