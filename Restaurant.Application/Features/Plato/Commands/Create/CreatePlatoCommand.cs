using System.Net;
using MediatR;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Plato.Commands.Create
{
    public class CreatePlatoCommand 
    {
        public class CreatePlatoCommandRequest : IRequest<MessageResult<int>>
        {
            public string? Nombre { get; set; }
            public decimal Precio { get; set; }
            public int CategoriaId { get; set; }
        }

        public class CreatePlatoCommandHandler : IRequestHandler<CreatePlatoCommandRequest, MessageResult<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CreatePlatoCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<MessageResult<int>> Handle(CreatePlatoCommandRequest request, CancellationToken cancellationToken)
            {
                var (status, categoria, message) = await _unitOfWork.Plato.InsertPlato(request, cancellationToken);

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
