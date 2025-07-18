using Buzile_TicTacToe.DBContext;
using Buzile_TicTacToe.Interfaces;
using Buzile_TicTacToe.Models;

namespace Buzile_TicTacToe.Repositories;

public class GameStepRepository : IGameStepRepository
{
    private readonly ApplicationContext _applicationContext;

    public GameStepRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Add(GameStep step)
    {
        _applicationContext.Set<GameStep>().Add(step);
        await _applicationContext.SaveChangesAsync();
    }
}