using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

public class PedidoDetalleConfiguration : IEntityTypeConfiguration<PedidoDetalle>
{
    public void Configure(EntityTypeBuilder<PedidoDetalle> builder)
    {
        builder.ToTable("PedidoDetalle");

        builder.HasKey(pd => pd.Id);

        builder.Property(pd => pd.Cantidad)
            .IsRequired();

        builder.Property(pd => pd.Precio)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(pd => pd.Subtotal)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // Relación con Pedido
        builder.HasOne(pd => pd.Pedido)
            .WithMany(p => p.Detalles)
            .HasForeignKey(pd => pd.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación con Plato
        builder.HasOne(pd => pd.Plato)
            .WithMany()
            .HasForeignKey(pd => pd.PlatoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
