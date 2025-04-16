namespace Core.Domain.Enums
{
    public enum ResponseCode
    {
        Ok,
        NotFound,
        InvalidArgument,
        OutOfRange,
        AlreadyExists,
        Aborted,
        Cancelled,
        DataLoss,
        DeadlineExceeded,
        FailedPrecondition,
        Internal,
        PermissionDenied,
        ResourceExhausted,
        Unauthenticated,
        Unavailable,
        Unimplemented,
        Unknown
    }
}