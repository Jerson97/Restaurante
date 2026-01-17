using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Domain.Enum;
using static Restaurant.Application.Features.Mesa.Commands.Create.CreateMesaCommand;
using static Restaurant.Application.Features.Mesa.Commands.Update.LiberarMesaCommand;
using static Restaurant.Application.Features.Mesa.Commands.Update.OcuparMesaCommand;
using static Restaurant.Application.Features.Mesa.Queries.GetAll.MesaQuery;

namespace Restaurant.Application.Interfaces.IRepository
{
    public interface IMesaRepository
    {
        Task<(ServiceStatus, DataCollection<MesaDto>, string)> GetAll(MesaQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertMesa(CreateMesaCommandRequest request, int usuarioId, CancellationToken cancellationToken);
        Task<(ServiceStatus, bool, string)> OcuparMesa(OcuparMesaCommandRequest request, int usuarioId, CancellationToken cancellationToken);
        Task<(ServiceStatus, bool, string)> LiberarMesa(LiberarMesaCommandRequest request, int usuarioId, CancellationToken cancellationToken);
        Task<(ServiceStatus, PedidoActivoMesaDto?, string)> GetPedidoActivoPorMesa(int mesaId, CancellationToken cancellationToken);
    }
}
