using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IRepository;
using Restaurant.Domain.Enum;
using static Restaurant.Application.Features.Plato.Commands.Create.CreatePlatoCommand;
using static Restaurant.Application.Features.Plato.Commands.Delete.DeletePlatoCommand;
using static Restaurant.Application.Features.Plato.Commands.Update.UpdatePlatoCommand;
using static Restaurant.Application.Features.Plato.Queries.GetAll.PlatoQuery;
using static Restaurant.Application.Features.Plato.Queries.GetById.PlatoByIdQuery;

namespace Restaurant.Persistence.Repositories
{
    public class PlatoRepository : IPlatoRepository
    {
        private readonly string _connectionString;

        public PlatoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresSQLConnection")!;
        }

        public async Task<(ServiceStatus, int?, string)> CancelPlato(DeletePlatoCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_plato_anular(
                    @p_id, 
                    @p_actualizadopor, 
                    @p_fechaactualizacion,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();
                parametros.Add("p_id", request.Id);
                parametros.Add("p_actualizadopor", 1);
                parametros.Add("p_fechaactualizacion", DateTime.UtcNow);

                parametros.Add("p_result", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parametros.Add("p_mensaje", dbType: DbType.String, direction: ParameterDirection.InputOutput);

                await connection.ExecuteAsync(procedure, parametros);

                int resultado = parametros.Get<int>("p_result");
                string mensaje = parametros.Get<string>("p_mensaje");

                if (resultado == 0)
                    return (ServiceStatus.NotFound, null, mensaje);

                return (ServiceStatus.Ok, request.Id, mensaje);
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null,
                    $"Error al anular el plato -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<(ServiceStatus, DataCollection<PlatoDto>, string)> GetAll(PlatoQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var platos = (await connection.QueryAsync<PlatoDto>(
                    "SELECT * FROM public.func_get_platos(@p_page, @p_page_size)",
                    new
                    {
                        p_page = request.Page,
                        p_page_size = request.Amount
                    },
                    commandType: CommandType.Text
                )).ToList();

                int total = await connection.QuerySingleAsync<int>(
                    "SELECT public.func_get_platos_count()",
                    commandType: CommandType.Text
                );

                if (platos.Count == 0)
                    return (ServiceStatus.Ok, new DataCollection<PlatoDto>
                    {
                        Total = 0,
                        Items = new List<PlatoDto>(),
                        Page = request.Page,
                        Pages = 0
                    }, "No hay registros");

                var result = new DataCollection<PlatoDto>
                {
                    Total = total,
                    Items = platos,
                    Page = request.Page,
                    Pages = (int)Math.Ceiling(total / (double)request.Amount)
                };

                return (ServiceStatus.Ok, result, "Succeeded");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null!,
                    $"Error al consultar platos -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, PlatoDto, string)> GetPlatoById(PlatoByIdQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var plato = await connection.QueryFirstOrDefaultAsync<PlatoDto>(
                    "SELECT * FROM public.func_get_plato_by_id(@p_id)",
                    new { p_id = request.Id },
                    commandType: CommandType.Text
                );

                if (plato == null)
                    return (ServiceStatus.NotFound, null!, "No se encontró el plato");

                return (ServiceStatus.Ok, plato, "Plato encontrado con éxito");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, new PlatoDto(), $"Error al consultar el plato -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> InsertPlato(CreatePlatoCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_plato_insert(
                    @p_nombre,
                    @p_precio,
                    @p_categoriaid,
                    @p_fechacreacion,
                    @p_creadopor,
                    @p_id,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_nombre", request.Nombre);
                parametros.Add("p_precio", request.Precio);
                parametros.Add("p_categoriaid", request.CategoriaId);
                parametros.Add("p_fechacreacion", DateTime.UtcNow);
                parametros.Add("p_creadopor", 1);

                parametros.Add("p_id", 0, DbType.Int32, ParameterDirection.InputOutput);
                parametros.Add("p_result", 0, DbType.Int32, ParameterDirection.InputOutput);
                parametros.Add("p_mensaje", "", DbType.String, ParameterDirection.InputOutput, size: 200);

                await connection.ExecuteAsync(procedure, parametros);

                var result = parametros.Get<int>("p_result");
                var mensaje = parametros.Get<string>("p_mensaje");
                var idCreado = parametros.Get<int>("p_id");


                if (result != 1)
                    return (ServiceStatus.FailedValidation, null, mensaje);

                return (ServiceStatus.Ok, idCreado, mensaje);
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null!, $"Error al crear plato -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> UpdatePlato(UpdatePlatoCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_plato_update(
                    @p_id,
                    @p_nombre,
                    @p_precio,
                    @p_categoriaid,
                    @p_actualizadopor,
                    @p_fechaactualizacion,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_id", request.Id, DbType.Int32, ParameterDirection.InputOutput);
                parametros.Add("p_nombre", request.Nombre);
                parametros.Add("p_precio", request.Precio);
                parametros.Add("p_categoriaid", request.CategoriaId);
                parametros.Add("p_actualizadopor", 1);
                parametros.Add("p_fechaactualizacion", DateTime.UtcNow);
                parametros.Add("p_result", 0, DbType.Int32, ParameterDirection.InputOutput);
                parametros.Add("p_mensaje", "", DbType.String, ParameterDirection.InputOutput, size: 200);

                await connection.ExecuteAsync(procedure, parametros);

                var result = parametros.Get<int>("p_result");
                var mensaje = parametros.Get<string>("p_mensaje");
                var id = parametros.Get<int>("p_id");

                if (result != 1)
                    return (ServiceStatus.FailedValidation, null, mensaje);

                return (ServiceStatus.Ok, id, mensaje);
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null!, $"Error al actualizar plato -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
