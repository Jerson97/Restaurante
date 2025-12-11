namespace Restaurant.Domain.Entities
{
    public class Plato : BaseEntityLog
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
    }
}
