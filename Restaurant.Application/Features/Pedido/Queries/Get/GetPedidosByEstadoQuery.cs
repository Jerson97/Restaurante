using System.Net;
using MediatR;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Pedido.Queries.Get
{
    public class GetPedidosByEstadoQuery
    {
        public class GetPedidosByEstadoQueryRequest : IRequest<MessageResult<DataCollection<PedidoListadoDto>>>
        {
            public string Estado { get; set; } = null!;
            public int Page { get; set; }
            public int Amount { get; set; }
        }

        public class Handler : IRequestHandler<GetPedidosByEstadoQueryRequest, MessageResult<DataCollection<PedidoListadoDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MessageResult<DataCollection<PedidoListadoDto>>> Handle(GetPedidosByEstadoQueryRequest request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.Estado))
                    throw new ErrorHandler(HttpStatusCode.BadRequest, "Estado es requerido");

                if (request.Page <= 0 || request.Amount <= 0)
                    throw new ErrorHandler(HttpStatusCode.BadRequest, "Page y Amount deben ser mayores a 0");

                var (status, data, message) = await _unitOfWork.Pedido.GetByEstado(request, cancellationToken);

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

                return MessageResult<DataCollection<PedidoListadoDto>>.Of(message, data!);
            }
        }
    }

}
