namespace Buzile_TicTacToe.Models.Requests;

public class SetStepRequest
{
    public Guid PlayerId { get; set; }
    public Guid GameId { get; set; }
    public int Column { get; set; }
    public int Row { get; set; }
}