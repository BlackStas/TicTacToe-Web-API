using Buzile_TicTacToe.Models;
using Buzile_TicTacToe.Models.Requests;

namespace Buzile_TicTacToe.Interfaces;

public interface IGameRepository
{
    Task Create(Game game, CancellationToken token);
    Task<Game> GetGame(Guid gameId, CancellationToken token);
    Task<bool> GameExists(Guid gameId, CancellationToken token);
    Task Update(Game game, CancellationToken token);

}