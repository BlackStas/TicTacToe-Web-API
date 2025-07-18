using Buzile_TicTacToe.DBContext;
using Buzile_TicTacToe.Interfaces;
using Buzile_TicTacToe.Models;
using Microsoft.EntityFrameworkCore;

namespace Buzile_TicTacToe.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ApplicationContext _applicationContext;

    public GameRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Create(Game game, CancellationToken token = default)
    {
        _applicationContext.Set<Game>().Add(game);
        await _applicationContext.SaveChangesAsync(token);
    }

    public async Task<Game> GetGame(Guid gameId, CancellationToken token = default)
    {
        return await _applicationContext.Set<Game>()
            .Include(x => x.Player1)
            .Include(x=> x.Player2)
            .Include(x=>x.Steps)
            .FirstOrDefaultAsync(x => x.Id == gameId, token);
    }
    
    public async Task<bool> GameExists(Guid gameId, CancellationToken token = default)
    {
        return await _applicationContext.Set<Game>().AnyAsync(x => x.Id == gameId, token);
    }

    public async Task Update(Game game, CancellationToken token)
    {
        _applicationContext.Set<Game>().Update(game);
        await _applicationContext.SaveChangesAsync(token);
    }
}