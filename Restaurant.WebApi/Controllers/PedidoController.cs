using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Domain.Result;
using static Restaurant.Application.Features.Pedido.Commands.Create.AgregarPlatoAPedidoCommand;
using static Restaurant.Application.Features.Pedido.Commands.Create.CreatePedidoMesaCommand;
using static Restaurant.Application.Features.Pedido.Commands.Update.MarcarPedidoEntregadoCommand;
using static Restaurant.Application.Features.Pedido.Commands.Update.MarcarPedidoListoCommand;
using static Restaurant.Application.Features.Pedido.Commands.Update.MarcarPedidoPreparandoCommand;

namespace Restaurant.WebApi.Controllers
{
    [Route("api/Pedido")]
    [ApiController]
    public class PedidoController : MyBaseController
    {
        [Authorize(Roles = "Mozo")]
        [HttpPost]
        public async Task<ActionResult<MessageResult<int>>> CreatePedidoMesa([FromBody] CreatePedidoMesaCommandRequest request) => Ok(await Mediator.Send(request));

        [Authorize(Roles = "Mozo")]
        [HttpPost("AgregarPlato")]
        public async Task<ActionResult<MessageResult<bool>>> AddPlatoPedido([FromBody] AgregarPlatoAPedidoCommandRequest request) => Ok(await Mediator.Send(request));


        [Authorize(Roles = "Mozo")]
        [HttpPut("{id}/preparar")]
        public async Task<ActionResult<MessageResult<bool>>> MarkAsPedidoPreparando(int id)
        {
            return Ok(await Mediator.Send(new MarcarPedidoPreparandoCommandRequest { PedidoId = id }));

        }
        [Authorize(Roles = "Mozo")]
        [HttpPut("{id}/listo")]
        public async Task<ActionResult<MessageResult<bool>>> MarkAsPedidoListo(int id)
        {
            return Ok(await Mediator.Send(new MarcarPedidoListoCommandRequest { PedidoId = id }));
        }

        [Authorize(Roles = "Mozo")]
        [HttpPut("{id}/entregado")]
        public async Task<ActionResult<MessageResult<bool>>> MarkAsPedidoEntregado(int id)
        {
            return Ok(await Mediator.Send(new MarcarPedidoEntregadoCommandRequest { PedidoId = id }));
        }
    }
}
