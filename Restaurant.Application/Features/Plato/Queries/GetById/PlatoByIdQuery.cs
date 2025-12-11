using System.Net;
using MediatR;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Plato.Queries.GetById
{
    public class PlatoByIdQuery
    {
        public class PlatoByIdQueryRequest :IRequest<MessageResult<PlatoDto>>
        {
            public int Id { get; set; }
        }

        public class PlatoByIdQueryHandler : IRequestHandler<PlatoByIdQueryRequest, MessageResult<PlatoDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public PlatoByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MessageResult<PlatoDto>> Handle(PlatoByIdQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _unitOfWork.Plato.GetPlatoById(request, cancellationToken);

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

                return MessageResult<PlatoDto>.Of(message, result);
            }
        }
    }
}
