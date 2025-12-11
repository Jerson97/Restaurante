using System.Net;
using MediatR;
using Restaurant.Application.Common;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Categoria.Queries.GetAll
{
    public class CategoriaQuery
    {
        public class CategoriaQueryRequest : IRequest<MessageResult<DataCollection<CategoriaDto>>>
        {
            public int Page { get; set; }
            public int Amount { get; set; }
        }

        public class CategoriaQueryHandler : IRequestHandler<CategoriaQueryRequest, MessageResult<DataCollection<CategoriaDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CategoriaQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MessageResult<DataCollection<CategoriaDto>>> Handle(CategoriaQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _unitOfWork.Categoria.GetAll(request, cancellationToken);

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

                return MessageResult<DataCollection<CategoriaDto>>.Of(message, result);
            }
        }
    }
}
