using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

public class Mesa : BaseEntityLog
{
    public int Id { get; set; }
    public int NumeroMesa { get; set; }
    public EstadoMesa Estado { get; set; }     // <-- ENUM
    public int? MozoId { get; set; }
    public Usuario? Mozo { get; set; }
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
