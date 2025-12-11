using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IRepository;

namespace Restaurant.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;
        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresSQLConnection")!;
        }

        public async Task<AssignRoleResultDto> AssignRoleAsync(int usuarioId, int rolId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var procedure = @"CALL public.usp_usuario_rol_asignar(
                @p_usuarioid,
                @p_rolid,
                @p_result,
                @p_mensaje
            );";

            var parameters = new DynamicParameters();
            parameters.Add("p_usuarioid", usuarioId);
            parameters.Add("p_rolid", rolId);

            parameters.Add("p_result", 0, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("p_mensaje", "", DbType.String, ParameterDirection.InputOutput, size: 200);

            await connection.ExecuteAsync(procedure, parameters);

            return new AssignRoleResultDto
            {
                Result = parameters.Get<int>("p_result"),
                Mensaje = parameters.Get<string>("p_mensaje")
            };
        }

        public async Task<CreateUsuarioResultDto> CreateUsuarioAsync(string nombre, string email, string pinHash)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var procedure = @"CALL public.usp_usuario_insert(
                @p_nombre,
                @p_email,
                @p_pinhash,
                @p_usuarioid,
                @p_result,
                @p_mensaje
            );";

            var parameters = new DynamicParameters();
            parameters.Add("p_nombre", nombre);
            parameters.Add("p_email", email);
            parameters.Add("p_pinhash", pinHash);

            parameters.Add("p_usuarioid", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            parameters.Add("p_result", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            parameters.Add("p_mensaje", dbType: DbType.String, direction: ParameterDirection.InputOutput);

            await connection.ExecuteAsync(procedure, parameters);

            return new CreateUsuarioResultDto
            {
                UsuarioId = parameters.Get<int?>("p_usuarioid"),
                Result = parameters.Get<int>("p_result"),
                Mensaje = parameters.Get<string>("p_mensaje")
            };
        }

        public async Task<LoginResultDto> LoginAsync(string email, string pinHash)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var procedure = @"CALL public.usp_usuario_login(
                @p_email,
                @p_pinhash,
                @p_usuarioid,
                @p_nombre,
                @p_roles,
                @p_result,
                @p_mensaje
            );";

            var parameters = new DynamicParameters();
            parameters.Add("p_email", email);
            parameters.Add("p_pinhash", pinHash);

            // OUT parameters need initial value
            parameters.Add("p_usuarioid", 0, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("p_nombre", "", DbType.String, ParameterDirection.InputOutput, size: 200);
            parameters.Add("p_roles", "", DbType.String, ParameterDirection.InputOutput, size: 200);

            parameters.Add("p_result", 0, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("p_mensaje", "", DbType.String, ParameterDirection.InputOutput, size: 200);

            await connection.ExecuteAsync(procedure, parameters);

            return new LoginResultDto
            {
                UsuarioId = parameters.Get<int?>("p_usuarioid"),
                Nombre = parameters.Get<string>("p_nombre"),
                Roles = parameters.Get<string>("p_roles"),
                Result = parameters.Get<int>("p_result"),
                Mensaje = parameters.Get<string>("p_mensaje")
            };
        }
    }
}
