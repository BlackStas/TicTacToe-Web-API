using Buzile_TicTacToe.Models.Responses;

namespace Buzile_TicTacToe.Interfaces;

public interface IGameService
{
    Task<Guid> Create(CancellationToken token = default);
    Task<GameStateResponse> SetStep(Guid gameId, Guid playerId, int column, int row, CancellationToken token);
}