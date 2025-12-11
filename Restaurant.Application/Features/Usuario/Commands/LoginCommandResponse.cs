using Restaurant.Application.Dtos;

namespace Restaurant.Application.Features.Usuario.Commands
{
    public class LoginCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; }
        public UsuarioLoginDto? Usuario { get; set; }
    }

}
