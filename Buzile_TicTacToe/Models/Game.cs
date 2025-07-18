namespace Buzile_TicTacToe.Models;

public class Game
{
    public Guid Id { get; set; }
    public Guid? Player1Id { get; set; }
    public Guid? Player2Id { get; set; }
    public User Player1 { get; set; }
    public User Player2 { get; set; }
    public bool IsEnd { get; set; }
    public ICollection<GameStep> Steps { get; set; } = new List<GameStep>();
    
}