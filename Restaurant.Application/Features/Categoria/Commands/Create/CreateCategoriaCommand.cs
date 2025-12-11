using System.Net;
using MediatR;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Categoria.Commands.Create
{
    public class CreateCategoriaCommand
    {
        public class CreateCategoriaCommandRequest : IRequest<MessageResult<int>>
        {
            public string? Nombre { get; set; }
        }

        public class CreateCategoriaCommandHandler : IRequestHandler<CreateCategoriaCommandRequest, MessageResult<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CreateCategoriaCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<MessageResult<int>> Handle(CreateCategoriaCommandRequest request, CancellationToken cancellationToken)
            {
                var (status, categoria, message) = await _unitOfWork.Categoria.InsertCategoria(request, cancellationToken);

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
