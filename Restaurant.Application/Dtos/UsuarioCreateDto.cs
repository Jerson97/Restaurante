namespace Restaurant.Application.Dtos
{
    public class UsuarioCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Pin { get; set; } = null!;
    }
}
