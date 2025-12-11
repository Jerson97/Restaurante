using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

public class DireccionConfiguration : IEntityTypeConfiguration<Direccion>
{
    public void Configure(EntityTypeBuilder<Direccion> builder)
    {

        builder.HasKey(d => d.Id);

        builder.Property(d => d.NombreCliente)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Telefono)
            .IsRequired()
            .HasMaxLength(9);

        builder.Property(d => d.DireccionCompleta)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Referencia)
            .HasMaxLength(200);

        builder.HasOne(d => d.Pedido)
            .WithOne(p => p.Direccion)
            .HasForeignKey<Direccion>(d => d.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
