using System.Net;

namespace Mekatrol.Automatum.Middleware.Exceptions;

public class BadRequestException : ServiceException
{
    public BadRequestException() : base(HttpStatusCode.BadRequest)
    {
    }

    public BadRequestException(string error) : base(HttpStatusCode.BadRequest, error)
    {
    }
}
