namespace Buzile_TicTacToe.Models.Responses;

public class GameStateResponse
{
    public Guid GameId { get; set; }
    public bool IsEnd { get; set; }
    public bool IsWinnable { get; set; }
    public bool IsDraw { get; set; }
    public Guid? WinnerId { get; set; }
}