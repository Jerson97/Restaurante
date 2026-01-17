using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IRepository;
using Restaurant.Domain.Enum;
using static Restaurant.Application.Features.Pedido.Commands.Create.CreatePedidoMesaCommand;
using static Restaurant.Application.Features.Pedido.Queries.Get.GetPedidosByEstadoQuery;

namespace Restaurant.Persistence.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _connectionString;

        public PedidoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresSQLConnection")!;
        }

        public async Task<(ServiceStatus, bool, string)> AddPlatoToPedido(int pedidoId, int platoId, int cantidad, int usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_pedido_agregar_plato(
                    @p_pedidoid,
                    @p_platoid,
                    @p_cantidad,
                    @p_usuarioid,
                    @p_fechaactualizacion,
                    @p_result,
                    @p_mensaje
                );";

                var parameters = new DynamicParameters();

                parameters.Add("p_pedidoid", pedidoId);
                parameters.Add("p_platoid", platoId);
                parameters.Add("p_cantidad", cantidad);
                parameters.Add("p_usuarioid", usuarioId);
                parameters.Add("p_fechaactualizacion", DateTime.UtcNow);

                parameters.Add("p_result", 0, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("p_mensaje", "", DbType.String, ParameterDirection.InputOutput, size: 200);

                await connection.ExecuteAsync(procedure, parameters, commandType: CommandType.Text);

                var result = parameters.Get<int>("p_result");
                var mensaje = parameters.Get<string>("p_mensaje");

                if (result != 1)
                    return (ServiceStatus.FailedValidation, false, mensaje);

                return (ServiceStatus.Ok, true, mensaje);
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, false,
                    $"Error al agregar plato al pedido -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<(ServiceStatus, int?, string)> CreatePedidoMesa(CreatePedidoMesaCommandRequest request, int usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_pedido_crear_mesa(
                    @p_mesaid,
                    @p_mozoid,
                    @p_fechacreacion,
                    @p_pedidoid,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_mesaid", request.MesaId);
                parametros.Add("p_fechacreacion", DateTime.UtcNow);
                parametros.Add("p_mozoid", usuarioId);

                parametros.Add("p_pedidoid", 0, DbType.Int32, ParameterDirection.InputOutput);
                parametros.Add("p_result", 0, DbType.Int32, ParameterDirection.InputOutput);
                parametros.Add("p_mensaje", "", DbType.String, ParameterDirection.InputOutput, size: 200);

                await connection.ExecuteAsync(procedure, parametros);

                var result = parametros.Get<int>("p_result");
                var mensaje = parametros.Get<string>("p_mensaje");
                var idCreado = parametros.Get<int>("p_pedidoid");


                if (result != 1)
                    return (ServiceStatus.FailedValidation, null, mensaje);

                return (ServiceStatus.Ok, idCreado, mensaje);
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null!, $"Error al crear el pedido -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, DataCollection<PedidoListadoDto>?, string)> GetByEstado(GetPedidosByEstadoQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var pedidos = (await connection.QueryAsync<PedidoListadoDto>(
                    "SELECT * FROM public.func_get_pedidos_by_estado(@p_estado, @p_page, @p_page_size)",
                    new 
                    { 
                        p_estado = request.Estado, 
                        p_page = request.Page, 
                        p_page_size = request.Amount 
                    },
                    commandType: CommandType.Text
                )).ToList();

                int total = await connection.QuerySingleAsync<int>(
                    "SELECT public.func_get_pedidos_by_estado_count(@p_estado)",
                    new { p_estado = request.Estado }
                );

                if (pedidos.Count == 0)
                    return (ServiceStatus.NotFound, null, "No hay pedidos para el estado indicado");

                var result = new DataCollection<PedidoListadoDto>
                {
                    Total = total,
                    Items = pedidos,
                    Page = request.Page,
                    Pages = (int)Math.Ceiling(total / (double)request.Amount)
                };

                return (ServiceStatus.Ok, result, "Succeeded");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null,
                    $"Error al listar pedidos por estado -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<(ServiceStatus, bool, string)> MarkPedidoAsEntregado(int pedidoId, int usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_pedido_marcar_entregado(
                    @p_pedidoid,
                    @p_usuarioid,
                    @p_fechaactualizacion,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_pedidoid", pedidoId);

                parametros.Add("p_usuarioid", usuarioId);
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
                    $"Error al cambiar estado del pedido -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, bool, string)> MarkPedidoAsListo(int pedidoId, int usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_pedido_marcar_listo(
                    @p_pedidoid,
                    @p_usuarioid,
                    @p_fechaactualizacion,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_pedidoid", pedidoId);

                parametros.Add("p_usuarioid", usuarioId);
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
                    $"Error al cambiar estado del pedido -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, bool, string)> MarkPedidoAsPreparando(int pedidoId, int usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_pedido_preparar(
                    @p_pedidoid,
                    @p_usuarioid,
                    @p_fechaactualizacion,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_pedidoid", pedidoId);

                parametros.Add("p_usuarioid", usuarioId);
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
                    $"Error al cambiar estado del pedido -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
