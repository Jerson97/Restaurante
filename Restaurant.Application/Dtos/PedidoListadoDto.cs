namespace Restaurant.Application.Dtos
{
    public class PedidoListadoDto
    {
        public int Id { get; set; }
        public string TipoPedido { get; set; } = null!;
        public int? MesaId { get; set; }
        public string Estado { get; set; } = null!;
        public decimal Total { get; set; }
        public int? MozoId { get; set; }
        //public int? RepartidorId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

}
