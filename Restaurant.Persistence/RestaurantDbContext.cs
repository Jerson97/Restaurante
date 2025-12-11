using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

namespace Restaurant.Persistence.Context
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }

        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Plato> Platos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar configuraciones
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestaurantDbContext).Assembly);

            //  Convertir tablas, columnas, claves e índices a minúsculas (recomendado para PostgreSQL)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Nombre de tabla en minúsculas
                entity.SetTableName(entity.GetTableName()?.ToLower());

                // Columnas en minúsculas
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }

                // Keys
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName()?.ToLower());
                }

                // Foreign Keys
                foreach (var fk in entity.GetForeignKeys())
                {
                    fk.SetConstraintName(fk.GetConstraintName()?.ToLower());
                }

                // Indexes
                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName()?.ToLower());
                }
            }
        }
    }
}
