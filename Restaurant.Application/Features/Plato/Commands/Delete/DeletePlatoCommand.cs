using System.Net;
using MediatR;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Application.Interfaces.Security;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Plato.Commands.Delete
{
    public class DeletePlatoCommand
    {
        public class DeletePlatoCommandRequest : IRequest<MessageResult<int>>
        {
            public int Id { get; set; }
        }

        public class DeletePlatoCommandHandler : IRequestHandler<DeletePlatoCommandRequest, MessageResult<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICurrentUserService _currentUserService;
            public DeletePlatoCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
            {
                _unitOfWork = unitOfWork;
                _currentUserService = currentUserService;
            }
            public async Task<MessageResult<int>> Handle(DeletePlatoCommandRequest request, CancellationToken cancellationToken)
            {
                var usuarioId = _currentUserService.UsuarioId;

                if (!usuarioId.HasValue)
                {
                    throw new ErrorHandler(HttpStatusCode.Unauthorized, "Usuario no autenticado");
                }

                var (status, categoria, message) = await _unitOfWork.Plato.CancelPlato(request, usuarioId.Value, cancellationToken);

                if (status != ServiceStatus.Ok)
                {
                    HttpStatusCode code = status switch
                    {
                        ServiceStatus.NotFound => HttpStatusCode.NotFound,
                        ServiceStatus.FailedValidation => HttpStatusCode.BadRequest,
                        ServiceStatus.Forbidden => HttpStatusCode.Forbidden,
                        ServiceStatus.UnprocessableEntity => HttpStatusCode.UnprocessableEntity,
                        _ => HttpStatusCode.InternalServerError
                    };

                    throw new ErrorHandler(code, message);
                }

                return MessageResult<int>.Of(message, categoria!.Value);
            }
        }
    }
}
