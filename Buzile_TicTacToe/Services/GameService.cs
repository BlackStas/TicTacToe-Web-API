using Buzile_TicTacToe.Interfaces;
using Buzile_TicTacToe.Models;
using Buzile_TicTacToe.Models.Responses;

namespace Buzile_TicTacToe.Services;

public class GameService : IGameService
{
    private const int BOARD_SIZE = 3;
    private const int COUNT_TO_THE_WIN = 3; // >3>board_size
    private readonly IGameRepository _gameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGameStepRepository _gameStepRepository;

    public GameService(IGameRepository gameRepository, IUserRepository userRepository, IGameStepRepository gameStepRepository)
    {
        _gameRepository = gameRepository;
        _userRepository = userRepository;
        _gameStepRepository = gameStepRepository;
    }

    public async Task<Guid> Create(CancellationToken token = default)
    {
        var game = new Game
        {
            Id = Guid.NewGuid()
        };
        await _gameRepository.Create(game, token);
        return game.Id;
    }

    public async Task<GameStateResponse> SetStep(Guid gameId, Guid playerId, int column, int row, CancellationToken token)
    {
        if (!await _gameRepository.GameExists(gameId, token))
        {
            throw new Exception("Данной игры не существует!");
        }

        var game = await _gameRepository.GetGame(gameId, token);

        if (game.IsEnd)
        {
            throw new Exception("Ваша игра уже закончена!");
        }

        if (!await IsCorrectUserForSetStep(game, playerId, token))
        {
            throw new Exception("Упс, что-то пошло не так D:");
        }

        if (game.Steps.Any(x=>x.Row == row && x.Column == column))
        {
            throw new Exception("Эта ячейка занята!");
        }

        // проверка фигуры, которая должна быть в текущей очереди
        var isCross = game.Steps.Count % 2 != 1;
        
        // реализация доп условия о вероятности поставить фигуру соперника каждый третий ход
        if (game.Steps.Count > 0 && game.Steps.Count % 3 == 0)
        {
            var random = new Random();
            
            if (random.Next(0, 10) == 1)
            {
                isCross = !isCross;
            }
        }

        var newStep = new GameStep
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            PlayerId = playerId,
            Column = column,
            Row = row,
            IsCross = isCross,
            StepTime = DateTime.UtcNow
        };
        await _gameStepRepository.Add(newStep);

        var win = IsWinningMove(newStep, game.Steps.ToList(), COUNT_TO_THE_WIN);

        // финальные переменные с результатами, которые пойдут в ответ
        var winGame = false;
        var drawGame = false;
        Guid? winner = null;
        
        if (win)
        {
            game.IsEnd = true;
            winGame = true;
            winner = game.Steps.FirstOrDefault(x=>x.IsCross == isCross).PlayerId;
        }

        if (game.Steps.Count == BOARD_SIZE * BOARD_SIZE)
        {
            game.IsEnd = true;
            drawGame = true;
        }

        await _gameRepository.Update(game, token);
        
        return new GameStateResponse
        {
            GameId = gameId,
            IsEnd = game.IsEnd,
            IsWinnable = winGame,
            IsDraw = drawGame,
            WinnerId = winner
        };
    }

    private bool IsWinningMove(GameStep lastStep, List<GameStep> steps, int winLength)
    {

        var currentFigureStep = steps
            .Where(x => x.IsCross == lastStep.IsCross)
            .ToHashSet(); 

        var directions = new (int directionRow, int directionCol)[]
        {
            (0, 1),   //  горизонталь
            (1, 0),   //  вертикаль
            (1, 1),   //  диагональ
            (1, -1),  //  обратная диагональ
        };

        foreach (var (directionRow, directionCol) in directions)
        {
            int count = 1; // учитывая lastStep

            count += CountInDirection(lastStep, directionRow, directionCol, currentFigureStep);
            count += CountInDirection(lastStep, -directionRow, -directionCol, currentFigureStep);

            if (count >= winLength)
                return true;
        }

        return false;
    }

    private int CountInDirection(GameStep origin, int directionRow, int directionCol, HashSet<GameStep> steps)
    {
        int count = 0;
        int row = origin.Row + directionRow;
        int col = origin.Column + directionCol;

        while (steps.Any(x => x.Row == row && x.Column == col))
        {
            count++;
            row += directionRow;
            col += directionCol;
        }

        return count;
    }

    private async Task<bool> IsCorrectUserForSetStep(Game game, Guid playerId, CancellationToken token)
    {
        if (game.Player1Id != null && game.Player1Id != playerId
                                   && game.Player2Id != null && game.Player2Id != playerId)
        {
            throw new Exception("Вы не являетесь игроком данной партии!");
        }

        var player = await _userRepository.Get(playerId, token);
        
        if (player == null)
        {
            throw new Exception($"Игрок с идентификатором {playerId} не найден!");
        }
        
        if (game.Player1Id == null)
        {
            game.Player1 = player;
            game.Player1Id = playerId;
            await _gameRepository.Update(game, token);
        }
        else if (game.Player2Id == null && game.Player1Id != playerId)
        {
            game.Player2 = player;
            game.Player2Id = playerId;
            await _gameRepository.Update(game, token);
        }

        if (game.Steps.Count > 0 && game.Steps.Last().PlayerId == playerId)
        {
            throw new Exception("Сейчас не ваш ход!");
        }

        return true;
    }
    
}