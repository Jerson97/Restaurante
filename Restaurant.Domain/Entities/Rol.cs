namespace Restaurant.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}
