using MediatR;
using Restaurant.Application.Common.Helpers;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Application.Interfaces.Token;

namespace Restaurant.Application.Features.Usuario.Commands
{
    public class LoginCommand
    {
        public class LoginCommandRequest : IRequest<LoginCommandResponse>
        {
            public string Email { get; set; } = null!;
            public string Pin { get; set; } = null!;
        }

        public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IJwtGenerator _jwtGenerator;

            public LoginCommandHandler(IJwtGenerator jwtGenerator, IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _jwtGenerator = jwtGenerator;
            }
            public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
            {
                var response = new LoginCommandResponse();

                string pinHash = HashHelper.HashPin(request.Pin.Trim().Replace(" ", ""));

                var loginResult = await _unitOfWork.Usuario.LoginAsync(request.Email, pinHash);

                if (loginResult.Result != 1)
                {
                    response.Succeeded = false;
                    response.Message = loginResult.Mensaje!;
                    return response;
                }

                List<string> roles = string.IsNullOrWhiteSpace(loginResult.Roles)
                    ? new List<string>()
                    : loginResult.Roles.Split(',').ToList();
                string token = _jwtGenerator.GenerateToken(
                    loginResult.UsuarioId!.Value,
                    loginResult.Nombre!,
                    roles
                );

                var usuarioDto = new UsuarioLoginDto
                {
                    UsuarioId = loginResult.UsuarioId.Value,
                    Nombre = loginResult.Nombre!,
                    Roles = roles
                };

                response.Succeeded = true;
                response.Message = "Login exitoso";
                response.Token = token;
                response.Usuario = usuarioDto;

                return response;
            }
        }
    }
}
