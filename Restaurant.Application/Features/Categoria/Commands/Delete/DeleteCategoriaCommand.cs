using System.Net;
using MediatR;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Categoria.Commands.Delete
{
    public class DeleteCategoriaCommand
    {
        public class DeleteCategoriaCommandRequest : IRequest<MessageResult<int>>
        {
            public int Id { get; set; }
        }

        public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommandRequest, MessageResult<int>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCategoriaCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MessageResult<int>> Handle(DeleteCategoriaCommandRequest request, CancellationToken cancellationToken)
            {
                var (status, categoria, message) = await _unitOfWork.Categoria.CancelCategoria(request, cancellationToken);

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
