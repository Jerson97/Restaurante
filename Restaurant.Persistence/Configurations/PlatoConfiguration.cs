using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

public class PlatoConfiguration : IEntityTypeConfiguration<Plato>
{
    public void Configure(EntityTypeBuilder<Plato> builder)
    {
        builder.ToTable("Plato");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Precio)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.HasOne(x => x.Categoria)
            .WithMany(c => c.Platos)
            .HasForeignKey(x => x.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
