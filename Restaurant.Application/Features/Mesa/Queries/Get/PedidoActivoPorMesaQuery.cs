using System.Net;
using MediatR;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Mesa.Queries.Get
{
    public class PedidoActivoPorMesaQuery
    {
        public class PedidoActivoPorMesaQueryRequest : IRequest<MessageResult<PedidoActivoMesaDto>>
        {
            public int MesaId { get; set; }
        }

        public class PedidoActivoPorMesaQueryHandler
            : IRequestHandler<PedidoActivoPorMesaQueryRequest, MessageResult<PedidoActivoMesaDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public PedidoActivoPorMesaQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MessageResult<PedidoActivoMesaDto>> Handle(
                PedidoActivoPorMesaQueryRequest request,
                CancellationToken cancellationToken)
            {
                var (status, pedido, message) =
                    await _unitOfWork.Mesa.GetPedidoActivoPorMesa(request.MesaId, cancellationToken);

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

                return MessageResult<PedidoActivoMesaDto>.Of(message, pedido!);
            }
        }
    }
}
