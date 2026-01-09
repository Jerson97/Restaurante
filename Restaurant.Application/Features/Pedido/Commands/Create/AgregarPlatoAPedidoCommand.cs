using System.Net;
using MediatR;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Application.Interfaces.Security;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Pedido.Commands.Create
{
    public class AgregarPlatoAPedidoCommand
    {
        public class AgregarPlatoAPedidoCommandRequest : IRequest<MessageResult<bool>>
        {
            public int PedidoId { get; set; }
            public int PlatoId { get; set; }
            public int Cantidad { get; set; }
        }

        public class AgregarPlatoAPedidoCommandHandler: IRequestHandler<AgregarPlatoAPedidoCommandRequest, MessageResult<bool>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICurrentUserService _currentUserService;

            public AgregarPlatoAPedidoCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
            {
                _unitOfWork = unitOfWork;
                _currentUserService = currentUserService;
            }

            public async Task<MessageResult<bool>> Handle(AgregarPlatoAPedidoCommandRequest request, CancellationToken cancellationToken)
            {
                var usuarioId = _currentUserService.UsuarioId;

                if (!usuarioId.HasValue)
                {
                    throw new ErrorHandler(HttpStatusCode.Unauthorized, "Usuario no autenticado");
                }

                var (status, success, message) = await _unitOfWork.Pedido.AddPlatoToPedido(request.PedidoId, request.PlatoId, request.Cantidad, usuarioId.Value, cancellationToken);

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

                return MessageResult<bool>.Of(message, success);
            }
        }

    }
}
