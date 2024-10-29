using System.Net;

namespace Mekatrol.Automatum.Middleware.Exceptions;

public class ForbiddenException : ServiceException
{
    public ForbiddenException() : base(HttpStatusCode.Forbidden)
    {
    }
}
