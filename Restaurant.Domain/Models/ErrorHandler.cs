using System.Net;
using System.Runtime.Serialization;

namespace Restaurant.Domain.Models
{
    public class ErrorHandler : Exception
    {
        [IgnoreDataMember]
        public HttpStatusCode Code { get; }

        [DataMember(Name = "message")]
        public override string Message { get; }


        public ErrorHandler(HttpStatusCode code, string message)
        {
            Code = code;
            Message = message;

        }
    }
}
