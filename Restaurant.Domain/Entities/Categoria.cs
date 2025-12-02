namespace Restaurant.Domain.Entities
{
    public class Categoria : BaseEntityLog
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        // Relación inversa
        public ICollection<Plato> Platos { get; set; } = new List<Plato>();
    }
}
