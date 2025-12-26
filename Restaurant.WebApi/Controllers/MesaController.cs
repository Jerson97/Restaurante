using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Domain.Result;
using static Restaurant.Application.Features.Mesa.Commands.Create.CreateMesaCommand;
using static Restaurant.Application.Features.Mesa.Queries.GetAll.MesaQuery;

namespace Restaurant.WebApi.Controllers
{
    [Route("api/Mesa")]
    [ApiController]
    public class MesaController : MyBaseController
    {
        [Authorize(Roles = "Admin,Mozo")]
        [HttpGet]
        public async Task<ActionResult<MessageResult<DataCollection<MesaDto>>>> GetAllCategoria([FromQuery] MesaQueryRequest request) => Ok(await Mediator.Send(request));

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MessageResult<int>>> CreateMesa([FromBody] CreateMesaCommandRequest request) => Ok(await Mediator.Send(request));
    }
}
