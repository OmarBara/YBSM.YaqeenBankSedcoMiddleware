namespace Core.Domain.Wrappers
{
    public class OperationResult
    {
        public enum ResultType
        {
            Success = 1,
            Failure,
            TechError
        }

        public OperationResult(ResultType type, List<string> messages, string traceId = null)
        {
            Type = type;
            Messages = messages;
            TraceId = traceId;
        }

        public ResultType Type { get; }
        public IReadOnlyList<string> Messages { get; }

        public string TraceId { get; set; }

        public static OperationResult Valid(List<string> messages = default, string traceId = null)
        => new(ResultType.Success, messages ?? new List<string>(), traceId);

        public static OperationResult UnValid(string traceId = null, params string[] messages)
        => new(ResultType.Failure, messages.ToList(), traceId);


        public static OperationResult TechError(string traceId = null, params string[] messages)
        => new(ResultType.TechError, messages.ToList(), traceId);

        public static OperationResult UnValid(List<string> messages, ResultType resultType = ResultType.Failure, string traceId = null)
        => new(resultType, messages, traceId);
    }

    public class OperationResult<T> : OperationResult
    {
        
        public OperationResult(ResultType type, List<string> messages, T result, string? traceId = default) :
            base(type, messages, traceId)
        {
            Result = result;
        }

        public T Result { get; }

        public static OperationResult<T> Valid(T result,
            List<string> messages = default, string traceId = null)
        => new(ResultType.Success, messages ?? new List<string>(), result, traceId);

        public static OperationResult<T> UnValid(List<string> messages, string traceId = null)
        => new(ResultType.Failure, messages, default, traceId);

        public new static OperationResult<T> UnValid(string traceId = null, params string[] messages)
        => new(ResultType.Failure, messages.ToList(), default, traceId);

        public static OperationResult<T> UnValid(ResultType resultType, string traceId = null, params string[] messages)
        => new(resultType, messages.ToList(), default, traceId);
    }

    public class LocalizedMessage
    {
        public enum Langs
        {
            Ar = 1,
            En,
            Fr,
            Es
        }

        public bool State { get; set; }
        public string Message { get; set; }
        public bool IsLocalized { get; set; }
        public object ObjParams { get; set; }
        public Langs Lang { get; set; }

        public static LocalizedMessage From(string message, bool isLocalized = false, object objParams = null, Langs langs = Langs.Ar, bool state = false)
        {
            return new LocalizedMessage
            {
                Message = message,
                IsLocalized = isLocalized,
                ObjParams = objParams,
                Lang = langs,
                State = state
            };
        }
    }
}