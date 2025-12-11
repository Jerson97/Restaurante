using System.Net;
using System.Text.Json.Serialization;
using MediatR;
using Restaurant.Application.Interfaces.IUnitOfWork;
using Restaurant.Domain.Enum;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.Application.Features.Plato.Commands.Update
{
    public class UpdatePlatoCommand
    {
        public class UpdatePlatoCommandRequest : IRequest<MessageResult<int>>
        {
            [JsonIgnore]
            public int Id { get; set; }
            public string? Nombre { get; set; }
            public decimal Precio { get; set; }
            public int CategoriaId { get; set; }
        }

        public class UpdatePlatoCommandHandler : IRequestHandler<UpdatePlatoCommandRequest, MessageResult<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public UpdatePlatoCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<MessageResult<int>> Handle(UpdatePlatoCommandRequest request, CancellationToken cancellationToken)
            {
                var (status, categoria, message) = await _unitOfWork.Plato.UpdatePlato(request, cancellationToken);

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
