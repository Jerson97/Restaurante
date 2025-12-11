using System.Net;
using System.Text.Json.Serialization;
using MediatR;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Categoria.Commands.Update
{
    public class UpdateCategoriaCommand
    {
        public class UpdateCategoriaCommandRequest : IRequest<MessageResult<int>>
        {
            [JsonIgnore]
            public int Id { get; set; }
            public string? Nombre { get; set; }
        }

        public class UpdateCategoriaCommandHandler : IRequestHandler<UpdateCategoriaCommandRequest, MessageResult<int>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateCategoriaCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MessageResult<int>> Handle(UpdateCategoriaCommandRequest request, CancellationToken cancellationToken)
            {
                var (status, categoria, message) = await _unitOfWork.Categoria.UpdateCategoria(request, cancellationToken);

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
