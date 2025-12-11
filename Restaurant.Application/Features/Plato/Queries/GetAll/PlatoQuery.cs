using System.Net;
using MediatR;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Plato.Queries.GetAll
{
    public class PlatoQuery
    {
        public class PlatoQueryRequest : IRequest<MessageResult<DataCollection<PlatoDto>>>
        {
            public int Page { get; set; }
            public int Amount { get; set; }
        }

        public class PlatoQueryHandler : IRequestHandler<PlatoQueryRequest, MessageResult<DataCollection<PlatoDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public PlatoQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<MessageResult<DataCollection<PlatoDto>>> Handle(PlatoQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _unitOfWork.Plato.GetAll(request, cancellationToken);

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

                return MessageResult<DataCollection<PlatoDto>>.Of(message, result);
            }
        }
    }
}
