namespace Restaurant.Application.Dtos
{
    public class PedidoActivoMesaDto
    {
        public int PedidoId { get; set; }
        public int MesaId { get; set; }
        public string TipoPedido { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public decimal Total { get; set; }
        public int? MozoId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

}
