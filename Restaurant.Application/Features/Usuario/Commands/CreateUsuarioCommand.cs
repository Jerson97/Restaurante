using MediatR;
using Restaurant.Application.Common.Helpers;
using Restaurant.Application.Interfaces.IUnitOfWork;

namespace Restaurant.Application.Features.Usuario.Commands
{
    public class CreateUsuarioCommand
    {
        public class CreateUsuarioCommandRequest : IRequest<CreateUsuarioCommandResponse>
        {
            public string Nombre { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Pin { get; set; } = null!;
        }

        public class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommandRequest, CreateUsuarioCommandResponse>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateUsuarioCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<CreateUsuarioCommandResponse> Handle(CreateUsuarioCommandRequest request, CancellationToken cancellationToken)
            {
                var response = new CreateUsuarioCommandResponse();

                string pinHash = HashHelper.HashPin(request.Pin);

                var result = await _unitOfWork.Usuario.CreateUsuarioAsync(
                    request.Nombre,
                    request.Email,
                    pinHash
                );

                if (result.Result != 1)
                {
                    response.Succeeded = false;
                    response.Message = result.Mensaje!;
                    return response;
                }

                response.Succeeded = true;
                response.UsuarioId = result.UsuarioId;
                response.Message = "Usuario creado exitosamente";
                return response;
            }
        }
    }
}
