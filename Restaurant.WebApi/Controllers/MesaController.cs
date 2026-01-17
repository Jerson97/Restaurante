using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Domain.Result;
using static Restaurant.Application.Features.Mesa.Commands.Create.CreateMesaCommand;
using static Restaurant.Application.Features.Mesa.Commands.Update.LiberarMesaCommand;
using static Restaurant.Application.Features.Mesa.Commands.Update.OcuparMesaCommand;
using static Restaurant.Application.Features.Mesa.Queries.Get.PedidoActivoPorMesaQuery;
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

        [Authorize(Roles = "Mozo")]
        [HttpPut("{id}/ocupar")]
        public async Task<ActionResult<MessageResult<bool>>> OcuparMesa(int id)
        {
            return Ok(await Mediator.Send(new OcuparMesaCommandRequest { MesaId = id }));
        }

        [Authorize(Roles = "Mozo")]
        [HttpPut("{id}/liberar")]
        public async Task<ActionResult<MessageResult<bool>>> LiberarMesa(int id)
        {
            return Ok(await Mediator.Send(new LiberarMesaCommandRequest { MesaId = id }));
        }

        [Authorize(Roles = "Mozo")]
        [HttpGet("{id}/pedido-activo")]
        public async Task<ActionResult<MessageResult<bool>>> GetPedidoActivoPorMesa(int id)
        {
            return Ok(await Mediator.Send(new PedidoActivoPorMesaQueryRequest { MesaId = id }));
        }
    }
}
