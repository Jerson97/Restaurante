namespace Restaurant.Application.Dtos
{
    public class UsuarioLoginDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
    }

}
