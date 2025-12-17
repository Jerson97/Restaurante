using Restaurant.Application.Interfaces.Security;
using System.Security.Claims;

namespace Restaurant.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UsuarioId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return null;

                return int.Parse(userIdClaim);
            }
        }
    }
}