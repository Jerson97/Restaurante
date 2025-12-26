using System.Net;
using MediatR;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Mesa.Queries.GetAll
{
    public class MesaQuery
    {
        public class MesaQueryRequest : IRequest<MessageResult<DataCollection<MesaDto>>>
        {
            public int Page { get; set; }
            public int Amount { get; set; }
        }

        public class MesaQueryHandler : IRequestHandler<MesaQueryRequest, MessageResult<DataCollection<MesaDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public MesaQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<MessageResult<DataCollection<MesaDto>>> Handle(MesaQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _unitOfWork.Mesa.GetAll(request, cancellationToken);

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

                return MessageResult<DataCollection<MesaDto>>.Of(message, result);
            }
        }
    }
}
