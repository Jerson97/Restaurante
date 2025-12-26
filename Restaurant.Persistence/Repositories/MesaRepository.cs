using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IRepository;
using Restaurant.Domain.Enum;
using static Restaurant.Application.Features.Mesa.Commands.Create.CreateMesaCommand;
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

        public async Task<(ServiceStatus, int?, string)> InsertMesa(CreateMesaCommandRequest request, int usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);

                var procedure = @"CALL public.usp_mesa_insert(
                    @p_numeromesa,
                    @p_fechacreacion,
                    @p_creadopor,
                    @p_id,
                    @p_result,
                    @p_mensaje
                );";

                var parametros = new DynamicParameters();

                parametros.Add("p_numeromesa", request.NumeroMesa);
                parametros.Add("p_fechacreacion", DateTime.UtcNow);
                parametros.Add("p_creadopor", usuarioId);

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
    }
}
