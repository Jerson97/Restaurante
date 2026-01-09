using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Domain.Result;
using static Restaurant.Application.Features.Categoria.Commands.Create.CreateCategoriaCommand;
using static Restaurant.Application.Features.Categoria.Commands.Delete.DeleteCategoriaCommand;
using static Restaurant.Application.Features.Categoria.Commands.Update.UpdateCategoriaCommand;
using static Restaurant.Application.Features.Categoria.Queries.GetAll.CategoriaQuery;
using static Restaurant.Application.Features.Categoria.Queries.GetById.CategoriaByIdQuery;

namespace Restaurant.WebApi.Controllers
{
    [Route("api/Categoria")]
    [ApiController]
    public class CategoriaController : MyBaseController
    {
        [HttpGet]
        public async Task<ActionResult<MessageResult<DataCollection<CategoriaDto>>>> GetAllCategoria([FromQuery] CategoriaQueryRequest request) => Ok(await Mediator.Send(request));

        [HttpGet("{id}")]
        public async Task<ActionResult<MessageResult<CategoriaDto>>> GetCategoriaById(int id)
        {
            return Ok(await Mediator.Send(new CategoriaByIdQueryRequest { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MessageResult<int>>> CreateCategoria([FromBody] CreateCategoriaCommandRequest request) => Ok(await Mediator.Send(request));

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<MessageResult<int>>> UpdateCategoria(int id, [FromBody] UpdateCategoriaCommandRequest request)
        {
            request.Id = id;
            return Ok(await Mediator.Send(request));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageResult<int>>> DeleteCategoria(int id)
        {
            return await Mediator.Send(new DeleteCategoriaCommandRequest { Id = id });
        }
    }
}