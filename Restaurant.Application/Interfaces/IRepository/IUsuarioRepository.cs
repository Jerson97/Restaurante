using Restaurant.Application.Dtos;

namespace Restaurant.Application.Interfaces.IRepository
{
    public interface IUsuarioRepository
    {
        Task<LoginResultDto> LoginAsync(string email, string pinHash);
        Task<CreateUsuarioResultDto> CreateUsuarioAsync(string nombre, string email, string pinHash);
        Task<AssignRoleResultDto> AssignRoleAsync(int usuarioId, int rolId);
    }

}
