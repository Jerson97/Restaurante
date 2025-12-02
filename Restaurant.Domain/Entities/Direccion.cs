namespace Restaurant.Domain.Entities
{
    public class Direccion
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;
        public string NombreCliente { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string DireccionCompleta { get; set; } = null!;
        public string? Referencia { get; set; }
    }
}
