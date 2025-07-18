using Buzile_TicTacToe.Interfaces;
using Buzile_TicTacToe.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Buzile_TicTacToe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody]CreateUserRequest request, CancellationToken token = default)
    {
        return Ok(await _userService.Create(request.Login, token));
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]string login, CancellationToken token = default)
    {
        return Ok(await _userService.Get(login, token));
    }
}