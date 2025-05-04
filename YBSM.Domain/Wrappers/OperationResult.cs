namespace Core.Domain.Wrappers
{
    // Base class for overall operation status and messages (can be used for errors)
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
           /* Messages = messages;
            TraceId = traceId;*/
        }

        public ResultType Type { get; }
        //public IReadOnlyList<string> Messages { get; } // Using IReadOnlyList for immutability
       // public string TraceId { get; set; }

        // Static factory methods for creating base OperationResult instances (often for operations without a specific data payload, or for errors)
        public static OperationResult Valid(List<string> messages = default, string traceId = null)
         => new(ResultType.Success, messages ?? new List<string>(), traceId);

        public static OperationResult UnValid(string traceId = null, params string[] messages)
         => new(ResultType.Failure, messages.ToList(), traceId);

        public static OperationResult TechError(string traceId = null, params string[] messages)
         => new(ResultType.TechError, messages.ToList(), traceId);

        public static OperationResult UnValid(List<string> messages, ResultType resultType = ResultType.Failure, string traceId = null)
         => new(resultType, messages, traceId);
    }

    // Class to represent a single item within the "Result" list in the desired JSON output
    public class TransactionResultItem
    {
        // Properties must be public with getters/setters for JSON serialization
        public string Code { get; set; }
        public string Message { get; set; }
        public string? TransactionType { get; set; }
    }

    // Modified generic class to hold a LIST of results (of type T) when successful
    // T here will typically be TransactionResultItem to match the desired JSON structure.
    // It inherits from OperationResult to potentially carry status and error messages as well,
    // although the desired success JSON structure doesn't show them at the top level.
    public class OperationResult<T> : OperationResult
    {
        // The constructor now accepts a List<T> for the successful result payload
        // When the Type is not Success, the Result list will be null or empty.
        public OperationResult(ResultType type, List<string> messages, List<T> result, string? traceId = default) :
            base(type, messages, traceId)
        {
            // Only set the Result list if the operation is successful.
            // This matches the common pattern where data payload is only present on success.
            //Result = type == ResultType.Success ? result : null;
            Result = result;
        }

        // This property will hold the list of results.
        // It must be named "Result" to match the desired JSON key.
        // When serialized for a successful operation, this list will be output under the key "Result".
        // For Failure/TechError, if Result is null, it might be omitted from the JSON depending on serializer settings.
        public List<T> Result { get; }

        // --- Static factory methods for creating OperationResult<T> instances ---

        // Factory method for a successful operation with a list of results.
        // This is the method you'll use to create the response that matches the desired JSON structure.
        // T will be TransactionResultItem in this case.
        public static OperationResult<T> Valid(List<T> result, List<string> messages = default)
            => new(ResultType.Success, messages ?? new List<string>(), result);

        // Factory methods for failure/tech error.
        // These reuse the base class logic for status and messages but return OperationResult<T>
        // and explicitly pass default (null for a list) for the result payload.

        public new static OperationResult<T> UnValid(List<string> messages, string traceId = null)
             => new(ResultType.Failure, messages, default, traceId); // Pass default (null) for the result list

        public new static OperationResult<T> UnValid(string traceId = null, params string[] messages)
            => new(ResultType.Failure, messages.ToList(), default, traceId); // Pass default (null) for the result list

        public new static OperationResult<T> UnValid(ResultType resultType, string traceId = null, params string[] messages)
             => new(resultType, messages.ToList(), default, traceId); // Pass default (null) for the result list

        public new static OperationResult<T> TechError(string traceId = null, params string[] messages)
             => new(ResultType.TechError, messages.ToList(), default, traceId); // Pass default (null) for the result list
    }

    // The LocalizedMessage class appears unrelated to the primary response structure
    // and is kept as is based on your original code.
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