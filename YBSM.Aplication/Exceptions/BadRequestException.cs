using System;

namespace YBSM.YaqeenBankSedcoMiddleware.Application.Exceptions
{
    public class BadRequestException: ApplicationException
    {
        public BadRequestException(string message): base(message)
        {

        }
    }
}
