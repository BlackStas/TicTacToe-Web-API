using Buzile_TicTacToe.Models;

namespace Buzile_TicTacToe.Interfaces;

public interface IGameStepRepository
{
    Task Add(GameStep step);
    
}