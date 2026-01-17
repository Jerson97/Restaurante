using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IRepository;
using Restaurant.Domain.Enum;
using static Restaurant.Application.Features.Mesa.Commands.Create.CreateMesaCommand;
using static Restaurant.Application.Features.Mesa.Commands.Update.LiberarMesaCommand;
using static Restaurant.Application.Features.Mesa.Commands.Update.OcuparMesaCommand;
using static Restaurant.Application.Features.Mesa.Queries.GetAll.MesaQuery;

namespace Restaurant.Persistence.Repositories
{
    public class MesaRepository : IMesaRepository
    {
        private readonly string _connectionString;

        public MesaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresSQLConnection")!;
        }

        public async Task<(ServiceStatus, DataCollection<MesaDto>, string)> GetAll(MesaQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var mesas = (await connection.QueryAsync<MesaDto>(
                    "SELECT * FROM public.func_get_mesas(@p_page, @p_page_size)",
                    new
                    {
                        p_page = request.Page,
                        p_page_size = request.Amount
                    },
                    commandType: CommandType.Text
                )).ToList();

                int total = await connection.QuerySingleAsync<int>(
                    "SELECT public.func_get_mesas_count()",
                    commandType: CommandType.Text
                );

                if (mesas.Count == 0)
                    return (ServiceStatus.NotFound, null!, "No hay registros para mostrar");

                var result = new DataCollection<MesaDto>
                {
                    Total = total,
                    Items = mesas,
                    Page = request.Page,
                    Pages = (int)Math.Ceiling(total / (double)request.Amount)
                };

                return (ServiceStatus.Ok, result, "Succeeded");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null!,
                    $"Error al consultar mesas -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, PedidoActivoMesaDto?, string)> GetPedidoActivoPorMesa(int mesaId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var pedido = await connection.QueryFirstOrDefaultAsync<PedidoActivoMesaDto>(
                    "SELECT * FROM public.func_get_pedido_activo_por_mesa(@p_mesaid)",
                    new { p_mesaid = mesaId },
                    commandType: CommandType.Text
                );

                if (pedido == null)
                    return (ServiceStatus.NotFound, null, "La mesa no tiene pedido activo");

                return (ServiceStatus.Ok, pedido, "Succeeded");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null,
                    $"Error al consultar pedido activo -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> InsertMesa(CreateMesaCommandRequest request, int usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_mesa_insert(
                    @p_numeromesa,
                    @p_fechacreacion,
                    @p_mozoid,
                    @p_id,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_numeromesa", request.NumeroMesa);
                parametros.Add("p_fechacreacion", DateTime.UtcNow);
                parametros.Add("p_mozoid", usuarioId);

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
                return (ServiceStatus.InternalError, null!, $"Error al crear mesa -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, bool, string)> LiberarMesa(LiberarMesaCommandRequest request, int usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_mesa_liberar(
                    @p_mesaid,
                    @p_mozoid,
                    @p_fechaactualizacion,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_mesaid", request.MesaId);
                parametros.Add("p_mozoid", usuarioId);
                parametros.Add("p_fechaactualizacion", DateTime.UtcNow);

                parametros.Add("p_result", 0, DbType.Int32, ParameterDirection.InputOutput);
                parametros.Add("p_mensaje", "", DbType.String, ParameterDirection.InputOutput, size: 200);

                await connection.ExecuteAsync(procedure, parametros);

                var result = parametros.Get<int>("p_result");
                var mensaje = parametros.Get<string>("p_mensaje");

                if (result != 1)
                    return (ServiceStatus.FailedValidation, false, mensaje);

                return (ServiceStatus.Ok, true, mensaje);
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, false,
                    $"Error al liberar mesa -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, bool, string)> OcuparMesa(OcuparMesaCommandRequest request, int usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_mesa_ocupar(
                    @p_mesaid,
                    @p_mozoid,
                    @p_fechaactualizacion,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_mesaid", request.MesaId);
                parametros.Add("p_mozoid", usuarioId);
                parametros.Add("p_fechaactualizacion", DateTime.UtcNow);

                parametros.Add("p_result", 0, DbType.Int32, ParameterDirection.InputOutput);
                parametros.Add("p_mensaje", "", DbType.String, ParameterDirection.InputOutput, size: 200);

                await connection.ExecuteAsync(procedure, parametros);

                var result = parametros.Get<int>("p_result");
                var mensaje = parametros.Get<string>("p_mensaje");

                if (result != 1)
                    return (ServiceStatus.FailedValidation, false, mensaje);

                return (ServiceStatus.Ok, true, mensaje);

            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, false,
                    $"Error al ocupar mesa -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
