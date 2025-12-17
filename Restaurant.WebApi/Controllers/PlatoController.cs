using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Domain.Result;
using static Restaurant.Application.Features.Plato.Commands.Create.CreatePlatoCommand;
using static Restaurant.Application.Features.Plato.Commands.Delete.DeletePlatoCommand;
using static Restaurant.Application.Features.Plato.Commands.Update.UpdatePlatoCommand;
using static Restaurant.Application.Features.Plato.Queries.GetAll.PlatoQuery;
using static Restaurant.Application.Features.Plato.Queries.GetById.PlatoByIdQuery;

namespace Restaurant.WebApi.Controllers
{
    [Route("api/Platos")]
    [ApiController]
    public class PlatoController : MyBaseController
    {
        [HttpGet]
        public async Task<ActionResult<MessageResult<DataCollection<PlatoDto>>>> GetAllCategoria([FromQuery] PlatoQueryRequest request) => Ok(await Mediator.Send(request));

        [HttpGet("{id}")]
        public async Task<ActionResult<MessageResult<PlatoDto>>> GetCategoriaById(int id)
        {
            return await Mediator.Send(new PlatoByIdQueryRequest { Id = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MessageResult<int>>> CreatePlato([FromBody] CreatePlatoCommandRequest request) => Ok(await Mediator.Send(request));

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<MessageResult<int>>> UpdatePlato(int id, [FromBody] UpdatePlatoCommandRequest request)
        {
            request.Id = id;
            return Ok(await Mediator.Send(request));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageResult<int>>> DeletePlato(int id)
        {
            return await Mediator.Send(new DeletePlatoCommandRequest { Id = id });
        }
    }
}
