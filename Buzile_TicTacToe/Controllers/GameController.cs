using Buzile_TicTacToe.Interfaces;
using Buzile_TicTacToe.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Buzile_TicTacToe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CancellationToken token)
    {
        return Ok(await _gameService.Create(token));
    }

    [HttpPost("SetStep")]
    public async Task<IActionResult> SetStep([FromBody]SetStepRequest request, CancellationToken token)
    {
        return Ok(await _gameService.SetStep(request.GameId, request.PlayerId, request.Column, request.Row, token));
    }
}