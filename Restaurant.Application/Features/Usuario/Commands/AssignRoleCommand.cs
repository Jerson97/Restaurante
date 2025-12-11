using MediatR;
using Restaurant.Application.Interfaces.IUnitOfWork;

namespace Restaurant.Application.Features.Usuario.Commands
{
    public class AssignRoleCommand
    {
        public class AssignRoleCommandRequest : IRequest<AssignRoleCommandResponse>
        {
            public int UsuarioId { get; set; }
            public int RolId { get; set; }
        }

        public class AssignRoleCommandHandler :IRequestHandler<AssignRoleCommandRequest, AssignRoleCommandResponse>
        {
            private readonly IUnitOfWork _unitOfWork;

            public AssignRoleCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<AssignRoleCommandResponse> Handle(AssignRoleCommandRequest request, CancellationToken cancellationToken)
            {
                var response = new AssignRoleCommandResponse();

                var result = await _unitOfWork.Usuario.AssignRoleAsync(
                    request.UsuarioId,
                    request.RolId
                );

                if (result.Result != 1)
                {
                    response.Succeeded = false;
                    response.Message = result.Mensaje ?? "No se pudo asignar el rol";
                    return response;
                }

                response.Succeeded = true;
                response.Message = result.Mensaje ?? "Rol asignado correctamente";
                return response;
            }
        }
    }
}
