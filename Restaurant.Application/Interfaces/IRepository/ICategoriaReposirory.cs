using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Domain.Enum;
using static Restaurant.Application.Features.Categoria.Commands.Create.CreateCategoriaCommand;
using static Restaurant.Application.Features.Categoria.Commands.Delete.DeleteCategoriaCommand;
using static Restaurant.Application.Features.Categoria.Commands.Update.UpdateCategoriaCommand;
using static Restaurant.Application.Features.Categoria.Queries.GetAll.CategoriaQuery;
using static Restaurant.Application.Features.Categoria.Queries.GetById.CategoriaByIdQuery;

namespace Restaurant.Application.Interfaces.IRepository
{
    public interface ICategoriaReposirory
    {
        Task<(ServiceStatus, DataCollection<CategoriaDto>, string)> GetAll(CategoriaQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, CategoriaDto, string)> GetCategoriaById(CategoriaByIdQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertCategoria(CreateCategoriaCommandRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> UpdateCategoria(UpdateCategoriaCommandRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> CancelCategoria(DeleteCategoriaCommandRequest request, CancellationToken cancellationToken);
    }
}
