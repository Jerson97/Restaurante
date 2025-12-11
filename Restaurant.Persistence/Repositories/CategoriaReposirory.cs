using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IRepository;
using Restaurant.Domain.Enum;
using static Restaurant.Application.Features.Categoria.Commands.Create.CreateCategoriaCommand;
using static Restaurant.Application.Features.Categoria.Commands.Delete.DeleteCategoriaCommand;
using static Restaurant.Application.Features.Categoria.Commands.Update.UpdateCategoriaCommand;
using static Restaurant.Application.Features.Categoria.Queries.GetAll.CategoriaQuery;
using static Restaurant.Application.Features.Categoria.Queries.GetById.CategoriaByIdQuery;

namespace Restaurant.Persistence.Repositories
{
    public class CategoriaReposirory : ICategoriaReposirory
    {
        private readonly string _connectionString;

        public CategoriaReposirory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresSQLConnection")!;
        }

        public async Task<(ServiceStatus, int?, string)> CancelCategoria(DeleteCategoriaCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_categoria_anular(@p_id, @p_actualizadopor, @p_fechaactualizacion,@p_result,@p_mensaje)";

                var parametros = new DynamicParameters();
                parametros.Add("p_id", request.Id);
                parametros.Add("p_actualizadopor", 1);
                parametros.Add("p_fechaactualizacion", DateTime.UtcNow);

                parametros.Add("p_result", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parametros.Add("p_mensaje", dbType: DbType.String, direction: ParameterDirection.InputOutput);

                var response = await connection.ExecuteAsync(procedure, parametros);

                int resultado = parametros.Get<int>("@p_result");
                string mensaje = parametros.Get<string>("@p_mensaje");

                if (resultado == 0)
                    return (ServiceStatus.NotFound, null, mensaje);

                if (resultado != 1) { return (ServiceStatus.FailedValidation, null, $"No se pudo anular la categoria : {mensaje}"); }

                return (ServiceStatus.Ok, request.Id, mensaje);


            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error al anular la categoria -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, CategoriaDto, string)> GetCategoriaById(CategoriaByIdQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var categoria = await connection.QueryFirstOrDefaultAsync<CategoriaDto>(
                    "SELECT * FROM public.func_get_categoria_by_id(@p_id)",
                    new { p_id = request.Id },
                    commandType: CommandType.Text
                );

                if (categoria == null)
                    return (ServiceStatus.NotFound, null!, "No se encontró la categoría");

                return (ServiceStatus.Ok, categoria, "Categoría encontrada con éxito");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, new CategoriaDto(), $"Error al consultar la categoría -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, DataCollection<CategoriaDto>, string)> GetAll(CategoriaQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var categorias = (await connection.QueryAsync<CategoriaDto>(
                    "SELECT * FROM public.func_get_categorias(@p_page, @p_page_size)",
                    new
                    {
                        p_page = request.Page,
                        p_page_size = request.Amount
                    },
                    commandType: CommandType.Text
                )).ToList();

                int total = await connection.QuerySingleAsync<int>(
                    "SELECT public.func_get_categorias_count()",
                    commandType: CommandType.Text
                );

                if (categorias.Count == 0)
                    return (ServiceStatus.NotFound, null!, "No hay registros para mostrar");

                var result = new DataCollection<CategoriaDto>
                {
                    Total = total,
                    Items = categorias,
                    Page = request.Page,
                    Pages = (int)Math.Ceiling(total / (double)request.Amount)
                };

                return (ServiceStatus.Ok, result, "Succeeded");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null!,
                    $"Error al consultar categorías -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }




        public async Task<(ServiceStatus, int?, string)> InsertCategoria(CreateCategoriaCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_categoria_insert(
                    @p_nombre,
                    @p_fechacreacion,
                    @p_creadopor,
                    @p_id,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_nombre", request.Nombre);
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
                return (ServiceStatus.InternalError, null!, $"Error al crear categoria -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> UpdateCategoria(UpdateCategoriaCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_categoria_update(
                    @p_id,
                    @p_nombre,
                    @p_actualizadopor,
                    @p_fechaactualizacion,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_id", request.Id, DbType.Int32, ParameterDirection.InputOutput);

                parametros.Add("p_nombre", request.Nombre);
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
                return (ServiceStatus.InternalError, null!, $"Error al actualizar categoria -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

    }
}
