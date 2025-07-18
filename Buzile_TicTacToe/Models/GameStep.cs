namespace Buzile_TicTacToe.Models;

public class GameStep
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid PlayerId { get; set; }
    public int Column { get; set; }
    public int Row { get; set; }
    public bool IsCross { get; set; }
    public DateTime StepTime { get; set; }
}