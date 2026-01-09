namespace Restaurant.Domain.Entities
{
    public class PedidoDetalle : BaseEntityLog
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;
        public int PlatoId { get; set; }
        public Plato Plato { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
    }
}
