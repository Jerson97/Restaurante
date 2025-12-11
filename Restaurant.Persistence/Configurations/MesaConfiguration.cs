using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class MesaConfiguration : IEntityTypeConfiguration<Mesa>
{
    public void Configure(EntityTypeBuilder<Mesa> builder)
    {

        builder.HasKey(m => m.Id);

        builder.Property(m => m.NumeroMesa)
            .IsRequired();

        builder.Property(m => m.Estado)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(m => m.Mozo)
            .WithMany()
            .HasForeignKey(m => m.MozoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Pedidos)
            .WithOne(p => p.Mesa)
            .HasForeignKey(p => p.MesaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
