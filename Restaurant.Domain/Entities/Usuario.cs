namespace Restaurant.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PinHash { get; set; } = null!;

        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }

}
