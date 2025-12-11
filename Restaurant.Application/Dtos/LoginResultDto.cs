namespace Restaurant.Application.Dtos
{
    public class LoginResultDto
    {
        public int? UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? Roles { get; set; }
        public int Result { get; set; }
        public string? Mensaje { get; set; }
    }

}
