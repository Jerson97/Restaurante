using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Domain.Enum;
using static Restaurant.Application.Features.Plato.Commands.Create.CreatePlatoCommand;
using static Restaurant.Application.Features.Plato.Commands.Delete.DeletePlatoCommand;
using static Restaurant.Application.Features.Plato.Commands.Update.UpdatePlatoCommand;
using static Restaurant.Application.Features.Plato.Queries.GetAll.PlatoQuery;
using static Restaurant.Application.Features.Plato.Queries.GetById.PlatoByIdQuery;

namespace Restaurant.Application.Interfaces.IRepository
{
    public interface IPlatoRepository
    {
        Task<(ServiceStatus, DataCollection<PlatoDto>, string)> GetAll(PlatoQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, PlatoDto, string)> GetPlatoById(PlatoByIdQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertPlato(CreatePlatoCommandRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> UpdatePlato(UpdatePlatoCommandRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> CancelPlato(DeletePlatoCommandRequest request, CancellationToken cancellationToken);
    }
}
