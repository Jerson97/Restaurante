namespace Restaurant.Application.Interfaces.Token
{
    public interface IJwtGenerator
    {
        string GenerateToken(int usuarioId, string nombre, List<string> roles);
    }
}
