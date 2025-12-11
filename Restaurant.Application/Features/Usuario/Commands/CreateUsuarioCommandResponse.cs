namespace Restaurant.Application.Features.Usuario.Commands
{
    public class CreateUsuarioCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = "";
        public int? UsuarioId { get; set; }
    }
}
