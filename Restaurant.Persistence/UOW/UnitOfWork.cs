using Microsoft.Extensions.Configuration;
using Restaurant.Application.Interfaces.IRepository;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Persistence.Repositories;

namespace Restaurant.Persistence.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        //private readonly RestaurantDbContext _context;
        private readonly IConfiguration configuration;
        private ICategoriaReposirory categoriaReposirory;
        private IPlatoRepository platoRepository;
        private IUsuarioRepository usuarioRepository;

        public UnitOfWork(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ICategoriaReposirory Categoria
        {
            get
            {
                if (categoriaReposirory is null) categoriaReposirory = new CategoriaReposirory(configuration);
                return categoriaReposirory;
            }
        }

        public IPlatoRepository Plato
        {
            get
            {
                if (platoRepository is null) platoRepository = new PlatoRepository(configuration);
                return platoRepository;
            }
        }

        public IUsuarioRepository Usuario
        {
            get
            {
                if (usuarioRepository is null) usuarioRepository = new UsuarioRepository(configuration);
                return usuarioRepository;
            }
        }
    }
}
