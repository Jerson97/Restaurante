namespace Restaurant.Domain.Result
{
    public class MessageResult<T>
    {
        public int Code { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }

        private MessageResult(string message, T data, int code)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public static MessageResult<T> Of(string message, T data, int? code = 1) => new MessageResult<T>(message, data, code.Value);

        public static MessageResult<T> Fail(string errorMessage)
        => new MessageResult<T>(errorMessage, default!, 0);
    }
}
