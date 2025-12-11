using System.Net;
using MediatR;
using Restaurant.Application.Dtos;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Categoria.Queries.GetById
{
    public class CategoriaByIdQuery
    {
        public class CategoriaByIdQueryRequest : IRequest<MessageResult<CategoriaDto>>
        {
            public int Id { get; set; }
        }

        public class CategoriaByIdQueryHandler : IRequestHandler<CategoriaByIdQueryRequest, MessageResult<CategoriaDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CategoriaByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MessageResult<CategoriaDto>> Handle(CategoriaByIdQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _unitOfWork.Categoria.GetCategoriaById(request, cancellationToken);

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

                return MessageResult<CategoriaDto>.Of(message, result);
            }
        }
    }
}
