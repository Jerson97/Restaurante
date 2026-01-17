using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Domain.Enum;
using static Restaurant.Application.Features.Pedido.Commands.Create.CreatePedidoMesaCommand;
using static Restaurant.Application.Features.Pedido.Queries.Get.GetPedidosByEstadoQuery;

namespace Restaurant.Application.Interfaces.IRepository
{
    public interface IPedidoRepository
    {
        Task<(ServiceStatus, int?, string)> CreatePedidoMesa(CreatePedidoMesaCommandRequest request, int usuarioId, CancellationToken cancellationToken);
        Task<(ServiceStatus, bool, string)> AddPlatoToPedido(int pedidoId, int platoId, int cantidad, int usuarioId, CancellationToken cancellationToken);
        Task<(ServiceStatus, bool, string)> MarkPedidoAsPreparando(int pedidoId, int usuarioId, CancellationToken cancellationToken);
        Task<(ServiceStatus, bool, string)> MarkPedidoAsListo(int pedidoId, int usuarioId, CancellationToken cancellationToken);
        Task<(ServiceStatus, bool, string)> MarkPedidoAsEntregado(int pedidoId, int usuarioId, CancellationToken cancellationToken);
        Task<(ServiceStatus, DataCollection<PedidoListadoDto>?, string)> GetByEstado(GetPedidosByEstadoQueryRequest request, CancellationToken cancellationToken);

    }
}
