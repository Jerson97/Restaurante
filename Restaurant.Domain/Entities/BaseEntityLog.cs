namespace Restaurant.Domain.Entities
{
    public class BaseEntityLog
    {
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
        public bool Eliminado { get; set; } = false;

        public int? CreadoPor { get; set; }
        public int? ActualizadoPor { get; set; }
    }
}
