using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        // Tabla
        builder.ToTable("Pedido");

        // Campos simples
        builder.Property(p => p.TipoPedido)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Estado)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();


        builder.Property(p => p.ClienteNombre)
            .HasMaxLength(100);

        builder.Property(p => p.ClienteTelefono)
            .HasMaxLength(9);

        builder.Property(p => p.Total)
            .HasColumnType("decimal(10,2)");

        // -----------------------------
        // Relaciones
        // -----------------------------

        // Pedido -> Mesa (muchos pedidos pueden pertenecer a una mesa)
        builder.HasOne(p => p.Mesa)
            .WithMany(m => m.Pedidos)
            .HasForeignKey(p => p.MesaId)
            .OnDelete(DeleteBehavior.Restrict); // Evitamos cascadas indeseadas

        // Pedido -> Mozo (Usuario)
        builder.HasOne(p => p.Mozo)
            .WithMany() // Si luego quieres inversa, la agregamos
            .HasForeignKey(p => p.MozoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Pedido -> Repartidor (Usuario)
        builder.HasOne(p => p.Repartidor)
            .WithMany()
            .HasForeignKey(p => p.RepartidorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Pedido -> Direccion (1:1)
        builder.HasOne(p => p.Direccion)
            .WithOne(d => d.Pedido)
            .HasForeignKey<Direccion>(d => d.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Pedido -> PedidoDetalle (1:N)
        builder.HasMany(p => p.Detalles)
            .WithOne(d => d.Pedido)
            .HasForeignKey(d => d.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
