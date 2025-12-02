using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

public class Pedido : BaseEntityLog
{
    public int Id { get; set; }

    public TipoPedido TipoPedido { get; set; }        // enum
    public int? MesaId { get; set; }
    public Mesa? Mesa { get; set; }

    public EstadoPedido Estado { get; set; }          // enum

    public string? ClienteNombre { get; set; }
    public string? ClienteTelefono { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public decimal Total { get; set; }

    public int? MozoId { get; set; }
    public Usuario? Mozo { get; set; }

    public int? RepartidorId { get; set; }
    public Usuario? Repartidor { get; set; }

    public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    public Direccion? Direccion { get; set; }
}
