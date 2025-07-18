using Microsoft.AspNetCore.Mvc;

namespace Buzile_TicTacToe.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(CancellationToken token)
    {
        return Ok();
    }
}