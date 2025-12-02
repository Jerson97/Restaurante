using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("Rol");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Nombre)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(r => r.Nombre)
            .IsUnique(); 
    }
}
