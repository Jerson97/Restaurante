using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Features.Usuario.Commands;
using static Restaurant.Application.Features.Usuario.Commands.AssignRoleCommand;
using static Restaurant.Application.Features.Usuario.Commands.CreateUsuarioCommand;
using static Restaurant.Application.Features.Usuario.Commands.LoginCommand;

namespace Restaurant.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : MyBaseController
    {
        [HttpPost("register")]
        public async Task<ActionResult<CreateUsuarioCommandResponse>> Register(CreateUsuarioCommandRequest request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommandRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Mediator.Send(request);

            if (!result.Succeeded)
                return Unauthorized(result); 

            return Ok(result);
        }

        [HttpPost("asignar-rol")]
        // [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<AssignRoleCommandResponse>> AsignarRol([FromBody] AssignRoleCommandRequest request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
