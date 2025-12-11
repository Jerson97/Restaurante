using Restaurant.Application.Interfaces.IRepository;

namespace Restaurant.Application.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoriaReposirory Categoria { get; }
        IPlatoRepository Plato { get; }
        IUsuarioRepository Usuario { get; }
    }
}
